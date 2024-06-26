// Helpers/KeyManager.cs
using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using Newtonsoft.Json;

namespace BankovniSustavApp.Helpers
{
    public class KeyPair
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public static class KeyManager
    {
        private static string _keysFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SignatureKeys.json");

        public static void SaveKeys(KeyPair keys)
        {
            var json = JsonConvert.SerializeObject(keys, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_keysFilePath, json);
        }

        public static KeyPair LoadKeys()
        {
            if (!File.Exists(_keysFilePath))
            {
                return null;
            }

            var json = File.ReadAllText(_keysFilePath);
            return JsonConvert.DeserializeObject<KeyPair>(json);
        }

        public static KeyPair GenerateRsaKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                return new KeyPair
                {
                    PublicKey = rsa.ToXmlString(false),
                    PrivateKey = rsa.ToXmlString(true)
                };
            }
        }
    }
}

