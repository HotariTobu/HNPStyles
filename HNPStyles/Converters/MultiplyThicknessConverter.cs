using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HNPStyles
{
    public class MultiplyThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness v && parameter is string sp)
            {
                string[] vs = sp.Split('/');
                double p1, p2, p3;
                switch (vs.Length)
                {
                    case 1:
                        if (double.TryParse(vs[0], out p2))
                        {
                            return v.Mul(p2);
                        }
                        break;
                    case 2:
                        if (double.TryParse(vs[0], out p1) && double.TryParse(vs[1], out p2))
                        {
                            Thickness re = v.Mul(p2);
                            re.Left = re.Left < p1 ? p1 : re.Left;
                            re.Top = re.Top < p1 ? p1 : re.Top;
                            re.Right = re.Right < p1 ? p1 : re.Right;
                            re.Bottom = re.Bottom < p1 ? p1 : re.Bottom;
                            return re;
                        }
                        break;
                    case 3:
                        if (double.TryParse(vs[0], out p1) && double.TryParse(vs[1], out p2) && double.TryParse(vs[2], out p3))
                        {
                            Thickness re = v.Mul(p2);
                            re.Left = re.Left < p1 ? p1 : re.Left > p3 ? p3 : re.Left;
                            re.Top = re.Top < p1 ? p1 : re.Top > p3 ? p3 : re.Top;
                            re.Right = re.Right < p1 ? p1 : re.Right > p3 ? p3 : re.Right;
                            re.Bottom = re.Bottom < p1 ? p1 : re.Bottom > p3 ? p3 : re.Bottom;
                            return re;
                        }
                        break;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness v && parameter is string sp)
            {
                string[] vs = sp.Split('/');
                double p;
                switch (vs.Length)
                {
                    case 1:
                        if (double.TryParse(vs[0], out p))
                        {
                            return v.Div(p);
                        }
                        break;
                    case 2:
                    case 3:
                        if (double.TryParse(vs[1], out p))
                        {
                            return v.Div(p);
                        }
                        break;
                }
            }

            return null;
        }
    }
}
