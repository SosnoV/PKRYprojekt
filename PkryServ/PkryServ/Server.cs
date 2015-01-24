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
    public class Server
    {
        public Server(string name)
        {
            serverName = name;
            database = new ClientDatabase();
            serverCertificate = null;
        }
        public void Run()
        {
            serverCertificate = new X509Certificate2("C:/Users/Tomasz/Documents/pkry.pfx", "pkrypass");
            Listen();
        }
        private void Listen()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            while(true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }
        private void ProcessClient(TcpClient client)
        {
            SslStream sslStream = new SslStream(client.GetStream(), false);
            try 
            {
                sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                int register = ReadOpeningMessage(sslStream, (IPEndPoint)client.Client.RemoteEndPoint);

                if (register == 2)
                {
                    byte[] result = Encoding.UTF8.GetBytes("1");

                    sslStream.Write(result);
                    sslStream.Flush();
                    sslStream.Close();
                    client.Close();
                    return;
                }
                else if (register == 1)
                {
                    byte[] buffer = new byte[128];
                    int bytes = sslStream.Read(buffer, 0, buffer.Length);
                    char[] chars = new char[Encoding.UTF8.GetCharCount(buffer, 0, bytes)];
                    Encoding.UTF8.GetChars(buffer, 0, bytes, chars, 0);

                    StringBuilder messageData = new StringBuilder();
                    messageData.Append(chars);
                    string msg = messageData.ToString();
                    string login = msg.Substring(3);

                    X509Certificate2 cer = database.GetCertificate(login);
                    byte[] certificate;
                    if (cer != null)
                        certificate = CryptoModule.PreparePrivateCertToSend(cer);
                    else
                        throw new Exception();

                    sslStream.Write(certificate);
                    sslStream.Flush();

                    database.RunClient();
                }
                else
                    throw new Exception("Something went wrong");

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                byte[] result = Encoding.UTF8.GetBytes("0");
                sslStream.Write(result);
                sslStream.Flush();
                sslStream.Close();
                client.Close();                
            }
        }
        private int ReadOpeningMessage(SslStream sslStream, IPEndPoint adress)
        {
            byte[] buffer = new byte[2048];
            StringBuilder sb = new StringBuilder();
            Decoder decoder = Encoding.UTF8.GetDecoder();
            int bytes;
 
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
            decoder.GetChars(buffer, 0, bytes, chars, 0);
            sb.Append(chars);

            string message = sb.ToString();

            if (String.Compare("REG", 0, message, 0, 3) == 0)
            {
                byte[] password = new byte[20];
                byte[] pesel = new byte[20];

                Array.Copy(buffer, 3, password, 0, 20);
                Array.Copy(buffer, 23, pesel, 0, 20);;
                string login = message.Substring(43);

                try
                {
                    database.AddClient(login, password, pesel);
                }
                catch (Exception e)
                {
                    throw e;
                }
                return 2;
            }
            else if (String.Compare("LOG", 0, message, 0, 3) == 0)
            {
                string login = message.Substring(3);
                int n = database.ClientN(login);
                sslStream.Write(Encoding.UTF8.GetBytes(n.ToString()));
                sslStream.Flush();
                if (n == 0)
                    throw new Exception("LOG: No login in database");

                byte[] pwd = new byte[20];
                int bytes2 = sslStream.Read(pwd, 0, pwd.Length);
                bool result = database.LogClient(login, pwd, --n);

                if (result)
                {
                    byte[] r = Encoding.UTF8.GetBytes("1");
                    sslStream.Write(r);
                    sslStream.Flush();
                    database.AddEndPoint(login, sslStream);
                    return 1;
                }
                throw new Exception("LOG: Wrong password");
            }
            else
                throw new Exception("Wrong Comand");
        }

        static X509Certificate2 serverCertificate;
        ClientDatabase database;
        string serverName;
    }
}
