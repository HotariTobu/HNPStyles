using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HNPStyles.Internal
{
    public class RangeSliderBase : RangeSlideBase
    {
        static RangeSliderBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSliderBase), new FrameworkPropertyMetadata(typeof(RangeSliderBase)));

            PaddingProperty.OverrideMetadata(typeof(RangeSliderBase), new FrameworkPropertyMetadata((d, e) => ((RangeSliderBase)d).UpdatePaddingLength()));
        }

        public RangeSliderBase()
        {
            InitializeHoldStoryboard();
        }

        #region == GripRadius ==

        private double GripRadius;
        private void UpdateGripRadius() => GripRadius = MinLength / 2;

        #endregion

        #region == PaddingLength ==

        private double PaddingLength;
        private void UpdatePaddingLength()
        {
            PaddingLength = IsHorizontal ? Padding.Left + Padding.Right : Padding.Top + Padding.Bottom;

            UpdateBarLength();
            UpdateBarPosition();
        }

        #endregion

        #region == BarLength ==

        private void UpdateBarLength()
        {
            BarLength = Math.Abs(BarStart - BarEnd) * (Length - MinLength - PaddingLength) + MinLength;
        }

        #endregion
        #region == BarPosition ==

        private void UpdateBarPosition()
        {
            BarPosition = Math.Min(BarStart, BarEnd) * (Length - MinLength - PaddingLength);
        }

        #endregion
        #region == BarStart ==

        protected override void OnBarStartChanged()
        {
            UpdateBarLength();
            UpdateBarPosition();
            UpdateGripStart();
        }

        private void UpdateBarStart()
        {
            double range = Maximum - Minimum;
            if (range == 0)
            {
                BarStart = 0;
            }
            else
            {
                BarStart = (StartValue - Minimum) / range;
            }
        }

        #endregion
        #region == BarEnd ==

        protected override void OnBarEndChanged()
        {
            UpdateBarLength();
            UpdateBarPosition();
            UpdateGripEnd();
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
                BarEnd = (EndValue - Minimum) / range;
            }
        }

        #endregion

        #region == GripStart ==

        private static readonly DependencyPropertyKey GripStartPropertyKey = DependencyProperty.RegisterReadOnly("GripStart", typeof(double), typeof(RangeSliderBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty GripStartProperty = GripStartPropertyKey.DependencyProperty;
        public double GripStart { get => (double)GetValue(GripStartProperty); private set => SetValue(GripStartPropertyKey, value); }

        private void UpdateGripStart()
        {
            GripStart = BarStart * (Length - MinLength * 2);
        }

        #endregion
        #region == GripEnd ==

        private static readonly DependencyPropertyKey GripEndPropertyKey = DependencyProperty.RegisterReadOnly("GripEnd", typeof(double), typeof(RangeSliderBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty GripEndProperty = GripEndPropertyKey.DependencyProperty;
        public double GripEnd { get => (double)GetValue(GripEndProperty); private set => SetValue(GripEndPropertyKey, value); }

        private void UpdateGripEnd()
        {
            GripEnd = BarEnd * (Length - MinLength * 2) + MinLength;
        }

        #endregion

        #region == StartValue ==

        public static readonly DependencyProperty StartValueProperty = DependencyProperty.Register("StartValue", typeof(double), typeof(RangeSliderBase), new PropertyMetadata(0d,
          (d, e) => ((RangeSliderBase)d).OnStartValueChanged(),
          (d, v) => ((RangeSliderBase)d).CoerceStartValue((double)v)));
        public double StartValue { get => (double)GetValue(StartValueProperty); set => SetValue(StartValueProperty, value); }

        protected virtual void OnStartValueChanged()
        {
            UpdateBarStart();
        }

        private double CoerceStartValue(double value)
        {
            return value < Minimum ? Minimum : value > EndValue ? EndValue : value;
        }

        #endregion
        #region == EndValue ==

        public static readonly DependencyProperty EndValueProperty = DependencyProperty.Register("EndValue", typeof(double), typeof(RangeSliderBase), new PropertyMetadata(0d,
          (d, e) => ((RangeSliderBase)d).OnEndValueChanged(),
          (d, v) => ((RangeSliderBase)d).CoerceEndValue((double)v)));
        public double EndValue { get => (double)GetValue(EndValueProperty); set => SetValue(EndValueProperty, value); }

        protected virtual void OnEndValueChanged()
        {
            UpdateBarEnd();
        }

        private double CoerceEndValue(double value)
        {
            return value < StartValue ? StartValue : value > Maximum ? Maximum : value;
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Base") is UIElement b)
            {
                b.MouseDown += Base_MouseDown;
                b.MouseMove += Base_MouseMove;
                b.MouseUp += Base_MouseUp;
                MouseLeave += Base_MouseUp;
            }
            if (GetTemplateChild("PART_HorizontalStartGrip") is UIElement hsg)
            {
                hsg.MouseDown += Grip_MouseDown;
                hsg.MouseMove += Grip_MouseMove;
                hsg.MouseUp += Grip_MouseUp;
                hsg.MouseUp += Base_MouseUp;
            }
            if (GetTemplateChild("PART_HorizontalEndGrip") is UIElement heg)
            {
                heg.MouseDown += Grip_MouseDown;
                heg.MouseMove += Grip_MouseMove;
                heg.MouseUp += Grip_MouseUp;
                heg.MouseUp += Base_MouseUp;
            }
            if (GetTemplateChild("PART_VerticalStartGrip") is UIElement vsg)
            {
                vsg.MouseDown += Grip_MouseDown;
                vsg.MouseMove += Grip_MouseMove;
                vsg.MouseUp += Grip_MouseUp;
                vsg.MouseUp += Base_MouseUp;
            }
            if (GetTemplateChild("PART_VerticalEndGrip") is UIElement veg)
            {
                veg.MouseDown += Grip_MouseDown;
                veg.MouseMove += Grip_MouseMove;
                veg.MouseUp += Grip_MouseUp;
                veg.MouseUp += Base_MouseUp;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateGripRadius();
            UpdateBarLength();
            UpdateBarPosition();
            UpdateGripStart();
            UpdateGripEnd();
        }

        #region == Hold ==

        private bool IsHolding;

        private void Base_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsHolding = true;
            BeginHold();
        }

        private void Base_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsHolding)
            {
                BeginHold();
            }
        }

        private void Base_MouseUp(object sender, MouseEventArgs e)
        {
            IsHolding = false;
            StopHold();
        }

        private void BeginHold()
        {
            StopHold();
            double pos = GetMousePosition();
            bool isStart = GetIsStart(pos);
            double from = isStart ? StartValue : EndValue;
            double length = Length - MinLength;
            double to;
            if (length == 0)
            {
                to = from;
            }
            else
            {
                to = (pos - GripRadius) / length * (Maximum - Minimum) + Minimum;
            }
            Storyboard.SetTargetProperty(HoldDoubleAnimation, new PropertyPath(isStart ? "StartValue" : "EndValue"));
            HoldDoubleAnimation.From = from;
            HoldDoubleAnimation.To = to;
            HoldDoubleAnimation.Duration = new Duration(new System.TimeSpan(0, 0, 0, 0, (int)(System.Math.Abs(to - from) * length / 10)));
            HoldStoryboard.Begin(this, true);
        }

        private void StopHold()
        {
            double startValue = StartValue;
            double endValue = EndValue;
            HoldStoryboard.Stop(this);
            HoldStoryboard.Remove(this);
            StartValue = startValue;
            EndValue = endValue;
        }

        private readonly Storyboard HoldStoryboard = new();
        private readonly DoubleAnimation HoldDoubleAnimation = new();

        private void InitializeHoldStoryboard()
        {
            HoldStoryboard.Children.Add(HoldDoubleAnimation);
            HoldDoubleAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn };
        }

        #endregion
        #region == Drag ==

        private bool IsDragging;
        private bool IsStart;
        private double DraggingValue;
        private double DraggingMousePosition;

        private void Grip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
            double pos = GetMousePosition();
            IsStart = GetIsStart(pos);
            DraggingValue = IsStart ? StartValue : EndValue;
            DraggingMousePosition = pos;
            Mouse.Capture((UIElement)sender);
        }

        private void Grip_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                double length = Length - MinLength;
                if (length != 0)
                {
                    double value = DraggingValue + (GetMousePosition() - DraggingMousePosition) / length * (Maximum - Minimum);
                    if (IsStart)
                    {
                        StartValue = value;
                    }
                    else
                    {
                        EndValue = value;
                    }
                }
            }
        }

        private void Grip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
            Mouse.Capture(null);
        }

        #endregion

        private bool GetIsStart(double pos)
        {
            return Math.Abs(pos - GripStart - GripRadius) < Math.Abs(pos - GripEnd - GripRadius);
        }
    }
}
