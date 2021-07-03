using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RoundComboBox : Internal.ComboBoxBase
    {
        static RoundComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundComboBox), new FrameworkPropertyMetadata(typeof(RoundComboBox)));
        }

        #region == InternalMargin ==

        private void UpdateInternalMargin()
        {
            double width = RenderSize.Width;
            double height = RenderSize.Height;
            double leftAndRight = width < height ? width / 2 : height / 2;
            InternalMargin = new Thickness()
            {
                Left = leftAndRight,
                Right = leftAndRight,
            };
        }

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateInternalMargin();
        }
    }
}
