using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HNPStyles.Internal
{
    public class SliderBase : SlideBase
    {
        static SliderBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderBase), new FrameworkPropertyMetadata(typeof(SliderBase)));

            PaddingProperty.OverrideMetadata(typeof(SliderBase), new FrameworkPropertyMetadata((d, e) => ((SliderBase)d).UpdatePaddingLength()));
        }

        public SliderBase()
        {
            InitializeHoldStoryboard();
        }

        #region == PaddingLength ==

        private double PaddingLength;
        private void UpdatePaddingLength()
        {
            PaddingLength = IsHorizontal ? Padding.Left + Padding.Right : Padding.Top + Padding.Bottom;

            UpdateBarLength();
        }

        #endregion

        #region == BarLength ==

        private void UpdateBarLength()
        {
            BarLength = BarEnd * (Length - PaddingLength);
        }

        #endregion
        #region == BarEnd ==

        protected override void OnBarEndChanged()
        {
            UpdateBarLength();
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
                BarEnd = (Value - Minimum) / range;
            }
        }

        #endregion

        #region == GripEnd ==

        private static readonly DependencyPropertyKey GripEndPropertyKey = DependencyProperty.RegisterReadOnly("GripEnd", typeof(double), typeof(SliderBase), new PropertyMetadata(0d));
        public static readonly DependencyProperty GripEndProperty = GripEndPropertyKey.DependencyProperty;
        public double GripEnd { get => (double)GetValue(GripEndProperty); private set => SetValue(GripEndPropertyKey, value); }

        private void UpdateGripEnd()
        {
            GripEnd = BarEnd * (Length - MinLength);
        }

        #endregion

        #region == Value ==

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(SliderBase), new PropertyMetadata(0d,
          (d, e) => ((SliderBase)d).OnValueChanged(),
          (d, v) => ((SliderBase)d).CoerceValue((double)v)));
        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        protected virtual void OnValueChanged()
        {
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
            UpdateBarEnd();
        }

        #endregion
        #region == Minimum ==

        protected override void OnMinimumChanged()
        {
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
            if (GetTemplateChild("PART_HorizontalEndGrip") is UIElement hg)
            {
                hg.MouseDown += Grip_MouseDown;
                hg.MouseMove += Grip_MouseMove;
                hg.MouseUp += Grip_MouseUp;
                hg.MouseUp += Base_MouseUp;
            }
            if (GetTemplateChild("PART_VerticalEndGrip") is UIElement vg)
            {
                vg.MouseDown += Grip_MouseDown;
                vg.MouseMove += Grip_MouseMove;
                vg.MouseUp += Grip_MouseUp;
                vg.MouseUp += Base_MouseUp;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateBarLength();
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
            double from = Value;
            double length = Length - MinLength;
            double to;
            if (length == 0)
            {
                to = from;
            }
            else
            {
                to = (GetMousePosition() - MinLength / 2) / length * (Maximum - Minimum) + Minimum;
            }
            HoldDoubleAnimation.From = from;
            HoldDoubleAnimation.To = to;
            HoldDoubleAnimation.Duration = new Duration(new System.TimeSpan(0, 0, 0, 0, (int)(System.Math.Abs(to - from) * length / 10)));
            HoldStoryboard.Begin(this, true);
        }

        private void StopHold()
        {
            double value = Value;
            HoldStoryboard.Stop(this);
            HoldStoryboard.Remove(this);
            Value = value;
        }

        private readonly Storyboard HoldStoryboard = new();
        private readonly DoubleAnimation HoldDoubleAnimation = new();

        private void InitializeHoldStoryboard()
        {
            HoldStoryboard.Children.Add(HoldDoubleAnimation);
            Storyboard.SetTargetProperty(HoldDoubleAnimation, new PropertyPath("Value"));
            HoldDoubleAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn };
        }

        #endregion
        #region == Drag ==

        private bool IsDragging;
        private double DraggingValue;
        private double DraggingMousePosition;

        private void Grip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
            DraggingValue = Value;
            DraggingMousePosition = GetMousePosition();
            Mouse.Capture((UIElement)sender);
        }

        private void Grip_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                double length = Length - MinLength;
                if (length != 0)
                {
                    Value = DraggingValue + (GetMousePosition() - DraggingMousePosition) / length * (Maximum - Minimum);
                }
            }
        }

        private void Grip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
            Mouse.Capture(null);
        }

        #endregion
    }
}
