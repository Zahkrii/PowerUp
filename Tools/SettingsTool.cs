using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PowerUp.Tools
{
    internal class SettingsTool
    {
        private static SettingsTool instance;

        public static SettingsTool Instance
        {
            get
            {
                if (instance == null)
                    instance = new SettingsTool();
                return instance;
            }
        }

        private const string SettingsFilename = "settings.json";

        private string _DefaultSettingspath =
            AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
            "Data\\" + SettingsFilename;

        //public string _UserSettingsPath =
        //    Assembly.GetEntryAssembly().Location +
        //    "\\Settings\\UserSettings\\" +
        //    UserSettingsFilename;

        public AppSettings AppSettings;

        public bool Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_DefaultSettingspath))
                {
                    string jsonStr = JsonConvert.SerializeObject(AppSettings);
                    foreach (char s in jsonStr)
                    {
                        sw.Write(s);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Read()
        {
            try
            {
                using (StreamReader sw = new StreamReader(_DefaultSettingspath))
                {
                    AppSettings = JsonConvert.DeserializeObject<AppSettings>(sw.ReadToEnd());
                }
                return AppSettings != null;
            }
            catch
            {
                return false;
            }
        }
    }

    public class AppSettings
    {
        public GlobalSettings GlobalSettings { get; set; }
        public SnapOCRSettings SnapOCRSettings { get; set; }
    }

    public class GlobalSettings
    {
    }

    public class SnapOCRSettings
    {
        public bool SnapOCREnabled { get; set; }
        public Hotkey.ControlKeyCode ControlKeyCode { get; set; }
        public System.Windows.Forms.Keys Keys { get; set; }
        public string API { get; set; }
        public string SecretID { get; set; }
        public string SecretKey { get; set; }
    }
}