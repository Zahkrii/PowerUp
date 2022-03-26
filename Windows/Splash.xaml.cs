using PowerUp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PowerUp.Windows
{
    /// <summary>
    /// Splash.xaml 的交互逻辑
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingsTool.Instance.Read();
            }
            catch
            {
                HandyControl.Controls.MessageBox.Show("Failed to read the configuration file \"settings.json\", please check if the file exists. \n" +
                    "reinstalling the software will solve the problem.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (SettingsTool.Instance.AppSettings.SnapOCRSettings.SnapOCREnabled)
            {
                Hotkey hot = new Hotkey(this, SettingsTool.Instance.AppSettings.SnapOCRSettings.ControlKeyCode, SettingsTool.Instance.AppSettings.SnapOCRSettings.Keys);
                hot.OnHotKey += SnapOCROnHotKey;
            }

            Window mainWindow = new MainWindow();
            System.Windows.Application.Current.MainWindow = mainWindow;
            this.Hide();
        }

        private void SnapOCROnHotKey()
        {
            if (string.IsNullOrEmpty(SettingsTool.Instance.AppSettings.SnapOCRSettings.SecretID)
                || string.IsNullOrEmpty(SettingsTool.Instance.AppSettings.SnapOCRSettings.SecretKey))
            {
                HandyControl.Controls.MessageBox.Show("API Configration is missing!\nPlease go to settings and configure API interface",
                                                      "Snap OCR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SnapOCR snapOCR = new SnapOCR();
                snapOCR.Snap();
            }
        }
    }
}