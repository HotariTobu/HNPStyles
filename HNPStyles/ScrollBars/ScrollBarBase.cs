using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HNPStyles.Internal
{
    public class ScrollBarBase : BarBase
    {
        static ScrollBarBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollBarBase), new FrameworkPropertyMetadata(typeof(ScrollBarBase)));
        }

        public ScrollBarBase()
        {
            InitializeRewindStoryboard();
            InitializeHoldStoryboard();
        }

        #region == IsHorizontal ==

        protected override void OnIsHorizontalChanged()
        {
            base.OnIsHorizontalChanged();
            UpdateMarginLength();
        }

        #endregion

        #region == MarginLength ==

        private double MarginLength;
        private void UpdateMarginLength() => MarginLength = IsHorizontal ? Margin.Left + Margin.Right : Margin.Top + Margin.Bottom;

        #endregion

        #region == GripLength ==

        public static readonly DependencyProperty GripLengthProperty = DependencyProperty.Register("GripLength", typeof(double), typeof(ScrollBarBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollBarBase)d).OnGripLengthChanged(),
          (d, v) => ((ScrollBarBase)d).CoerceGripLength((double)v)));
        public double GripLength { get => (double)GetValue(GripLengthProperty); set => SetValue(GripLengthProperty, value); }

        private double DesiredGripLength;

        public static readonly RoutedEvent GripLengthChangedEvent = EventManager.RegisterRoutedEvent("GripLengthChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollBarBase));
        public event RoutedEventHandler GripLengthChanged { add => AddHandler(GripLengthChangedEvent, value); remove => RemoveHandler(GripLengthChangedEvent, value); }
        private void RaiseGripLengthChangedEvent() => RaiseEvent(new RoutedEventArgs(GripLengthChangedEvent));

        protected virtual void OnGripLengthChanged()
        {
            RaiseGripLengthChangedEvent();
        }

        private double CoerceGripLength(double value)
        {
            if (value < MinLength)
            {
                value = MinLength;
            }
            else if (value > Length)
            {
                value = Length;
            }
            return value;
        }

        private void UpdateGripLength()
        {
            double targetLength = TargetLength;
            if (targetLength == 0)
            {
                return;
            }
            double gripLength = Length * (Length + MarginLength) / targetLength;
            double gripPosition = InternalGripPosition;
            DesiredGripLength = CoerceGripLength(gripLength);
            if (gripPosition < 0)
            {
                gripLength += gripPosition;
            }
            else if (gripPosition > Length - gripLength)
            {
                gripLength = Length - gripPosition;
            }
            GripLength = gripLength;
        }

        #endregion
        #region == GripPosition ==

        private static readonly DependencyPropertyKey GripPositionPropertyKey = DependencyProperty.RegisterReadOnly("GripPosition", typeof(double), typeof(ScrollBarBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollBarBase)d).OnGripPositionChanged(),
          (d, v) => ((ScrollBarBase)d).CoerceGripPosition((double)v)));
        public static readonly DependencyProperty GripPositionProperty = GripPositionPropertyKey.DependencyProperty;
        public double GripPosition { get => (double)GetValue(GripPositionProperty); private set => SetValue(GripPositionPropertyKey, value); }

        private double DesiredGripPosition;

        public static readonly RoutedEvent GripPositionChangedEvent = EventManager.RegisterRoutedEvent("GripPositionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollBarBase));
        public event RoutedEventHandler GripPositionChanged { add => AddHandler(GripPositionChangedEvent, value); remove => RemoveHandler(GripPositionChangedEvent, value); }
        private void RaiseGripPositionChangedEvent() => RaiseEvent(new RoutedEventArgs(GripPositionChangedEvent));

        protected virtual void OnGripPositionChanged()
        {
            RaiseGripPositionChangedEvent();
        }

        private double CoerceGripPosition(double value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else
            {
                double gripRange = Length - GripLength;
                if (value > gripRange)
                {
                    value = gripRange;
                }
            }
            return value;
        }

        private void UpdateGripPosition()
        {
            GripPosition = InternalGripPosition;
        }

        #endregion
        #region == InternalGripPosition ==

        public static readonly DependencyProperty InternalGripPositionProperty = DependencyProperty.Register("InternalGripPosition", typeof(double), typeof(ScrollBarBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollBarBase)d).OnInternalGripPositionChanged()));
        public double InternalGripPosition { get => (double)GetValue(InternalGripPositionProperty); set => SetValue(InternalGripPositionProperty, value); }

        public static readonly RoutedEvent InternalGripPositionChangedEvent = EventManager.RegisterRoutedEvent("InternalGripPositionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollBarBase));
        public event RoutedEventHandler InternalGripPositionChanged { add => AddHandler(InternalGripPositionChangedEvent, value); remove => RemoveHandler(InternalGripPositionChangedEvent, value); }
        private void RaiseInternalGripPositionChangedEvent() => RaiseEvent(new RoutedEventArgs(InternalGripPositionChangedEvent));

        protected virtual void OnInternalGripPositionChanged()
        {
            UpdateGripLength();
            UpdateGripPosition();
            UpdateTargetPosition();
            RaiseInternalGripPositionChangedEvent();
        }

        #endregion
        #region == PositionSource ==

        private double PositionSource
        {
            get => InternalGripPosition;
            set
            {
                if (value < 0)
                {
                    BeginRewind(value, 0);
                    return;
                }
                else
                {
                    double gripRange = Length - DesiredGripLength;
                    if (value > gripRange)
                    {
                        BeginRewind(value, gripRange);
                        return;
                    }
                }

                StopRewind();
                InternalGripPosition = value;

                if (value == 0)
                {
                    TargetPosition = 0;
                }
            }
        }

        private void UpdatePositionSource()
        {
            double gripPosition = -TargetPosition;
            double targetRange = TargetLength - Length - MarginLength;
            if (targetRange == 0)
            {
                gripPosition = 0;
            }
            else
            {
                gripPosition *= (Length - DesiredGripLength) / targetRange;
            }
            PositionSource = gripPosition;
        }

        private void BeginRewind(double from, double to)
        {
            StopRewind();
            DesiredGripPosition = to;
            RewindDoubleAnimation.From = from;
            RewindDoubleAnimation.To = to;
            RewindDoubleAnimation.Duration = new Duration(new System.TimeSpan(0, 0, 0, 0, (int)System.Math.Abs(to - from) * 4));
            RewindStoryboard.Completed += RewindStoryboard_Completed;
            RewindStoryboard.Begin(this, true);
            if (InternalIsRewindingPaused)
            {
                RewindStoryboard.Pause(this);
            }
        }

        private void StopRewind()
        {
            double gripLength = GripLength;
            double gripPosition = InternalGripPosition;
            RewindStoryboard.Stop(this);
            RewindStoryboard.Remove(this);
            RewindStoryboard.Completed -= RewindStoryboard_Completed;
            GripLength = gripLength;
            InternalGripPosition = gripPosition;
        }

        private void RewindStoryboard_Completed(object sender, System.EventArgs e)
        {
            StopRewind();
            GripLength = DesiredGripLength;
            InternalGripPosition = DesiredGripPosition;
        }

        private readonly Storyboard RewindStoryboard = new();
        private readonly DoubleAnimation RewindDoubleAnimation = new();

        private void InitializeRewindStoryboard()
        {
            RewindStoryboard.Children.Add(RewindDoubleAnimation);
            Storyboard.SetTargetProperty(RewindDoubleAnimation, new PropertyPath("InternalGripPosition"));
            RewindDoubleAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
        }

        #endregion

        #region == IsRewindingPaused ==

        public static readonly DependencyProperty IsRewindingPausedProperty = DependencyProperty.Register("IsRewindingPaused", typeof(bool), typeof(ScrollBarBase), new PropertyMetadata(false,
          (d, e) => ((ScrollBarBase)d).OnIsRewindingPausedChanged()));
        public bool IsRewindingPaused { get => (bool)GetValue(IsRewindingPausedProperty); set => SetValue(IsRewindingPausedProperty, value); }

        public static readonly RoutedEvent IsRewindingPausedChangedEvent = EventManager.RegisterRoutedEvent("IsRewindingPausedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollBarBase));
        public event RoutedEventHandler IsRewindingPausedChanged { add => AddHandler(IsRewindingPausedChangedEvent, value); remove => RemoveHandler(IsRewindingPausedChangedEvent, value); }
        private void RaiseIsRewindingPausedChangedEvent() => RaiseEvent(new RoutedEventArgs(IsRewindingPausedChangedEvent));

        protected virtual void OnIsRewindingPausedChanged()
        {
            InternalIsRewindingPaused = IsRewindingPaused;
            RaiseIsRewindingPausedChangedEvent();
        }

        private bool _InternalIsRewindingPaused;
        private bool InternalIsRewindingPaused
        {
            get => _InternalIsRewindingPaused;
            set
            {
                if (value)
                {
                    RewindStoryboard.Pause(this);
                }
                else
                {
                    RewindStoryboard.Resume(this);
                }
                _InternalIsRewindingPaused = value;
            }
        }

        #endregion

        #region == TargetLength ==

        public static readonly DependencyProperty TargetLengthProperty = DependencyProperty.Register("TargetLength", typeof(double), typeof(ScrollBarBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollBarBase)d).OnTargetLengthChanged()));
        public double TargetLength { get => (double)GetValue(TargetLengthProperty); set => SetValue(TargetLengthProperty, value); }

        public static readonly RoutedEvent TargetLengthChangedEvent = EventManager.RegisterRoutedEvent("TargetLengthChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollBarBase));
        public event RoutedEventHandler TargetLengthChanged { add => AddHandler(TargetLengthChangedEvent, value); remove => RemoveHandler(TargetLengthChangedEvent, value); }
        private void RaiseTargetLengthChangedEvent() => RaiseEvent(new RoutedEventArgs(TargetLengthChangedEvent));

        protected virtual void OnTargetLengthChanged()
        {
            UpdateGripLength();
            UpdatePositionSource();
            UpdateTargetGripRatio();
            RaiseTargetLengthChangedEvent();
        }

        #endregion
        #region == TargetPosition ==

        public static readonly DependencyProperty TargetPositionProperty = DependencyProperty.Register("TargetPosition", typeof(double), typeof(ScrollBarBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollBarBase)d).OnTargetPositionChanged()));
        public double TargetPosition { get => (double)GetValue(TargetPositionProperty); set => SetValue(TargetPositionProperty, value); }

        public static readonly RoutedEvent TargetPositionChangedEvent = EventManager.RegisterRoutedEvent("TargetPositionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollBarBase));
        public event RoutedEventHandler TargetPositionChanged { add => AddHandler(TargetPositionChangedEvent, value); remove => RemoveHandler(TargetPositionChangedEvent, value); }
        private void RaiseTargetPositionChangedEvent() => RaiseEvent(new RoutedEventArgs(TargetPositionChangedEvent));

        private bool IsExternalTargetPositionChange = true;

        protected virtual void OnTargetPositionChanged()
        {
            if (IsExternalTargetPositionChange)
            {
                UpdatePositionSource();
            }
            else
            {
                IsExternalTargetPositionChange = true;
            }
            RaiseTargetPositionChangedEvent();
        }

        private void UpdateTargetPosition()
        {
            double targetPosition = InternalGripPosition;
            double gripRange = Length - DesiredGripLength;
            if (gripRange == 0)
            {
                targetPosition = 0;
            }
            else
            {
                targetPosition *= (TargetLength - Length - MarginLength) / gripRange;
            }
            IsExternalTargetPositionChange = false;
            TargetPosition = -targetPosition;
        }

        #endregion

        #region == TargetGripRatio ==

        public double TargetGripRatio { get; private set; }

        private void UpdateTargetGripRatio()
        {
            double length = Length + MarginLength;
            if (length != 0)
            {
                TargetGripRatio = TargetLength / length;
            }
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
            if (GetTemplateChild("PART_Grip") is UIElement g)
            {
                g.MouseDown += Grip_MouseDown;
                g.MouseMove += Grip_MouseMove;
                g.MouseUp += Grip_MouseUp;
                g.MouseUp += Base_MouseUp;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateGripLength();
            UpdatePositionSource();
            UpdateTargetGripRatio();
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
            double from = InternalGripPosition;
            double to = GetMousePosition() - GripLength / 2;
            HoldDoubleAnimation.From = from;
            HoldDoubleAnimation.To = to;
            HoldDoubleAnimation.Duration = new Duration(new System.TimeSpan(0, 0, 0, 0, (int)(System.Math.Abs(to - from) * TargetGripRatio)));
            HoldStoryboard.Begin(this, true);
        }

        private void StopHold()
        {
            double gripPosition = InternalGripPosition;
            HoldStoryboard.Stop(this);
            HoldStoryboard.Remove(this);
            PositionSource = gripPosition;
        }

        private readonly Storyboard HoldStoryboard = new();
        private readonly DoubleAnimation HoldDoubleAnimation = new();

        private void InitializeHoldStoryboard()
        {
            HoldStoryboard.Children.Add(HoldDoubleAnimation);
            Storyboard.SetTargetProperty(HoldDoubleAnimation, new PropertyPath("InternalGripPosition"));
            HoldDoubleAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn };
        }

        #endregion
        #region == Drag ==

        private bool IsDragging;
        private double DraggingGripPosition;
        private double DraggingMousePosition;

        private void Grip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TargetGripRatio < 1)
            {
                return;
            }
            IsDragging = true;
            InternalIsRewindingPaused = true;
            DraggingGripPosition = PositionSource;
            DraggingMousePosition = GetMousePosition();
            Mouse.Capture((UIElement)sender);
        }

        private void Grip_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                PositionSource = DraggingGripPosition + GetMousePosition() - DraggingMousePosition;
            }
        }

        private void Grip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
            InternalIsRewindingPaused = false;
            Mouse.Capture(null);
        }

        #endregion
    }
}
