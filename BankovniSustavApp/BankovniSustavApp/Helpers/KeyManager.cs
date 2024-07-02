using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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
        private static string _publicKeyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PublicKey.json");
        private static string _privateKeysFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "PrivateKeys.json");
        private static Dictionary<int, string> _privateKeysDictionary = new Dictionary<int, string>();

        static KeyManager()
        {
            LoadPublicKey();
            LoadAllPrivateKeys();
        }

        private static void LoadPublicKey()
        {
            if (File.Exists(_publicKeyFilePath))
            {
                var json = File.ReadAllText(_publicKeyFilePath);
                PublicKey = JsonConvert.DeserializeObject<string>(json);
            }
        }

        private static void SavePublicKey(string publicKey)
        {
            var json = JsonConvert.SerializeObject(publicKey, Formatting.Indented);
            File.WriteAllText(_publicKeyFilePath, json);
        }

        private static void LoadAllPrivateKeys()
        {
            if (File.Exists(_privateKeysFilePath))
            {
                var json = File.ReadAllText(_privateKeysFilePath);
                _privateKeysDictionary = JsonConvert.DeserializeObject<Dictionary<int, string>>(json) ?? new Dictionary<int, string>();
            }
        }

        private static void SaveAllPrivateKeys()
        {
            var json = JsonConvert.SerializeObject(_privateKeysDictionary, Formatting.Indented);
            File.WriteAllText(_privateKeysFilePath, json);
        }

        public static void SaveKeysForUser(int userId, KeyPair keys)
        {
            if (string.IsNullOrEmpty(PublicKey))
            {
                SavePublicKey(keys.PublicKey);
            }
            _privateKeysDictionary[userId] = keys.PrivateKey;
            SaveAllPrivateKeys();
        }

        public static string LoadPrivateKeyForUser(int userId)
        {
            _privateKeysDictionary.TryGetValue(userId, out var privateKey);
            return privateKey;
        }

        public static bool KeysExistForUser(int userId)
        {
            return _privateKeysDictionary.ContainsKey(userId);
        }
        
        public static string PublicKey { get; private set; }

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
