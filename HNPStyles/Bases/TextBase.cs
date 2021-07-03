using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace HNPStyles
{
    public class TextBase : Internal.Base
    {
        static TextBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata(typeof(TextBase)));

            FlowDirectionProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata((d, e) => ((TextBase)d).UpdateGeometry()));
            FontFamilyProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata((d, e) => ((TextBase)d).UpdateGeometry()));
            FontStyleProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata((d, e) => ((TextBase)d).UpdateGeometry()));
            FontWeightProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata((d, e) => ((TextBase)d).UpdateGeometry()));
            FontStretchProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata((d, e) => ((TextBase)d).UpdateGeometry()));
            FontSizeProperty.OverrideMetadata(typeof(TextBase), new FrameworkPropertyMetadata((d, e) => ((TextBase)d).UpdateGeometry()));
        }

        public TextBase()
        {
            UpdatePPD();
            //PresentationSource.AddSourceChangedHandler(this, (s, e) => UpdatePPD());
            
        }

        #region == PPD ==

        private double PPD;

        private void UpdatePPD()
        {
            PPD = PresentationSource.FromVisual(this)?.CompositionTarget.TransformToDevice.M11 ?? 1;
        }

        #endregion
        #region == Text ==

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBase), new PropertyMetadata(string.Empty,
            (d, e) => ((TextBase)d).UpdateGeometry()));
        public string Text { get => GetValue(TextProperty) as string; set => SetValue(TextProperty, value); }

        #endregion

        protected override void OnPrimaryColorChanged()
        {
            base.OnPrimaryColorChanged();
            InnerBrush = Foreground;
        }

        protected override void UpdateGeometry()
        {
            FormattedText formattedText = new(Text, CultureInfo.CurrentCulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Transparent, PPD);
            Size size = formattedText.BuildHighlightGeometry(new Point()).Bounds.Size;
            Width = size.Width;
            Height = size.Height;
            Geometry = formattedText.BuildGeometry(new Point());
        }
    }
}
