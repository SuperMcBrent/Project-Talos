using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Utilities {
    public class BoolToStringConverter : BoolToValueConverter<String> { }
    public class BoolToColorConverter : BoolToValueConverter<SolidColorBrush> { }
    public class BoolToStaticResourceConverter : BoolToValueConverter<Style> { }
    public class BoolToValueConverter<T> : IValueConverter {
        //http://geekswithblogs.net/codingbloke/archive/2010/05/28/a-generic-boolean-value-converter.aspx
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }
}
