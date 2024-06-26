using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IniParser;
using IniParser.Model;

namespace BankovniSustavApp.Helpers
{
    public class SettingsManager
    {
        private string _iniPath;

        public SettingsManager(string iniPath)
        {
            _iniPath = iniPath;
        }

        public string ReadSetting(string section, string key)
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


        public void WriteSetting(string section, string key, string value)
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
    }

}
