using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZombieGame.UI.Templates
{
    /// <summary>
    /// Interaction logic for GradientProgressBarUI.xaml
    /// </summary>
    public partial class GradientProgressBarUI : UserControl
    {
        [Description(""), Category("Common")]
        public string ProgressText
        {
            get { return (string)GetValue(ProgressTextProperty); }
            set { SetValue(ProgressTextProperty, value); ProgressLabel.Content = value; }
        }
        [Description(""), Category("Common")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); TitleLabel.Content = value; }
        }
        [Description(""), Category("Common")]
        public double FillPercentage
        {
            get { return (double)GetValue(FillPercentageProperty); }
            set { SetValue(FillPercentageProperty, value); UpdateFillBar(); }
        }

        [Description(""), Category("Brush")]
        public Color FillColor1
        {
            get { return (Color)GetValue(FillColor1Property); }
            set { SetValue(FillColor1Property, value); }
        }
        [Description(""), Category("Brush")]
        public Color FillColor2
        {
            get { return (Color)GetValue(FillColor2Property); }
            set { SetValue(FillColor2Property, value); }
        }
        [Description(""), Category("Brush")]
        public Color FillColor3
        {
            get { return (Color)GetValue(FillColor3Property); }
            set { SetValue(FillColor3Property, value); }
        }

        public static readonly DependencyProperty FillColor1Property =
            DependencyProperty.Register("FillColor1", typeof(Color), typeof(GradientProgressBarUI), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));
        public static readonly DependencyProperty FillColor2Property =
            DependencyProperty.Register("FillColor2", typeof(Color), typeof(GradientProgressBarUI), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));
        public static readonly DependencyProperty FillColor3Property =
            DependencyProperty.Register("FillColor3", typeof(Color), typeof(GradientProgressBarUI), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));
        public static readonly DependencyProperty ProgressTextProperty =
            DependencyProperty.Register("ProgressText", typeof(string), typeof(GradientProgressBarUI), new PropertyMetadata(""));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GradientProgressBarUI), new PropertyMetadata(""));
        public static readonly DependencyProperty FillPercentageProperty =
            DependencyProperty.Register("FillPercentage", typeof(double), typeof(GradientProgressBarUI), new PropertyMetadata(100d));

        public GradientProgressBarUI()
        {
            InitializeComponent();
        }

        private void UpdateFillBar()
        {
            FillBar.Width = ActualWidth * FillPercentage / 100;

            var x = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5),
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                MappingMode = BrushMappingMode.RelativeToBoundingBox,
                SpreadMethod = GradientSpreadMethod.Pad,
                GradientStops = new GradientStopCollection(),
                Opacity = 1,
            };
            x.GradientStops.Add(new GradientStop(FillColor1, 0.0));
            x.GradientStops.Add(new GradientStop(FillColor2, 0.5));
            x.GradientStops.Add(new GradientStop(FillColor3, 1.0));

            FillBar.Fill = x;
        }
    }
}
