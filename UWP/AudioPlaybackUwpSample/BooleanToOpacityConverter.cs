using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AudioPlaybackUwpSample
{
    public class BooleanToOpacityConverter : IValueConverter
    {
        private double _trueValue = 1;
        private double _falseValue = 0.3;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? _trueValue : _falseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
