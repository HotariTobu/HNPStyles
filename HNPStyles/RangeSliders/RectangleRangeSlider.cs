using System.Windows;

namespace HNPStyles
{
    public class RectangleRangeSlider : Internal.RangeSliderBase
    {
        static RectangleRangeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleRangeSlider), new FrameworkPropertyMetadata(typeof(RectangleRangeSlider)));
        }
    }
}
