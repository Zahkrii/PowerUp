using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PowerUp.Windows
{
    /// <summary>
    /// OCRWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OCRWindow : Window
    {
        public OCRWindow()
        {
            InitializeComponent();

            DetectedTextBox.Visibility = Visibility.Collapsed;
            LoadingBar.Visibility = Visibility.Visible;
        }

        public void UpdateText(string base64str)
        {
            DetectedTextBox.Text = base64str;
            LoadingBar.Visibility = Visibility.Collapsed;
            DetectedTextBox.Visibility = Visibility.Visible;
            DetectedTextBox.Focus();
        }

        public void UpdateImage(ImageSource screenshot)
        {
            SnapImage.Source = screenshot;
        }
    }
}