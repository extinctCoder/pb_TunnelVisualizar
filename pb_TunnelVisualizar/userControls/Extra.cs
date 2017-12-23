using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pb_TunnelVisualizar.userControls
{
    class Extra
    {
    }

    public static class getRectangle
    {
        public static Rectangle rectangle()
        {
            return new Rectangle()
            {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Purple,
                StrokeThickness = 5
            };
            
        }
        public static Rectangle rectangle (Brush rectangleColor)
        {
            return new Rectangle()
            {
                Width = 10,
                Height = 10,
                Stroke = rectangleColor,
                StrokeThickness = 5
            };

        }
    }
}
