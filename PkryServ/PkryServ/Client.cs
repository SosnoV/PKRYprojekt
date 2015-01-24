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
    public delegate void MsgSignal(object sender, MsgEvent e);
    public class MsgEvent
    {
        public MsgEvent(byte[] data, string log)
        {
            msg = data;
            login = log;
        }
        public byte[] msg { get; private set; }
        public string login { get; private set; }
    }
    class Client
    {
        public Client(string login, byte[] password, byte[] pesel )
        {
            this.login = login;
            passwordHash = password;
            peselHash = pesel;
            n = 1;
            generator = new Random();
            isOnline = false;
            isRunning = false;
            certificate = CryptoModule.GenerateCeriticate(login);
        }
        public bool CheckPesel(byte[] pesel)
        {
            string p1 = null;
            string p2 = null;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pesel)
            {
                sb.Append(b.ToString()).Append(" ");
            }
            p1 = sb.ToString();
            sb.Clear();
            foreach (byte b in peselHash)
            {
                sb.Append(b.ToString()).Append(" ");
            }
            p2 = sb.ToString();

            if (p1.Equals(p2))
                return true;
            return false;
        }
        public int GetN()
        {
            if( n == 1)
            {
                n = generator.Next(5, 15);
                return n;
            }

            return n--;
        }
        public void Show()
        {
            Console.WriteLine("Client: {0}", login);
            Console.Write("Paswword: ");
            foreach(byte b in passwordHash)
            {
                Console.Write(b.ToString() + " ");
            }
            Console.WriteLine();
            Console.Write("Pesel: ");
            foreach (byte b in peselHash)
            {
                Console.Write(b.ToString() + " ");
            }
            Console.WriteLine();
        }
        public string Write()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(login).Append(" ");
            foreach(byte b in passwordHash)
            {
                sb.Append(b.ToString()).Append(" ");
            }
            foreach (byte b in peselHash)
            {
                sb.Append(b.ToString()).Append(" ");
            }
            return sb.ToString();
        }
        public void Run()
        {
            listener = new Thread(Listen);
            listener.IsBackground = true;
            listener.Start();
            isRunning = true;
        }
        private void Listen()
        {
            byte[] buffor = new byte[8192];
            byte[] data = null;
            while(true)
            {
                int bytes = sslStream.Read(buffor, 0, buffor.Length);
                data = new byte[bytes];
                Array.Copy(buffor, data, bytes);
                MsgCame(new MsgEvent(data, login));
            }
        }
        private void MsgCame(MsgEvent e)
        {
            if (msgSignal != null)
                msgSignal(this, e);
        }
        public void Send(byte[] data)
        {
            sslStream.Write(data);
            sslStream.Flush();
        }
        public void Stop()
        {
            listener.Abort();
            isOnline = false;
            isRunning = false;
            sslStream.Close();
            sslStream = null;
        }
 
        Random generator;
        int n;
        Thread listener;
        public bool isOnline { get; set; }
        public bool isRunning { get; set; }
        public string login {get; private set;}
        public string connectedTo { get;  set; }
        public byte[] passwordHash { get; set;}
        public byte[] peselHash {get; set;}
        public X509Certificate2 certificate { get; private set; }
        public SslStream sslStream { get; set; }
        public event MsgSignal msgSignal;
    }
}
