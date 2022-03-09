using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SharedWPF
{
    public class MultiCalculateDoubleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is double result && parameter is string methods && methods.Length == values.Length - 1)
            {
                for (int i = 0; i < methods.Length; i++)
                {
                    if (values[i + 1] is double value)
                    {
#if NET5_0 || NETCOREAPP || NETCOREAPP3_1
                        result = methods[i] switch
                        {
                            '+' => result + value,
                            '-' => result - value,
                            '*' => result * value,
                            '/' => result / value,
                            '_' => value - result,
                            '\\' => value / result,
                            _ => result,
                        };
#else
                        switch (methods[i])
                        {
                            case '+':
                                result += value;
                                break;
                            case '-':
                                result -= value;
                                break;
                            case '*':
                                result *= value;
                                break;
                            case '/':
                                result /= value;
                                break;
                            case '_':
                                result = value - result;
                                break;
                            case '\\':
                                result = value / result;
                                break;
                        }
#endif
                    }
                }
                return result;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
