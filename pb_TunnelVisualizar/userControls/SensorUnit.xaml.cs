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
        public SensorUnit()
        {
            InitializeComponent();
        }

        public SensorUnit(db.sensor _sensor)
        {
            this.InitializeComponent();
            this._sensor = _sensor;
            this.sensor_id_label.Content = this._sensor.iddata.ToString();
            this.sensor_description_label.Content = this._sensor.description.ToString();
            this.position_x_label.Content = this._sensor.position_x.ToString();
            this.position_y_label.Content = this._sensor.position_y.ToString();

            if (this._sensor.data_waterLavel.Count() > 0)
            {
                this._dataWaterFlow = this._sensor.data_waterFlow.Last();
                this.flow_rate_label.Content =
                    this._dataWaterFlow.data == null ? "null" : this._dataWaterFlow.data.ToString();
            }
            if (this._sensor.data_waterFlow.Count() > 0)
            {
                this._dataWaterLavel = this._sensor.data_waterLavel.Last();
                this.water_height_label.Content =
                    this._dataWaterLavel.data == null ? "null" : this._dataWaterLavel.data.ToString();
            }
        }

        private void dataUpdater()
        {
            Thread dataUpatderThread = new Thread(dataUpdaterFunction);
            dataUpatderThread.Name = "dataUpatderThread";
            dataUpatderThread.IsBackground = true;
            dataUpatderThread.Start();
        }
        private void dataUpdaterFunction()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                using (var db = new pb_TunnelVisualizarDatabaseEntities())
                {
                    this._dataWaterFlow = (db.sensors.Find(_sensor.iddata)).data_waterFlow.Last();
                    this._dataWaterLavel = (db.sensors.Find(_sensor.iddata)).data_waterLavel.Last();
                };
                this.flow_rate_label.Content = this._dataWaterFlow.data.ToString();
                this.water_height_label.Content = this._dataWaterLavel.data.ToString();
                Debug.WriteLine(this._dataWaterFlow.data.ToString());
                Debug.WriteLine(this._dataWaterLavel.data.ToString());
            }));
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
            if (this._sensor.data_waterLavel.Count() > 0 && this._sensor.data_waterFlow.Count() > 0)
            {
                using (var db = new pb_TunnelVisualizarDatabaseEntities())
                {
                    this._dataWaterFlow = (db.sensors.Find(_sensor.iddata)).data_waterFlow.Last();
                    this._dataWaterLavel = (db.sensors.Find(_sensor.iddata)).data_waterLavel.Last();
                };
                this.flow_rate_label.Content = this._dataWaterFlow.data.ToString();
                this.water_height_label.Content = this._dataWaterLavel.data.ToString();
                Debug.WriteLine(this._dataWaterFlow.data.ToString());
                Debug.WriteLine(this._dataWaterLavel.data.ToString());
            }
        }
    }
}
