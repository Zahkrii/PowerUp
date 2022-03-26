using HandyControl.Controls;
using HandyControl.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PowerUp.Tools;
using System.Windows.Forms;

namespace PowerUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            NavFrame.Focus();
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SideMenu_SelectionChanged(object sender, FunctionEventArgs<object> e)
        {
            foreach (SideMenuItem a in MainSideMenu.Items)
            {
                if (a.IsSelected)
                {
                    foreach (SideMenuItem b in a.Items)
                    {
                        if (b.IsSelected)
                        {
                            NavFrame.Navigate(new Uri("/Pages/" + b.Tag.ToString() + "Page.xaml", UriKind.Relative));
                        }
                    }
                }
            }
        }
    }
}