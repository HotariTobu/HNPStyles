using System.Windows;

namespace HNPStyles
{
    public class RoundRangeSlider : Internal.RangeSliderBase
    {
        static RoundRangeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundRangeSlider), new FrameworkPropertyMetadata(typeof(RoundRangeSlider)));
        }
    }
}
