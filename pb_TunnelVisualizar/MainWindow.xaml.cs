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
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;

namespace pb_TunnelVisualizar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Info_Button_OnClick(object sender, RoutedEventArgs e)
        {
            this.StatusBar.Text = "designed and developed by extinctCoder";
            this.DialogHost_TextBlock.Visibility = Visibility.Hidden;
            this.Devloper_Grid.Visibility = Visibility.Visible;
            this.DialogHost.IsOpen = true;
        }

        private void Devloper_Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/extinctCoder");
        }
        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            this.DialogHost_TextBlock.Visibility = Visibility.Visible;
            this.Devloper_Grid.Visibility = Visibility.Hidden;
            this.StatusBar.Text = " ";
        }
    }
}
