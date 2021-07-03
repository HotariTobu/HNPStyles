using System;
using System.Windows;

namespace HNPStyles.Internal
{
    public class RangeSlideBase : SlideBase
    {
        static RangeSlideBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSlideBase), new FrameworkPropertyMetadata(typeof(RangeSlideBase)));
        }

        #region == BarPosition ==

        private static readonly DependencyPropertyKey BarPositionPropertyKey = DependencyProperty.RegisterReadOnly("BarPosition", typeof(double), typeof(RangeSlideBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty BarPositionProperty = BarPositionPropertyKey.DependencyProperty;
        public double BarPosition { get => (double)GetValue(BarPositionProperty); protected set => SetValue(BarPositionPropertyKey, value); }

        #endregion
        #region == BarStart ==

        public static readonly DependencyProperty BarStartProperty = DependencyProperty.Register("BarStart", typeof(double), typeof(RangeSlideBase), new PropertyMetadata(0d,
          (d, e) => ((RangeSlideBase)d).OnBarStartChanged()));
        public double BarStart { get => (double)GetValue(BarStartProperty); set => SetValue(BarStartProperty, value); }

        protected virtual void OnBarStartChanged() { }

        #endregion
    }
}
