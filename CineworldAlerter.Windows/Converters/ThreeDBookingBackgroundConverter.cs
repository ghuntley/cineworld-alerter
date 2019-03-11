using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CineworldAlerter.Converters
{
    public class ThreeDBookingBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var red = (Color)Application.Current.Resources["CineworldRedColor"];
            var blue = (Color) Application.Current.Resources["ThreeDBlueColor"];

            if (!(value is bool is3d) || !is3d)
                return new SolidColorBrush(red);

            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };

            var stop1 = new GradientStop
            {
                Color = red,
                Offset = 0.0
            };

            gradient.GradientStops.Add(stop1);

            var stop2 = new GradientStop
            {
                Color = red,
                Offset = 0.5
            };

            gradient.GradientStops.Add(stop2);

            var stop3 = new GradientStop
            {
                Color = blue,
                Offset = 0.5
            };

            gradient.GradientStops.Add(stop3);

            var stop4 = new GradientStop
            {
                Color = blue,
                Offset = 1.0
            };

            gradient.GradientStops.Add(stop4);

            return gradient;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
