using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;
using System.Threading;

namespace PkryServ
{
    public class ClientDatabase
    {
        public ClientDatabase()
        {
            database = new List<Client>();
            try
            {
                ReadFromFile();
            }
            catch
            {

            }
        }
        private void ReadFromFile()
        {
            string[] lines = System.IO.File.ReadAllLines(@"data.txt");
            foreach (string line in lines)
            {
                string[] data = line.Split(' ');
                byte[] password = new byte[20];
                byte[] pesel = new byte[20];
                string login = data[0];

                for(int i = 0; i < 20; ++i)
                {
                    password[i] = Byte.Parse(data[i + 1]);
                }
                for (int i = 0; i < 20; ++i)
                {
                    pesel[i] = Byte.Parse(data[i + 21]);
                }

                Client tmp = new Client(login, password, pesel);
                tmp.msgSignal += new MsgSignal(MsgService);
                database.Add(tmp);
                Console.WriteLine("Client {0} registered" , login);
            }
        }
        public void AddClient(string login, byte[] password, byte[] pesel)
        {
            foreach(Client client in database)
            {
                if (login == client.login || client.CheckPesel(pesel))
                    throw new Exception("REG: Login or Pesel in use");
            }
            Client tmp = new Client(login, password, pesel);
            tmp.msgSignal += new MsgSignal(MsgService);
            database.Add(tmp);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"data.txt", true))
            {
                file.WriteLine(tmp.Write());                
            }
            Console.WriteLine("Client {0} registered, hash lenghth: {1}", login, password.Length);            
        }
        public X509Certificate2 GetCertificate(string login)
        {
            foreach(Client c in database)
            {
                if (c.login == login)
                    return c.certificate;
            }
            X509Certificate2 wrong = null;
            return wrong;
        }
        private void MsgService(object sender, MsgEvent e)
        {
            StringBuilder sb = new StringBuilder();
            Decoder decoder = Encoding.UTF8.GetDecoder();

            char[] chars = new char[decoder.GetCharCount(e.msg, 0, e.msg.Length)];
            decoder.GetChars(e.msg, 0, e.msg.Length, chars, 0);
            sb.Append(chars);

            string message = sb.ToString();

            if (String.Compare("ONL", 0, message, 0, 3) == 0)
            {
                string result = null;
                foreach(Client c in database)
                {
                    if(c.isRunning)
                    {
                        result += c.login + " ";
                    }
                }
                byte[] msg;
                if(result == null)
                    msg = Encoding.UTF8.GetBytes("ONLRNikt nie jest online");
                else
                    msg = Encoding.UTF8.GetBytes("ONLR"+result);
                foreach(Client c in database)
                {
                    if (c.login == e.login)
                        c.Send(msg);               
                }
                return;
            }
            else if(String.Compare("ISO", 0, message, 0, 3) == 0)
            {
                string login = message.Substring(3);
                bool isOn = false;
                string result;
                foreach(Client c in database)
                {
                    if(c.login == login && c.isRunning)
                    {
                        isOn = true;
                        break;
                    }
                }
                if (isOn)
                {
                    result = "ISORTrue";
                    foreach(Client c in database)
                    {
                        if(c.login == e.login)
                        {
                            c.connectedTo = login;
                            break;
                        }
                    }
                }
                else
                    result = "ISORFalse";

                byte[] msg = Encoding.UTF8.GetBytes(result);
                foreach (Client c in database)
                {
                    if (c.login == e.login)
                        c.Send(msg);
                }
                return;
            }
            else if(String.Compare("GCR", 0, message, 0, 3) == 0)
            {
                X509Certificate2 cerAlice = null;
                X509Certificate2 cerBob = null;
                string alice = e.login;
                string bob = null;                
                foreach(Client c in database)
                {
                    if(c.login == alice)
                    {
                        bob = c.connectedTo;
                        break;
                    }
                }

                //Pobranie odpowiednich certyfikatow
                foreach(Client c in database)
                {
                    if (c.login == alice)
                        cerAlice = c.certificate;
                    else if (c.login == bob)
                        cerBob = c.certificate;
                }
                //Przygotowanie wiadomości dla Boba
                //sb.Clear();
                string prefix = "GCRB";
                int bytes = Encoding.UTF8.GetByteCount(prefix);
                //sb.Append(prefix).Append(bytes);
                //prefix = sb.ToString();
                //bytes = Encoding.UTF8.GetByteCount(prefix);
                byte[] pre = Encoding.UTF8.GetBytes(prefix);
                byte[] rawData = CryptoModule.PreparePublicCertToSend(cerAlice);
                byte[] bobMsg = new byte[rawData.Length + bytes];
                pre.CopyTo(bobMsg, 0);
                rawData.CopyTo(bobMsg, bytes);

                //Przygotowanie wiadomości dla Alice
                //sb.Clear();
                prefix = "GCRA";
                bytes = Encoding.UTF8.GetByteCount(prefix);
                //sb.Append(prefix).Append(bytes);
                //prefix = sb.ToString();
                //bytes = Encoding.UTF8.GetByteCount(prefix);
                byte[] preA = Encoding.UTF8.GetBytes(prefix);
                byte[] rawDataA = CryptoModule.PreparePublicCertToSend(cerBob);
                byte[] aliceMsg = new byte[rawDataA.Length + bytes];
                preA.CopyTo(aliceMsg, 0);
                rawDataA.CopyTo(aliceMsg, bytes);

                //Wyslanie wiadomosci
                foreach(Client c in database)
                {
                    if (c.login == alice)
                        c.Send(aliceMsg);
                    else if (c.login == bob)
                        c.Send(bobMsg);
                }
                return;
            }
        }
        public void RunClient()
        {
            foreach(Client client in database)
            {
                if (client.isOnline && !client.isRunning)
                    client.Run();
            }
        }
        public bool LogClient(string login, byte[] password, int n)
        {
            foreach (Client client in database)
            {
                if (login == client.login)
                {
                    string pass = null;
                    foreach(byte b in password)
                    {
                        pass += (b.ToString() + " ");
                    }
                    --n;

                    byte[] hash = CryptoModule.HashNTimes(client.passwordHash, n);
                    string h = null;
                    foreach (byte b in hash)
                    {
                        h += (b.ToString() + " ");
                    }

                    if (h.Equals(pass))
                    {
                        Console.WriteLine("LOG: Logged {0}", login);
                        client.isOnline = true;
                        return true;
                    }
                }
            }
            return false;
        }
        public int ClientN(string login)
        {
            foreach (Client client in database)
            {
                if (login == client.login)
                    return client.GetN();
            }
            return 0;
        }
        public void AddEndPoint(string login, SslStream endPoint)
        {
            foreach (Client client in database)
            {
                if (login == client.login)
                {
                    client.sslStream = endPoint;
                    client.isOnline = true;
                    break;
                }
            }
        }
        List<Client> database;
    }
}
