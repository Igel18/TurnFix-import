﻿namespace TurnFixImport.framework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

        public class InverseBoolToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo language)
            {
                return (value is bool && !(bool)value) ? Visibility.Visible: Visibility.Hidden;
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
            {
                throw new NotImplementedException();
            }
        }
}
