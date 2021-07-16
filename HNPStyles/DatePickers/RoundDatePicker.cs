using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RoundDatePicker : Internal.DatePickerBase
    {
        static RoundDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundDatePicker), new FrameworkPropertyMetadata(typeof(RoundDatePicker)));
        }
    }
}
