using System;
using System.Globalization;
using System.Windows.Data;

namespace SharedWPF
{
    public class DoubleAddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is string sp)
            {
                string[] vs = sp.Split('/');
                double p1, p2, p3;
                switch (vs.Length)
                {
                    case 1:
                        if (double.TryParse(vs[0], out p2))
                        {
                            return v + p2;
                        }
                        break;
                    case 2:
                        if (double.TryParse(vs[0], out p1) && double.TryParse(vs[1], out p2))
                        {
                            double re = v + p2;
                            return re < p1 ? p1 : re;
                        }
                        break;
                    case 3:
                        if (double.TryParse(vs[0], out p1) && double.TryParse(vs[1], out p2) && double.TryParse(vs[2], out p3))
                        {
                            double re = v + p2;
                            return re < p1 ? p1 : re > p3 ? p3 : re;
                        }
                        break;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is string sp)
            {
                string[] vs = sp.Split('/');
                double p1, p2, p3;
                switch (vs.Length)
                {
                    case 1:
                        if (double.TryParse(vs[0], out p2))
                        {
                            return v - p2;
                        }
                        break;
                    case 2:
                        if (double.TryParse(vs[0], out p1) && double.TryParse(vs[1], out p2))
                        {
                            double re = v - p2;
                            return re < p1 ? p1 : re;
                        }
                        break;
                    case 3:
                        if (double.TryParse(vs[0], out p1) && double.TryParse(vs[1], out p2) && double.TryParse(vs[2], out p3))
                        {
                            double re = v - p2;
                            return re < p1 ? p1 : re > p3 ? p3 : re;
                        }
                        break;
                }
            }

            return null;
        }
    }
}
