using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using pb_TunnelVisualizar.userControls;

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
            this.ModuleInitializer();
            this.loadModule();
        }

        private void ModuleInitializer()
        {
            Thread systemConsoleThread = new Thread(threadedSystemConsoleInitializerFunction);
            systemConsoleThread.Name = "systemConsoleThread";
            systemConsoleThread.IsBackground = true;
            systemConsoleThread.Start();

            Thread mapControlThread = new Thread(threadedMapControlInitializerFunction);
            mapControlThread.Name = "mapControlThread";
            mapControlThread.IsBackground = true;
            mapControlThread.Start();
        }

        void loadModule()
        {
            try
            {
                using (var db = new db.pb_TunnelVisualizarDatabaseEntities())
                {
                    if (db.sensors.Count() > 0)
                    {
                        foreach (var dbSensor in db.sensors)
                        {
                            this.sensor_unit_grid.Children.Add(new SensorUnit(dbSensor));
                        }
                    }
                    else
                    {
                        this.sensor_unit_grid.Children.Add(new StackPanel()
                        {
                            Children = { new Label()
                            {
                                Content = "no component added in database"
                            }}
                        });
                    }
                }
            }
            catch (Exception e)
            {
                this.sensor_unit_grid.Children.Add(new StackPanel()
                {
                    Children = { new Label()
                    {
                        Content = e.Message
                    }}
                });
                SystemConsole.setConsoleTxt(e.Message);
            }
            
            
        }

        private void threadedSystemConsoleInitializerFunction()
        {
            this.console_grid.Dispatcher.Invoke((Action) (() =>
            {
                this.console_grid.Children.Add(SystemConsole.console);
            }));
        }
        private void threadedMapControlInitializerFunction()
        {
            this.map_grid.Dispatcher.Invoke((Action)(() =>
            {
                this.map_grid.Children.Add(new MapControl());
            }));
        }
        private void Info_Button_OnClick(object sender, RoutedEventArgs e)
        {
            SystemConsole.setConsoleTxtThreadSafe("designed and developed by extinctCoder");
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
            SystemConsole.setConsoleTxtThreadSafe(" ");
        }
    }
}
