using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using pb_TunnelVisualizar.db;
using MahApps.Metro;
using MaterialDesignThemes;

namespace pb_TunnelVisualizar.userControls
{
    /// <summary>
    /// Interaction logic for SensorUnit.xaml
    /// </summary>
    public partial class SensorUnit : UserControl
    {
        private sensor _sensor;
        private data_waterFlow _dataWaterFlow;
        private data_waterLavel _dataWaterLavel;
        private Thread dataUpatderThread;
        private bool flag_1 = true;
        private bool flag_2 = true;
        public SensorUnit()
        {
            InitializeComponent();
        }

        public SensorUnit(db.sensor _sensor)
        {
            try
            {
                this.InitializeComponent();
                this._sensor = _sensor;
                this.sensor_id_label.Content = this._sensor.iddata.ToString();
                this.sensor_description_label.Content =
                    this._sensor.description == null ? "null" : this._sensor.description.ToString();
                this.position_x_label.Content =
                    this._sensor.position_x == null ? "null" : this._sensor.position_x.ToString();
                this.position_y_label.Content = this._sensor.position_y == null ? "null" : this._sensor.position_y;

                if (this._sensor.data_waterFlow.Count() > 0)
                {
                    this._dataWaterFlow = this._sensor.data_waterFlow.Last();
                    this.flow_rate_label.Content =
                        this._dataWaterFlow.data == null ? "null" : this._dataWaterFlow.data.ToString();
                }
                if (this._sensor.data_waterLavel.Count() > 0)
                {
                    this._dataWaterLavel = this._sensor.data_waterLavel.Last();
                    this.water_height_label.Content =
                        this._dataWaterLavel.data == null ? "null" : this._dataWaterLavel.data.ToString();
                }
                this.dataUpdater();
            }
            catch (Exception e)
            {
                SystemConsole.setConsoleTxt(e.Message);
            }
        }

        private void dataUpdater()
        {
            this.dataUpatderThread = new Thread(dataUpdaterFunction);
            this.dataUpatderThread.Name = "dataUpatderThread";
            this.dataUpatderThread.IsBackground = true;
            this.dataUpatderThread.Start();
        }
        private void dataUpdaterFunction()
        {
            try
            {
                while (true)
                {
                    this.container.Dispatcher.Invoke((Action)(() =>
                    {
                        using (var db = new pb_TunnelVisualizarDatabaseEntities())
                        {
                            this._sensor = db.sensors.Find(this._sensor.iddata);
                            if (this._sensor.data_waterFlow.Count() > 0)
                            {
                                this._dataWaterFlow = this._sensor.data_waterFlow.Last();
                                this.flow_rate_label.Content = this._dataWaterFlow.data == null
                                    ? "null"
                                    : this._dataWaterFlow.data.ToString();
                                try
                                {
                                    if (Convert.ToDouble(this._dataWaterFlow.data) <=Convert.ToDouble(this.flow_rate_label_meter.Text))
                                    {
                                            this.Card.Background = Brushes.Red;
                                            this.flag_1 = false;
                                        
                                    }
                                    else
                                    {
                                        flag_1 = true;
                                        if (flag_1&&flag_2)
                                        {
                                            this.Card.Background = Brushes.WhiteSmoke;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                }
                                
                            }
                            if (this._sensor.data_waterLavel.Count() > 0)
                            {
                                this._dataWaterLavel = this._sensor.data_waterLavel.Last();
                                this.water_height_label.Content = this._dataWaterLavel.data == null
                                    ? "null"
                                    : this._dataWaterLavel.data.ToString();
                                try
                                {

                                    if (Convert.ToDouble(this._dataWaterLavel.data) <=Convert.ToDouble(this.water_height_label_meter.Text))
                                    {
                                            this.Card.Background = Brushes.Red;
                                            this.flag_2 = false;
                                        
                                    }
                                    else
                                    {
                                        flag_2 = true;
                                        if (flag_1 && flag_2)
                                        {
                                            this.Card.Background = Brushes.WhiteSmoke;
                                        }

                                    }
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                SystemConsole.setConsoleTxtThreadSafe(e.Message);
            }
            
        }
        public SensorUnit(sensor _sensor, data_waterFlow _dataWaterFlow, data_waterLavel _dataWaterLavel)
        {
            this._sensor = _sensor;
            this._dataWaterFlow = _dataWaterFlow;
            this._dataWaterLavel = _dataWaterLavel;
        }

        private void ComponentUpdater()
        {
            this.sensor_id_label.Content = this._sensor.iddata.ToString();
            this.sensor_description_label.Content = this._sensor.description.ToString();
            this.position_x_label.Content = this._sensor.position_x.ToString();
            this.position_y_label.Content = this._sensor.position_y.ToString();
            this.flow_rate_label.Content = this._dataWaterFlow.data.ToString();
            this.water_height_label.Content = this._dataWaterLavel.data.ToString();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new pb_TunnelVisualizarDatabaseEntities())
            {
                this._sensor = db.sensors.Find(this._sensor.iddata);
                if (this._sensor.data_waterFlow.Count() > 0)
                {
                    this._dataWaterFlow = this._sensor.data_waterFlow.Last();
                    this.flow_rate_label.Content = this._dataWaterFlow.data.ToString();
                }
                if (this._sensor.data_waterLavel.Count() > 0)
                {
                    this._dataWaterLavel = this._sensor.data_waterLavel.Last();
                    this.water_height_label.Content = this._dataWaterLavel.data.ToString();
                }
            }
        }
    }
}
