using System.Windows;

namespace HNPStyles
{
    public class RectangleSlider : Internal.SliderBase
    {
        static RectangleSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleSlider), new FrameworkPropertyMetadata(typeof(RectangleSlider)));
        }
    }
}
