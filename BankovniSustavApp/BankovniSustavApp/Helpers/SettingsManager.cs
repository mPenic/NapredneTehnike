using System;
using System.IO;
using IniParser;
using IniParser.Model;
using Microsoft.Win32;

namespace BankovniSustavApp.Helpers
{
    public class SettingsManager
    {
        private string _iniPath;
        private bool _useRegistry;
        private const string RegistryPath = @"Software\BankovniSustavApp";

        public SettingsManager(string iniPath, bool useRegistry)
        {
            _iniPath = iniPath;
            _useRegistry = useRegistry;
        }

        public string ReadSetting(string section, string key)
        {
            if (_useRegistry)
            {
                return ReadFromRegistry(section, key);
            }
            else
            {
                return ReadFromIniFile(section, key);
            }
        }

        public void WriteSetting(string section, string key, string value)
        {
            if (_useRegistry)
            {
                WriteToRegistry(section, key, value);
            }
            else
            {
                WriteToIniFile(section, key, value);
            }
        }

        private string ReadFromIniFile(string section, string key)
        {
            if (!File.Exists(_iniPath))
            {
                // Handle the case where the INI file doesn't exist
                return string.Empty;
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(_iniPath);

            return data[section][key];
        }

        private void WriteToIniFile(string section, string key, string value)
        {
            var parser = new FileIniDataParser();
            IniData data = new IniData();

            if (File.Exists(_iniPath))
            {
                data = parser.ReadFile(_iniPath);
            }
            else
            {
                data = new IniData(); // Create new if not exists
            }

            data[section][key] = value;
            parser.WriteFile(_iniPath, data);
        }

        private string ReadFromRegistry(string section, string key)
        {
            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath + "\\" + section))
            {
                if (registryKey != null)
                {
                    return registryKey.GetValue(key)?.ToString() ?? string.Empty;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void WriteToRegistry(string section, string key, string value)
        {
            using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath + "\\" + section))
            {
                registryKey.SetValue(key, value);
            }
        }
    }
}
