using System;
using System.Windows;
using System.Windows.Controls;

namespace HNPStyles
{
    public class RectangleComboBox : Internal.ComboBoxBase
    {
        static RectangleComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleComboBox), new FrameworkPropertyMetadata(typeof(RectangleComboBox)));
        }

        #region == CornerRadius ==

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RectangleComboBox), new PropertyMetadata(new CornerRadius(),
            (d, e) => ((RectangleComboBox)d).UpdateInternalMargin()));
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        #endregion
        #region == InternalMargin ==

        private void UpdateInternalMargin()
        {
            CornerRadius cornerRadius = CornerRadius;
            InternalMargin = new Thickness()
            {
                Left = Math.Max(cornerRadius.TopLeft, cornerRadius.BottomLeft),
                Right = Math.Max(cornerRadius.TopRight, cornerRadius.BottomRight),
            };
        }

        #endregion
    }
}
