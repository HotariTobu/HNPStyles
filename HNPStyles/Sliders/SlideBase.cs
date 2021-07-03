using System;
using System.Windows;
using System.Windows.Media;

namespace HNPStyles.Internal
{
    public class SlideBase : BarBase
    {
        static SlideBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlideBase), new FrameworkPropertyMetadata(typeof(SlideBase)));
        }

        #region == VividBrush ==

        public static readonly DependencyProperty VividBrushProperty = DependencyProperty.Register("VividBrush", typeof(Brush), typeof(SlideBase), new PropertyMetadata(Brushes.White));
        public Brush VividBrush { get => (Brush)GetValue(VividBrushProperty); set => SetValue(VividBrushProperty, value); }

        #endregion

        #region == IsHorizontal ==

        protected override void OnIsHorizontalChanged()
        {
            base.OnIsHorizontalChanged();
            UpdateHorizontalVisibility();
            UpdateVerticalVisibility();
        }

        #endregion
        #region == HorizontalVisibility ==

        private static readonly DependencyPropertyKey HorizontalVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("HorizontalVisibility", typeof(Visibility), typeof(SlideBase), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty HorizontalVisibilityProperty = HorizontalVisibilityPropertyKey.DependencyProperty;
        public Visibility HorizontalVisibility { get => (Visibility)GetValue(HorizontalVisibilityProperty); private set => SetValue(HorizontalVisibilityPropertyKey, value); }

        private void UpdateHorizontalVisibility()
        {
            HorizontalVisibility = IsHorizontal ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
        #region == VerticalVisibility ==

        private static readonly DependencyPropertyKey VerticalVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("VerticalVisibility", typeof(Visibility), typeof(SlideBase), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty VerticalVisibilityProperty = VerticalVisibilityPropertyKey.DependencyProperty;
        public Visibility VerticalVisibility { get => (Visibility)GetValue(VerticalVisibilityProperty); private set => SetValue(VerticalVisibilityPropertyKey, value); }

        private void UpdateVerticalVisibility()
        {
            VerticalVisibility = IsHorizontal ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion

        #region == BarLength ==

        private static readonly DependencyPropertyKey BarLengthPropertyKey = DependencyProperty.RegisterReadOnly("BarLength", typeof(double), typeof(SlideBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty BarLengthProperty = BarLengthPropertyKey.DependencyProperty;
        public double BarLength { get => (double)GetValue(BarLengthProperty); protected set => SetValue(BarLengthPropertyKey, value); }

        #endregion
        #region == BarEnd ==

        public static readonly DependencyProperty BarEndProperty = DependencyProperty.Register("BarEnd", typeof(double), typeof(SlideBase), new PropertyMetadata(0d,
          (d, e) => ((SlideBase)d).OnBarEndChanged()));
        public double BarEnd { get => (double)GetValue(BarEndProperty); set => SetValue(BarEndProperty, value); }

        protected virtual void OnBarEndChanged() { }

        #endregion

        #region == Maximum ==

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(SlideBase), new PropertyMetadata(0d,
          (d, e) => ((SlideBase)d).OnMaximumChanged(),
          (d, v) => ((SlideBase)d).CoerceMaximum((double)v)));
        public double Maximum { get => (double)GetValue(MaximumProperty); set => SetValue(MaximumProperty, value); }

        protected virtual void OnMaximumChanged() { }

        private double CoerceMaximum(double value)
        {
            return value < Minimum ? Minimum : value;
        }

        #endregion
        #region == Minimum ==

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(SlideBase), new PropertyMetadata(0d,
          (d, e) => ((SlideBase)d).OnMinimumChanged(),
          (d, v) => ((SlideBase)d).CoerceMinimum((double)v)));
        public double Minimum { get => (double)GetValue(MinimumProperty); set => SetValue(MinimumProperty, value); }

        protected virtual void OnMinimumChanged() { }

        private double CoerceMinimum(double value)
        {
            return value > Maximum ? Maximum : value;
        }

        #endregion
    }
}
