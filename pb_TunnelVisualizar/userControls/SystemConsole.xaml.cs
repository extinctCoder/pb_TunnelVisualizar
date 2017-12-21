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

namespace pb_TunnelVisualizar.userControls
{
    /// <summary>
    /// Interaction logic for SystemConsole.xaml
    /// </summary>
    public partial class SystemConsole : UserControl
    {
        private static SystemConsole _console = null;
        private SystemConsole()
        {
            InitializeComponent();
        }
        public static SystemConsole console
        {
            get
            {
                if (SystemConsole._console == null)
                {
                    SystemConsole._console = new SystemConsole();
                }
                return SystemConsole._console;
            }

        }
        public static void setConsoleTxt(string content)
        {
            console.StatusBar.Text = content.ToString();
        }
        public static void setConsoleTxtThreadSafe(string content)
        {
            try
            {
                lock (SystemConsole.console)
                {
                    SystemConsole.console.StatusBar.Dispatcher.Invoke(new Action(() => SystemConsole.console.StatusBar.Text = content));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
