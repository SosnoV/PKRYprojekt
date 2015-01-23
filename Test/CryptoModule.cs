using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Test
{
    class CryptoModule
    {
        private static Encoding enc = Encoding.UTF8;
        private static SHA1CryptoServiceProvider sha1Provider = new SHA1CryptoServiceProvider();
        private static string hashFunctionVersion = "SHA1";

        private static RSACryptoServiceProvider myPrivateKey = null;
        private static RSACryptoServiceProvider myPublicKey = null;
        private static Dictionary<string, RSACryptoServiceProvider> otherPublicKeys = new Dictionary<string, RSACryptoServiceProvider>();

        //public CryptoModule() 
        //{
        //    otherPublicKeys = new Dictionary<string, RSACryptoServiceProvider>();
        //    sha1Provider = new SHA1CryptoServiceProvider();
        //}

        public static byte[] HashMessage(string message)
        {
            //SHA1CryptoServiceProvider sha1Provider = new SHA1CryptoServiceProvider(); //mocniejsze sha do uzycia?
            Byte[] dataToHash = enc.GetBytes(message);
            return sha1Provider.ComputeHash(dataToHash);
        }

        public static byte[] HashNTimes(byte[] data, int n)
        {
            byte[] result = null;
            for (int i = 0; i < n; i++)
            {
                result = Hash(data);
            }
            return result;
        }
        public static byte[] Hash(byte[] data)
        {
            return sha1Provider.ComputeHash(data);
        }

        public static bool Verify(byte[] decryptedMsg, byte[] signedMsg, string senderName)
        {
            RSACryptoServiceProvider csp;
            otherPublicKeys.TryGetValue(senderName, out csp);
            return csp.VerifyData(decryptedMsg, hashFunctionVersion, signedMsg);
        }

        public static byte[] Sign(string msg)
        {
            return myPrivateKey.SignData(enc.GetBytes(msg), hashFunctionVersion);
        }

        public static byte[] DecryptMsg(byte[] msg)
        {
            return myPrivateKey.Decrypt(msg, true);
        }

        public static byte[] EncryptMsg(byte[] msg, string receiverName)
        {
            RSACryptoServiceProvider csp;
            if (!otherPublicKeys.TryGetValue(receiverName, out csp))
                return null;
            return csp.Encrypt(msg, true);
        }

        //Import key from certificate
        public static void ImportKey(X509Certificate2 cert, bool isPrivate, bool isMine)
        {
            if (isPrivate)
            {
                myPrivateKey = (RSACryptoServiceProvider)cert.PrivateKey;
            }
            else if (isMine)
            {
                myPublicKey = (RSACryptoServiceProvider)cert.PublicKey.Key;
            }
            else
            {
                string[] array = cert.Subject.Split('=');
                otherPublicKeys.Add(array[1], (RSACryptoServiceProvider)cert.PublicKey.Key);
            }

        }

        public static void RemoveUserKey(string userName)
        {
            otherPublicKeys.Remove(userName);
        }

        public static X509Certificate2 CreatePublicCertFromRawData(byte[] rawData)
        {
            X509Certificate2 cert = new X509Certificate2(rawData);
            return cert;
        }

        public static X509Certificate2 CreatePrivateCertFromRawData(byte[] rawData)
        {
            X509Certificate2 cert = new X509Certificate2(rawData, "pwd", X509KeyStorageFlags.Exportable);
            return cert;
        }
    }
}
