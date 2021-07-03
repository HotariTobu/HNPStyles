using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HNPStyles.Internal
{
    public class SwitchBase : Control
    {
        static SwitchBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchBase), new FrameworkPropertyMetadata(typeof(SwitchBase)));

            WidthProperty.OverrideMetadata(typeof(SwitchBase), new FrameworkPropertyMetadata((d, e) => { },
                (d, v) =>
                {
                    double width = (double)v;
                    double height = ((SwitchBase)d).RenderSize.Height;
                    return width < height ? height * 2 : width;
                }));
            HeightProperty.OverrideMetadata(typeof(SwitchBase), new FrameworkPropertyMetadata((d, e) => { },
                (d, v) =>
                {
                    double width = ((SwitchBase)d).RenderSize.Width;
                    double height = (double)v;
                    return width < height ? width / 2 : height;
                }));
        }

        #region == VividBrush ==

        public static readonly DependencyProperty VividBrushProperty = DependencyProperty.Register("VividBrush", typeof(Brush), typeof(SwitchBase), new PropertyMetadata(Brushes.White));
        public Brush VividBrush { get => (Brush)GetValue(VividBrushProperty); set => SetValue(VividBrushProperty, value); }

        #endregion

        #region == IsOn ==

        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register("IsOn", typeof(bool), typeof(SwitchBase), new PropertyMetadata(false));
        public bool IsOn { get => (bool)GetValue(IsOnProperty); set => SetValue(IsOnProperty, value); }

        #endregion

        #region == RelativePosition ==

        public static readonly DependencyProperty RelativePositionProperty = DependencyProperty.Register("RelativePosition", typeof(double), typeof(SwitchBase), new PropertyMetadata(0d,
            (d, e) => ((SwitchBase)d).UpdateAbsolutePosition()));
        public double RelativePosition { get => (double)GetValue(RelativePositionProperty); set => SetValue(RelativePositionProperty, value); }

        #endregion
        #region == AbsolutePosition ==

        private static readonly DependencyPropertyKey AbsolutePositionPropertyKey = DependencyProperty.RegisterReadOnly("AbsolutePosition", typeof(double), typeof(SwitchBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty AbsolutePositionProperty = AbsolutePositionPropertyKey.DependencyProperty;
        public double AbsolutePosition { get => (double)GetValue(AbsolutePositionProperty); private set => SetValue(AbsolutePositionPropertyKey, value); }

        private void UpdateAbsolutePosition()
        {
            AbsolutePosition = (ActualWidth - ActualHeight) * RelativePosition;
            UpdateInnerWidth();
        }

        #endregion
        #region == InnerWidth ==

        private static readonly DependencyPropertyKey InnerWidthPropertyKey = DependencyProperty.RegisterReadOnly("InnerWidth", typeof(double), typeof(SwitchBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty InnerWidthProperty = InnerWidthPropertyKey.DependencyProperty;
        public double InnerWidth { get => (double)GetValue(InnerWidthProperty); private set => SetValue(InnerWidthPropertyKey, value); }

        private void UpdateInnerWidth() => InnerWidth = AbsolutePosition + ActualHeight / 2;

        #endregion

        private bool IsMouseDown;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsMouseDown = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            IsOn ^= IsMouseDown;
            IsMouseDown = false;
        }
    }
}
