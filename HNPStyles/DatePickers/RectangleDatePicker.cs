using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RectangleDatePicker : Internal.DatePickerBase
    {
        static RectangleDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleDatePicker), new FrameworkPropertyMetadata(typeof(RectangleDatePicker)));
        }
    }
}
