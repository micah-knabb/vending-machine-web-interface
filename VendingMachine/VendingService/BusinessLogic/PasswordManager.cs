using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VendingService
{
    /// <summary>
    /// Used to manage password verification, hash generation, salt generation, and encryption
    /// </summary>
    public class PasswordManager
    {
        private const int _workFactor = 200;
        private const int _saltSize = 16;

        public string Salt { get; private set; }
        public string Hash { get; private set; }

        //Use when Registering a new user
        public PasswordManager(string password)
        {
            Salt = GenerateSalt(password);
            Hash = GenerateHash(password);
        }

        //Use this when verifying an existing user
        public PasswordManager(string password, string salt)
        {
            Salt = salt;
            Hash = GenerateHash(password);
        }

        public bool Verify(string hash)
        {
            return Hash == hash;
        }

        //generates own salt
        private string GenerateSalt(string password)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, _saltSize, _workFactor);
            return Convert.ToBase64String(rfc.Salt);
        }

        private string GenerateHash(string password)
        {
            Rfc2898DeriveBytes rfc = HashPasswordWithPBKDF2(password, Salt);
            return Convert.ToBase64String(rfc.GetBytes(20));
        }

        private Rfc2898DeriveBytes HashPasswordWithPBKDF2(string password, string salt)
        {
            // Creates the crypto service provider and provides the salt - usually used to check for a password match
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), _workFactor);

            return rfc2898DeriveBytes;
        }

        public static string DecryptCipherText(string cipherText, string privateKeyPath)
        {
            RSACryptoServiceProvider cipher = new RSACryptoServiceProvider();
            cipher.FromXmlString(System.IO.File.ReadAllText(privateKeyPath));
            byte[] original = cipher.Decrypt(Convert.FromBase64String(cipherText), false);
            return Encoding.UTF8.GetString(original);
        }

        public static string GetCipherText(string plaintext, string publicKeyPath)
        {
            RSACryptoServiceProvider cipher = new RSACryptoServiceProvider();
            cipher.FromXmlString(System.IO.File.ReadAllText(publicKeyPath));
            byte[] data = Encoding.UTF8.GetBytes(plaintext);
            byte[] cipherText = cipher.Encrypt(data, false);
            return Convert.ToBase64String(cipherText);
        }

        public static void GenerateKeys(string publicKeyFileName, string privateKeyFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamWriter publicKeyFile = null;
            StreamWriter privateKeyFile = null;

            string publicKey = "";
            string privateKey = "";

            try
            {
                // Create a new key pair on target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL
                //cspParams.ProviderName; // CSP name

                cspParams.Flags = CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)KeyNumber.Exchange;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Export public key
                publicKey = rsaProvider.ToXmlString(false);

                // Write public key to file 
                publicKeyFile = System.IO.File.CreateText(publicKeyFileName);
                publicKeyFile.Write(publicKey);

                // Export private/public key pair
                privateKey = rsaProvider.ToXmlString(true);

                // Write private/public key pair to file
                privateKeyFile = System.IO.File.CreateText(privateKeyFileName);
                privateKeyFile.Write(privateKey);
            }
            finally
            {
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }

                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
            }
        }
        
    }
}
