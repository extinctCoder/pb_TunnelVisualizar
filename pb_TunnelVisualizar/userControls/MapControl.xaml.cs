using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace pb_TunnelVisualizar.userControls
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        public static MapControl _mapControl = null;
        public static PointLatLng _startingPosition;
        public static PointLatLng _endingPosition;
        public static GMapMarker _currentMarker;

        public static MapControl mapControl
        {
            get
            {
                if (MapControl._currentMarker == null)
                {
                    MapControl._mapControl = new MapControl();
                }
                return MapControl._mapControl;
            }
        }

        private MapControl()
        {
            InitializeComponent();
            if (!PingNetwork("pingtest.com"))
            {
                map_viw.Manager.Mode = AccessMode.CacheOnly;
                SystemConsole.setConsoleTxtThreadSafe("No Internet connection available, going to CacheOnly mode");
            }
            else
            {
                SystemConsole.setConsoleTxtThreadSafe("Internet connection available, going to normal mode");
            }
            map_viw.MapProvider = GMapProviders.OpenStreetMap;
            map_viw.Position = new PointLatLng(23.8103, 90.4125);
            map_viw.Zoom = 10;

            map_type_combo_bx.ItemsSource = GMapProviders.List;
            map_type_combo_bx.SelectedItem = map_viw.MapProvider;

            map_mode_combo_bx.ItemsSource = Enum.GetValues(typeof(AccessMode));
            map_mode_combo_bx.SelectedItem = map_viw.Manager.Mode;

            routing_CK_bx.IsChecked = map_viw.Manager.UseRouteCache;
            geo_coding_ck_bx.IsChecked = map_viw.Manager.UseGeocoderCache;

            current_marker_cheack_bx.IsChecked = true;
            drag_map_cheack_bx.IsChecked = map_viw.CanDragMap;
            grid_cheack_bx.IsChecked = true;

            MapControl._currentMarker = new GMapMarker(map_viw.Position);
            {
                MapControl._currentMarker.Shape = new Rectangle
                {
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1.5
                };
            }
        }

        private void zom_slidr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            map_viw.Zoom = ((int) zom_slidr.Value);
            SystemConsole.setConsoleTxtThreadSafe("Map Zoomed In : " + zom_slidr.Value.ToString() + "x");
        }

        private void reload_btn_Click(object sender, RoutedEventArgs e)
        {
            map_viw.ReloadMap();
            SystemConsole.setConsoleTxtThreadSafe("Map Re-Initialized");
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemConsole.setConsoleTxtThreadSafe("Trying To Save Map Snapshot In Desktop Folder");
                ImageSource imageSource = map_viw.ToImageSource();
                PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.FileName = "MAP " + DateTime.Today.ToString();
                saveFileDialog.DefaultExt = ".png"; // Default file extension
                saveFileDialog.Filter = "Image (.png)|*.png"; // Filter files by extension
                saveFileDialog.AddExtension = true;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string filename = saveFileDialog.FileName;
                    using (System.IO.Stream stream = System.IO.File.OpenWrite(filename))
                    {
                        pngBitmapEncoder.Save(stream);
                    }
                }
                SystemConsole.setConsoleTxtThreadSafe("Successfully Done Save Map Snapshot In Desktop Folder Process");
            }
            catch (Exception ex)
            {
                SystemConsole.setConsoleTxtThreadSafe(ex.Message);
            }
        }

       

        private void export_btn_Click(object sender, RoutedEventArgs e)
        {
            SystemConsole.setConsoleTxtThreadSafe("Saving The Map Cache In Local Storage");
            map_viw.ShowExportDialog();
        }

        private void clear_all_cache_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are You Sure To Clear The Map Cache?\nNo Data Will Be Saved In Local Storage",
                    "Map Cache Clear Warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    map_viw.Manager.PrimaryCache.DeleteOlderThan(DateTime.Now, null);
                    SystemConsole.setConsoleTxtThreadSafe("Successfully Cleared The Map Cache");
                }
                catch (Exception ex)
                {
                    SystemConsole.setConsoleTxtThreadSafe(ex.ToString());
                }
            }
        }

        private void prefetch_btn_Click(object sender, RoutedEventArgs e)
        {
            RectLatLng rectLatLng = map_viw.SelectedArea;
            if (!rectLatLng.IsEmpty)
            {
                for (int i = (int) map_viw.Zoom; i <= map_viw.MaxZoom; i++)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Ready Prefetch The Map At Zoom = " + i + " ?",
                        "Prefetch", MessageBoxButton.YesNoCancel);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        TilePrefetcher tilePrefetcher = new TilePrefetcher();
                        tilePrefetcher.ShowCompleteMessage = true;
                        tilePrefetcher.Start(rectLatLng, i, map_viw.MapProvider, 100);
                    }
                    else if (messageBoxResult == MessageBoxResult.No)
                    {
                        continue;
                    }
                    else if (messageBoxResult == MessageBoxResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else
            {
                SystemConsole.setConsoleTxtThreadSafe("Select The Map Area Holding ALT");
            }
        }

        private void import_btn_Click(object sender, RoutedEventArgs e)
        {
            SystemConsole.setConsoleTxtThreadSafe("Importing The Map Cache From The Local Storage");
            map_viw.ShowImportDialog();
        }

        private void current_marker_cheack_bx_Checked(object sender, RoutedEventArgs e)
        {
            if (MapControl._currentMarker != null)
            {
                map_viw.Markers.Add(MapControl._currentMarker);
                SystemConsole.setConsoleTxtThreadSafe("Current Marker Initialized");
            }
        }

        private void current_marker_cheack_bx_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MapControl._currentMarker != null)
            {
                map_viw.Markers.Remove(MapControl._currentMarker);
                SystemConsole.setConsoleTxtThreadSafe("Current Marker De Initialized");
            }
        }

        private void grid_cheack_bx_Checked(object sender, RoutedEventArgs e)
        {
            map_viw.ShowTileGridLines = true;
            SystemConsole.setConsoleTxtThreadSafe("Grid System Initialized");
        }

        private void grid_cheack_bx_Unchecked(object sender, RoutedEventArgs e)
        {
            map_viw.ShowTileGridLines = false;
            SystemConsole.setConsoleTxtThreadSafe("Grid System De Initialized");
        }

        private void drag_map_cheack_bx_Checked(object sender, RoutedEventArgs e)
        {
            map_viw.CanDragMap = true;
            SystemConsole.setConsoleTxtThreadSafe("Drag Map Ability Initialized");
        }

        private void drag_map_cheack_bx_Unchecked(object sender, RoutedEventArgs e)
        {
            map_viw.CanDragMap = false;
            SystemConsole.setConsoleTxtThreadSafe("Drag Map Ability De Initialized");
        }

        private void map_viw_MouseEnter(object sender, MouseEventArgs e)
        {
            map_viw.Focus();
            SystemConsole.setConsoleTxtThreadSafe("Map Gets Focus");
        }

        private void map_viw_MouseMove(object sender, MouseEventArgs e)
        {
            if (MapControl.mapControl.current_marker_cheack_bx.IsChecked == true &&
                e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                MapControl.mapControl.map_viw.Markers.Remove(MapControl._currentMarker);
                System.Windows.Point p = e.GetPosition(map_viw);
                MapControl._currentMarker.Position = map_viw.FromLocalToLatLng((int) p.X, (int) p.Y);
                map_viw.Markers.Add(MapControl._currentMarker);
                SystemConsole.setConsoleTxtThreadSafe("Current Position Of Marker Is : " + p.X.ToString() + ", " +
                                                      p.Y.ToString());
            }
        }

        private void map_viw_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MapControl.mapControl.current_marker_cheack_bx.IsChecked == true)
            {
                MapControl.mapControl.map_viw.Markers.Remove(MapControl._currentMarker);
                System.Windows.Point p = e.GetPosition(map_viw);
                MapControl._currentMarker.Position = map_viw.FromLocalToLatLng((int) p.X, (int) p.Y);
                map_viw.Markers.Add(MapControl._currentMarker);
                SystemConsole.setConsoleTxtThreadSafe("Current Position Of Marker Is : " + p.X.ToString() + ", " +
                                                      p.Y.ToString());
            }
        }

        private void map_viw_OnTileLoadStart()
        {
            //SystemConsole.setConsoleTxtThreadSafe("New tiles are downloading from server ...");
            //try
            //{
            //    if (MapControl.mapControl != null)
            //    {
            //        //loading_br.IsIndeterminate = true;
            //        SystemConsole.setConsoleTxtThreadSafe("New tiles are downloading from server ...");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SystemConsole.setConsoleTxtThreadSafe(ex.Message);
            //}
        }

        private void map_viw_OnTileLoadComplete(long ElapsedMilliseconds)
        {
            //SystemConsole.setConsoleTxtThreadSafe("Map Load Completed In : " + ElapsedMilliseconds.ToString() + " Milliseconds");
            //try
            //{
            //    if (MapControl.mapControl != null)
            //    {
            //        //loading_br.IsIndeterminate = false;
            //        SystemConsole.setConsoleTxtThreadSafe("Map Load Completed In : " + ElapsedMilliseconds.ToString() + " Milliseconds");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SystemConsole.setConsoleTxtThreadSafe(ex.Message);
            //}
        }

        private void map_type_combo_bx_DropDownClosed(object sender, EventArgs e)
        {
            map_viw.MapProvider = (GMapProvider) map_type_combo_bx.SelectedItem;
            map_viw.ReloadMap();
        }

        private void map_mode_combo_bx_DropDownClosed(object sender, EventArgs e)
        {
            map_viw.Manager.Mode = (AccessMode) map_mode_combo_bx.SelectedItem;
            map_viw.ReloadMap();
        }

        public static bool PingNetwork(string hostNameOrAddress)
        {
            using (Ping p = new Ping())
            {
                byte[] buffer = Encoding.ASCII.GetBytes("network test string");
                int timeout = 4444; // 4s

                try
                {
                    PingReply reply = p.Send(hostNameOrAddress, timeout, buffer);
                    return (reply.Status == IPStatus.Success) ? true : false;
                }
                catch (Exception ex)
                {
                    SystemConsole.setConsoleTxtThreadSafe(ex.Message);
                }
            }
            return false;
        }
    }
}
