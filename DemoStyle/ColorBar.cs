using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoStyle
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Opecap"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Opecap;assembly=Opecap"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:ColorBar/>
    ///
    /// </summary>
    public class ColorBar : Control
    {
        static ColorBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorBar), new FrameworkPropertyMetadata(typeof(ColorBar)));
        }

        #region == MaxValue ==

        public double MaxValue { get => (double)GetValue(MaxValueProperty); set => SetValue(MaxValueProperty, value); }
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(ColorBar), new PropertyMetadata(0d));

        #endregion
        #region == MinValue ==

        public double MinValue { get => (double)GetValue(MinValueProperty); set => SetValue(MinValueProperty, value); }
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(ColorBar), new PropertyMetadata(0d));

        #endregion
        #region == Value ==

        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ColorBar), new PropertyMetadata(0d,
            (d, e) => { }, (d, baseValue) =>
            {
                double result = (double)baseValue;
                if (d is ColorBar colorBar)
                {
                    return result < colorBar.MinValue ? colorBar.MinValue : result > colorBar.MaxValue ? colorBar.MaxValue : result;
                }
                return result;
            }));

        #endregion

        private Border Bar { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Bar = GetTemplateChild("Bar") as Border;
            if (Bar != null)
            {
                Bar.MouseDown += Bar_MouseDown;
                Bar.MouseMove += Bar_MouseMove;
                Bar.MouseUp += Bar_MouseUp;
            }
        }

        private void UpdateValue()
        {
            Value = Mouse.GetPosition(Bar).X / Bar.ActualWidth * (MaxValue - MinValue) + MinValue;
        }

        private void Bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(Bar);
            UpdateValue();
        }

        private void Bar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.MiddleButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                UpdateValue();
            }
        }

        private void Bar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }
    }
}
