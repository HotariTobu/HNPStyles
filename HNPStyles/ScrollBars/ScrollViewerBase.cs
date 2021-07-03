using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HNPStyles.Internal
{
    public class ScrollViewerBase : ContentControl
    {
        static ScrollViewerBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollViewerBase), new FrameworkPropertyMetadata(typeof(ScrollViewerBase)));
        }

        #region == X ==

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnXChanged(),
          (d, v) => ((ScrollViewerBase)d).CoerceX((double)v)));
        public double X { get => (double)GetValue(XProperty); set => SetValue(XProperty, value); }

        public static readonly RoutedEvent XChangedEvent = EventManager.RegisterRoutedEvent("XChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler XChanged { add => AddHandler(XChangedEvent, value); remove => RemoveHandler(XChangedEvent, value); }
        private void RaiseXChangedEvent() => RaiseEvent(new RoutedEventArgs(XChangedEvent));

        protected virtual void OnXChanged()
        {
            RaiseXChangedEvent();
        }

        private double CoerceX(double value)
        {
            return IsHorizontallyScrollable ? value : 0;
        }

        #endregion
        #region == Y ==

        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnYChanged(),
          (d, v) => ((ScrollViewerBase)d).CoerceY((double)v)));
        public double Y { get => (double)GetValue(YProperty); set => SetValue(YProperty, value); }

        public static readonly RoutedEvent YChangedEvent = EventManager.RegisterRoutedEvent("YChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler YChanged { add => AddHandler(YChangedEvent, value); remove => RemoveHandler(YChangedEvent, value); }
        private void RaiseYChangedEvent() => RaiseEvent(new RoutedEventArgs(YChangedEvent));

        protected virtual void OnYChanged()
        {
            RaiseYChangedEvent();
        }

        private double CoerceY(double value)
        {
            return IsVerticallyScrollable ? value : 0;
        }

        #endregion

        #region == IsHorizontalScrollBarPaused ==

        private static readonly DependencyPropertyKey IsHorizontalScrollBarPausedPropertyKey = DependencyProperty.RegisterReadOnly("IsHorizontalScrollBarPaused", typeof(bool), typeof(ScrollViewerBase), new PropertyMetadata(false));
        public static readonly DependencyProperty IsHorizontalScrollBarPausedProperty = IsHorizontalScrollBarPausedPropertyKey.DependencyProperty;
        public bool IsHorizontalScrollBarPaused { get => (bool)GetValue(IsHorizontalScrollBarPausedProperty); private set => SetValue(IsHorizontalScrollBarPausedPropertyKey, value); }

        #endregion
        #region == IsVerticalScrollBarPaused ==

        private static readonly DependencyPropertyKey IsVerticalScrollBarPausedPropertyKey = DependencyProperty.RegisterReadOnly("IsVerticalScrollBarPaused", typeof(bool), typeof(ScrollViewerBase), new PropertyMetadata(false));
        public static readonly DependencyProperty IsVerticalScrollBarPausedProperty = IsVerticalScrollBarPausedPropertyKey.DependencyProperty;
        public bool IsVerticalScrollBarPaused { get => (bool)GetValue(IsVerticalScrollBarPausedProperty); private set => SetValue(IsVerticalScrollBarPausedPropertyKey, value); }

        #endregion

        #region == Scale ==

        private static readonly DependencyPropertyKey ScalePropertyKey = DependencyProperty.RegisterReadOnly("Scale", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(1d,
          (d, e) => ((ScrollViewerBase)d).OnScaleChanged(),
          (d, v) => ((ScrollViewerBase)d).CoerceScale((double)v)));
        public static readonly DependencyProperty ScaleProperty = ScalePropertyKey.DependencyProperty;
        public double Scale { get => (double)GetValue(ScaleProperty); private set => SetValue(ScalePropertyKey, value); }

        private double LastScale = 1;

        public static readonly RoutedEvent ScaleChangedEvent = EventManager.RegisterRoutedEvent("ScaleChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler ScaleChanged { add => AddHandler(ScaleChangedEvent, value); remove => RemoveHandler(ScaleChangedEvent, value); }
        private void RaiseScaleChangedEvent() => RaiseEvent(new RoutedEventArgs(ScaleChangedEvent));

        protected virtual void OnScaleChanged()
        {
            UpdateContentWidth();
            UpdateContentHeight();
            RaiseScaleChangedEvent();
            LastScale = Scale;
        }

        private double CoerceScale(double value)
        {
            value = value < 0 ? 0 : value;
            X = CenterX - value / LastScale * (CenterX - X);
            Y = CenterY - value / LastScale * (CenterY - Y);
            return value;
        }

        #endregion
        #region == CenterX ==

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnCenterXChanged()));
        public double CenterX { get => (double)GetValue(CenterXProperty); set => SetValue(CenterXProperty, value); }

        public static readonly RoutedEvent CenterXChangedEvent = EventManager.RegisterRoutedEvent("CenterXChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler CenterXChanged { add => AddHandler(CenterXChangedEvent, value); remove => RemoveHandler(CenterXChangedEvent, value); }
        private void RaiseCenterXChangedEvent() => RaiseEvent(new RoutedEventArgs(CenterXChangedEvent));

        protected virtual void OnCenterXChanged()
        {
            RaiseCenterXChangedEvent();
        }

        #endregion
        #region == CenterY ==

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnCenterYChanged()));
        public double CenterY { get => (double)GetValue(CenterYProperty); set => SetValue(CenterYProperty, value); }

        public static readonly RoutedEvent CenterYChangedEvent = EventManager.RegisterRoutedEvent("CenterYChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler CenterYChanged { add => AddHandler(CenterYChangedEvent, value); remove => RemoveHandler(CenterYChangedEvent, value); }
        private void RaiseCenterYChangedEvent() => RaiseEvent(new RoutedEventArgs(CenterYChangedEvent));

        protected virtual void OnCenterYChanged()
        {
            RaiseCenterYChangedEvent();
        }

        #endregion

        #region == HorizontalScrollBarVisibility ==

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewerBase), new PropertyMetadata(ScrollBarVisibility.Disabled,
          (d, e) => ((ScrollViewerBase)d).OnHorizontalScrollBarVisibilityChanged()));
        public ScrollBarVisibility HorizontalScrollBarVisibility { get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); set => SetValue(HorizontalScrollBarVisibilityProperty, value); }

        public static readonly RoutedEvent HorizontalScrollBarVisibilityChangedEvent = EventManager.RegisterRoutedEvent("HorizontalScrollBarVisibilityChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler HorizontalScrollBarVisibilityChanged { add => AddHandler(HorizontalScrollBarVisibilityChangedEvent, value); remove => RemoveHandler(HorizontalScrollBarVisibilityChangedEvent, value); }
        private void RaiseHorizontalScrollBarVisibilityChangedEvent() => RaiseEvent(new RoutedEventArgs(HorizontalScrollBarVisibilityChangedEvent));

        protected virtual void OnHorizontalScrollBarVisibilityChanged()
        {
            UpdateComputedHorizontalScrollBarVisibility();
            RaiseHorizontalScrollBarVisibilityChangedEvent();
        }

        #endregion
        #region == VerticalScrollBarVisibility ==

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewerBase), new PropertyMetadata(ScrollBarVisibility.Disabled,
          (d, e) => ((ScrollViewerBase)d).OnVerticalScrollBarVisibilityChanged()));
        public ScrollBarVisibility VerticalScrollBarVisibility { get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); set => SetValue(VerticalScrollBarVisibilityProperty, value); }

        public static readonly RoutedEvent VerticalScrollBarVisibilityChangedEvent = EventManager.RegisterRoutedEvent("VerticalScrollBarVisibilityChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler VerticalScrollBarVisibilityChanged { add => AddHandler(VerticalScrollBarVisibilityChangedEvent, value); remove => RemoveHandler(VerticalScrollBarVisibilityChangedEvent, value); }
        private void RaiseVerticalScrollBarVisibilityChangedEvent() => RaiseEvent(new RoutedEventArgs(VerticalScrollBarVisibilityChangedEvent));

        protected virtual void OnVerticalScrollBarVisibilityChanged()
        {
            UpdateComputedVerticalScrollBarVisibility();
            RaiseVerticalScrollBarVisibilityChangedEvent();
        }

        #endregion

        #region == ComputedHorizontalScrollBarVisibility ==

        private static readonly DependencyPropertyKey ComputedHorizontalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedHorizontalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewerBase), new PropertyMetadata(Visibility.Visible,
          (d, e) => ((ScrollViewerBase)d).OnComputedHorizontalScrollBarVisibilityChanged()));
        public static readonly DependencyProperty ComputedHorizontalScrollBarVisibilityProperty = ComputedHorizontalScrollBarVisibilityPropertyKey.DependencyProperty;
        public Visibility ComputedHorizontalScrollBarVisibility { get => (Visibility)GetValue(ComputedHorizontalScrollBarVisibilityProperty); private set => SetValue(ComputedHorizontalScrollBarVisibilityPropertyKey, value); }

        private bool IsHorizontallyScrollable;

        public static readonly RoutedEvent ComputedHorizontalScrollBarVisibilityChangedEvent = EventManager.RegisterRoutedEvent("ComputedHorizontalScrollBarVisibilityChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler ComputedHorizontalScrollBarVisibilityChanged { add => AddHandler(ComputedHorizontalScrollBarVisibilityChangedEvent, value); remove => RemoveHandler(ComputedHorizontalScrollBarVisibilityChangedEvent, value); }
        private void RaiseComputedHorizontalScrollBarVisibilityChangedEvent() => RaiseEvent(new RoutedEventArgs(ComputedHorizontalScrollBarVisibilityChangedEvent));

        protected virtual void OnComputedHorizontalScrollBarVisibilityChanged()
        {
            IsChangingSuitableHeight = true;
            RaiseComputedHorizontalScrollBarVisibilityChangedEvent();
        }

        private void UpdateComputedHorizontalScrollBarVisibility()
        {
            if (IsChangingSuitableWidth)
            {
                return;
            }
            IsHorizontallyScrollable = ContentWidth / SuitableWidth > 1;
            ComputedHorizontalScrollBarVisibility = HorizontalScrollBarVisibility switch
            {
                ScrollBarVisibility.Disabled => Visibility.Collapsed,
                ScrollBarVisibility.Hidden => Visibility.Hidden,
                ScrollBarVisibility.Visible => Visibility.Visible,
                _ => IsHorizontallyScrollable ? Visibility.Visible : Visibility.Collapsed,
            };
        }

        #endregion
        #region == ComputedVerticalScrollBarVisibility ==

        private static readonly DependencyPropertyKey ComputedVerticalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedVerticalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewerBase), new PropertyMetadata(Visibility.Visible,
          (d, e) => ((ScrollViewerBase)d).OnComputedVerticalScrollBarVisibilityChanged()));
        public static readonly DependencyProperty ComputedVerticalScrollBarVisibilityProperty = ComputedVerticalScrollBarVisibilityPropertyKey.DependencyProperty;
        public Visibility ComputedVerticalScrollBarVisibility { get => (Visibility)GetValue(ComputedVerticalScrollBarVisibilityProperty); private set => SetValue(ComputedVerticalScrollBarVisibilityPropertyKey, value); }

        private bool IsVerticallyScrollable;

        public static readonly RoutedEvent ComputedVerticalScrollBarVisibilityChangedEvent = EventManager.RegisterRoutedEvent("ComputedVerticalScrollBarVisibilityChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler ComputedVerticalScrollBarVisibilityChanged { add => AddHandler(ComputedVerticalScrollBarVisibilityChangedEvent, value); remove => RemoveHandler(ComputedVerticalScrollBarVisibilityChangedEvent, value); }
        private void RaiseComputedVerticalScrollBarVisibilityChangedEvent() => RaiseEvent(new RoutedEventArgs(ComputedVerticalScrollBarVisibilityChangedEvent));

        protected virtual void OnComputedVerticalScrollBarVisibilityChanged()
        {
            IsChangingSuitableWidth = true;
            RaiseComputedVerticalScrollBarVisibilityChangedEvent();
        }

        private void UpdateComputedVerticalScrollBarVisibility()
        {
            if (IsChangingSuitableHeight)
            {
                return;
            }
            IsVerticallyScrollable = ContentHeight / SuitableHeight > 1;
            ComputedVerticalScrollBarVisibility = VerticalScrollBarVisibility switch
            {
                ScrollBarVisibility.Disabled => Visibility.Collapsed,
                ScrollBarVisibility.Hidden => Visibility.Hidden,
                ScrollBarVisibility.Visible => Visibility.Visible,
                _ => IsVerticallyScrollable ? Visibility.Visible : Visibility.Collapsed,
            };
        }

        #endregion

        #region == SuitableWidth ==

        private static readonly DependencyPropertyKey SuitableWidthPropertyKey = DependencyProperty.RegisterReadOnly("SuitableWidth", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnSuitableWidthChanged()));
        public static readonly DependencyProperty SuitableWidthProperty = SuitableWidthPropertyKey.DependencyProperty;
        public double SuitableWidth { get => (double)GetValue(SuitableWidthProperty); private set => SetValue(SuitableWidthPropertyKey, value); }

        public static readonly RoutedEvent SuitableWidthChangedEvent = EventManager.RegisterRoutedEvent("SuitableWidthChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler SuitableWidthChanged { add => AddHandler(SuitableWidthChangedEvent, value); remove => RemoveHandler(SuitableWidthChangedEvent, value); }
        private void RaiseSuitableWidthChangedEvent() => RaiseEvent(new RoutedEventArgs(SuitableWidthChangedEvent));

        protected virtual void OnSuitableWidthChanged()
        {
            UpdateComputedHorizontalScrollBarVisibility();
            RaiseSuitableWidthChangedEvent();
        }

        private bool IsChangingSuitableWidth;

        private void UpdateSuitableWidth()
        {
            SuitableWidth = Canvas.ActualWidth;
            IsChangingSuitableWidth = false;
        }

        #endregion
        #region == SuitableHeight ==

        private static readonly DependencyPropertyKey SuitableHeightPropertyKey = DependencyProperty.RegisterReadOnly("SuitableHeight", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnSuitableHeightChanged()));
        public static readonly DependencyProperty SuitableHeightProperty = SuitableHeightPropertyKey.DependencyProperty;
        public double SuitableHeight { get => (double)GetValue(SuitableHeightProperty); private set => SetValue(SuitableHeightPropertyKey, value); }

        public static readonly RoutedEvent SuitableHeightChangedEvent = EventManager.RegisterRoutedEvent("SuitableHeightChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler SuitableHeightChanged { add => AddHandler(SuitableHeightChangedEvent, value); remove => RemoveHandler(SuitableHeightChangedEvent, value); }
        private void RaiseSuitableHeightChangedEvent() => RaiseEvent(new RoutedEventArgs(SuitableHeightChangedEvent));

        protected virtual void OnSuitableHeightChanged()
        {
            UpdateComputedVerticalScrollBarVisibility();
            RaiseSuitableHeightChangedEvent();
        }

        private bool IsChangingSuitableHeight;

        private void UpdateSuitableHeight()
        {
            SuitableHeight = Canvas.ActualHeight;
            IsChangingSuitableHeight = false;
        }

        #endregion

        #region == ContentWidth ==

        private static readonly DependencyPropertyKey ContentWidthPropertyKey = DependencyProperty.RegisterReadOnly("ContentWidth", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnContentWidthChanged()));
        public static readonly DependencyProperty ContentWidthProperty = ContentWidthPropertyKey.DependencyProperty;
        public double ContentWidth { get => (double)GetValue(ContentWidthProperty); private set => SetValue(ContentWidthPropertyKey, value); }

        public static readonly RoutedEvent ContentWidthChangedEvent = EventManager.RegisterRoutedEvent("ContentWidthChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler ContentWidthChanged { add => AddHandler(ContentWidthChangedEvent, value); remove => RemoveHandler(ContentWidthChangedEvent, value); }
        private void RaiseContentWidthChangedEvent() => RaiseEvent(new RoutedEventArgs(ContentWidthChangedEvent));

        protected virtual void OnContentWidthChanged()
        {
            UpdateComputedHorizontalScrollBarVisibility();
            RaiseContentWidthChangedEvent();
        }

        private void UpdateContentWidth()
        {
            ContentWidth = ContentHost.ActualWidth * Scale;
        }

        #endregion
        #region == ContentHeight ==

        private static readonly DependencyPropertyKey ContentHeightPropertyKey = DependencyProperty.RegisterReadOnly("ContentHeight", typeof(double), typeof(ScrollViewerBase), new PropertyMetadata(0d,
          (d, e) => ((ScrollViewerBase)d).OnContentHeightChanged()));
        public static readonly DependencyProperty ContentHeightProperty = ContentHeightPropertyKey.DependencyProperty;
        public double ContentHeight { get => (double)GetValue(ContentHeightProperty); private set => SetValue(ContentHeightPropertyKey, value); }

        public static readonly RoutedEvent ContentHeightChangedEvent = EventManager.RegisterRoutedEvent("ContentHeightChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler ContentHeightChanged { add => AddHandler(ContentHeightChangedEvent, value); remove => RemoveHandler(ContentHeightChangedEvent, value); }
        private void RaiseContentHeightChangedEvent() => RaiseEvent(new RoutedEventArgs(ContentHeightChangedEvent));

        protected virtual void OnContentHeightChanged()
        {
            UpdateComputedVerticalScrollBarVisibility();
            RaiseContentHeightChangedEvent();
        }

        private void UpdateContentHeight()
        {
            ContentHeight = ContentHost.ActualHeight * Scale;
        }

        #endregion

        #region == CanZoom ==

        public static readonly DependencyProperty CanZoomProperty = DependencyProperty.Register("CanZoom", typeof(bool), typeof(ScrollViewerBase), new PropertyMetadata(false,
          (d, e) => ((ScrollViewerBase)d).OnCanZoomChanged()));
        public bool CanZoom { get => (bool)GetValue(CanZoomProperty); set => SetValue(CanZoomProperty, value); }

        public static readonly RoutedEvent CanZoomChangedEvent = EventManager.RegisterRoutedEvent("CanZoomChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler CanZoomChanged { add => AddHandler(CanZoomChangedEvent, value); remove => RemoveHandler(CanZoomChangedEvent, value); }
        private void RaiseCanZoomChangedEvent() => RaiseEvent(new RoutedEventArgs(CanZoomChangedEvent));

        protected virtual void OnCanZoomChanged()
        {
            RaiseCanZoomChangedEvent();
        }

        #endregion

        #region == PanningMode ==

        public static readonly DependencyProperty PanningModeProperty = DependencyProperty.Register("PanningMode", typeof(PanningMode), typeof(ScrollViewerBase), new PropertyMetadata(PanningMode.None,
          (d, e) => ((ScrollViewerBase)d).OnPanningModeChanged()));
        public PanningMode PanningMode { get => (PanningMode)GetValue(PanningModeProperty); set => SetValue(PanningModeProperty, value); }

        public static readonly RoutedEvent PanningModeChangedEvent = EventManager.RegisterRoutedEvent("PanningModeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ScrollViewerBase));
        public event RoutedEventHandler PanningModeChanged { add => AddHandler(PanningModeChangedEvent, value); remove => RemoveHandler(PanningModeChangedEvent, value); }
        private void RaisePanningModeChangedEvent() => RaiseEvent(new RoutedEventArgs(PanningModeChangedEvent));

        protected virtual void OnPanningModeChanged()
        {

            System.Diagnostics.Debug.WriteLine($"ScrollViewer PanningMode hasn't implemented yet!!");
            RaisePanningModeChangedEvent();
        }

        #endregion

        private Canvas Canvas = new();
        private ContentPresenter ContentHost = new();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded += (s, e) =>
            {
                IsChangingSuitableWidth = false;
                IsChangingSuitableHeight = false;
            };

            Canvas = (Canvas)GetTemplateChild("PART_Canvas");
            if (Canvas != null)
            {
                Canvas.SizeChanged += Canvas_SizeChanged;
            }
            ContentHost = (ContentPresenter)GetTemplateChild("PART_ContentHost");
            if (ContentHost != null)
            {
                ContentHost.SizeChanged += ContentHost_SizeChanged;
            }
        }

        #region == SizeChanged ==

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                UpdateSuitableWidth();
            }
            if (e.HeightChanged)
            {
                UpdateSuitableHeight();
            }
        }

        private void ContentHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                UpdateContentWidth();
            }
            if (e.HeightChanged)
            {
                UpdateContentHeight();
            }
        }

        #endregion

        #region == MouseDrag ==

        private bool IsMiddleDragging;
        private Point MiddleDraggingMousePoint;
        private Point MiddleDraggingPoint;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                IsMiddleDragging = true;
                IsHorizontalScrollBarPaused = true;
                IsVerticalScrollBarPaused = true;
                MiddleDraggingMousePoint = e.GetPosition(this);
                MiddleDraggingPoint = new Point(X, Y);
                Mouse.Capture(this);
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsMiddleDragging)
            {
                Point point = e.GetPosition(this);
                X = MiddleDraggingPoint.X + point.X - MiddleDraggingMousePoint.X;
                Y = MiddleDraggingPoint.Y + point.Y - MiddleDraggingMousePoint.Y;
                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.MiddleButton == MouseButtonState.Released)
            {
                IsMiddleDragging = false;
                IsHorizontalScrollBarPaused = false;
                IsVerticalScrollBarPaused = false;
                Mouse.Capture(null);
                e.Handled = true;
            }
        }

        #endregion
        #region == MouseWheel ==

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                if (CanZoom)
                {
                    Point point = e.GetPosition(this);
                    CenterX = point.X;
                    CenterY = point.Y;
                    Scale += e.Delta / 1200d;
                }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                X += e.Delta / 6d;
            }
            else
            {
                Y += e.Delta / 6d;
            }

            e.Handled = true;
        }

        #endregion
    }
}
