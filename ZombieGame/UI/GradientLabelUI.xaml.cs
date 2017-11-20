using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for GradientLabelUI.xaml
    /// </summary>
    public partial class GradientLabelUI : Label
    {
        public GradientLabelUI()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Foreground = new SolidColorBrush(Colors.White);
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
            x.GradientStops.Add(new GradientStop(Color.FromArgb(200, 200, 0, 0), 0.0));
            x.GradientStops.Add(new GradientStop(Color.FromArgb(200, 220, 50, 50), 0.5));
            x.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 100, 100), 1.0));
            Background = x;

            AnimateMouseEnter();
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Foreground = new SolidColorBrush(Colors.Black);
            Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            AnimateMouseLeave();
        }

        private void AnimateMouseEnter()
        {
            var T = new TranslateTransform(0, 0);
            Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 200));
            DoubleAnimation anim = new DoubleAnimation(20, duration);
            T.BeginAnimation(TranslateTransform.XProperty, anim);
            RenderTransform = T;
        }

        private void AnimateMouseLeave()
        {
            var T = new TranslateTransform(20, 0);
            Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 200));
            DoubleAnimation anim = new DoubleAnimation(0, duration);
            T.BeginAnimation(TranslateTransform.XProperty, anim);
            RenderTransform = T;
        }
    }
}
