using System;
using System.Windows;

namespace HNPStyles.Internal
{
    public class ProgressBarBase : RangeSlideBase
    {
        static ProgressBarBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBarBase), new FrameworkPropertyMetadata(typeof(ProgressBarBase)));
        }

        #region == BarLength ==

        private void UpdateBarLength()
        {
            BarLength = Math.Abs(BarStart - BarEnd) * Length;
        }

        #endregion
        #region == BarPosition ==

        private void UpdateBarPosition()
        {
            BarPosition = Math.Min(BarStart, BarEnd) * Length;
        }

        #endregion
        #region == BarStart ==

        protected override void OnBarStartChanged()
        {
            UpdateBarLength();
            UpdateBarPosition();
        }

        private void UpdateBarStart()
        {
            BarStart = 0;
        }

        #endregion
        #region == BarEnd ==

        protected override void OnBarEndChanged()
        {
            UpdateBarLength();
            UpdateBarPosition();
        }

        private void UpdateBarEnd()
        {
            double range = Maximum - Minimum;
            if (range == 0)
            {
                BarEnd = 0;
            }
            else
            {
                BarEnd = (Value - Minimum) / range;
            }
        }

        #endregion

        #region == Value ==

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ProgressBarBase), new PropertyMetadata(0d,
          (d, e) => ((ProgressBarBase)d).OnValueChanged(),
          (d, v) => ((ProgressBarBase)d).CoerceValue((double)v)));
        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        protected virtual void OnValueChanged()
        {
            UpdateBarStart();
            UpdateBarEnd();
        }

        private double CoerceValue(double value)
        {
            return value < Minimum ? Minimum : value > Maximum ? Maximum : value;
        }

        #endregion
        #region == Maximum ==

        protected override void OnMaximumChanged()
        {
            UpdateBarStart();
            UpdateBarEnd();
        }

        #endregion
        #region == Minimum ==

        protected override void OnMinimumChanged()
        {
            UpdateBarStart();
            UpdateBarEnd();
        }

        #endregion

        #region == ProgressBarModes ==

        public static readonly DependencyProperty ProgressBarModesProperty = DependencyProperty.Register("ProgressBarModes", typeof(ProgressBarModes), typeof(ProgressBarBase), new PropertyMetadata(ProgressBarModes.Normal));
        public ProgressBarModes ProgressBarModes { get => (ProgressBarModes)GetValue(ProgressBarModesProperty); set => SetValue(ProgressBarModesProperty, value); }

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateBarLength();
            UpdateBarPosition();
        }
    }
}
