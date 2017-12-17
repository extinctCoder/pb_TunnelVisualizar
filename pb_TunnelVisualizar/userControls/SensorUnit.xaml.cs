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
    }
}
