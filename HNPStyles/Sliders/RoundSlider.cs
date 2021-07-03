using System.Windows;

namespace HNPStyles
{
    public class RoundSlider : Internal.SliderBase
    {
        static RoundSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundSlider), new FrameworkPropertyMetadata(typeof(RoundSlider)));
        }
    }
}
