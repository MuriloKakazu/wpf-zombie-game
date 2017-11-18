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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBar : UserControl
    {
        public string ProgressText
        {
            get { return (string)GetValue(ProgressTextProperty); }
            set { SetValue(ProgressTextProperty, value); ProgressLabel.Content = value; }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); TitleLabel.Content = value; }
        }
        public double FillPercentage
        {
            get { return (double)GetValue(FillPercentageProperty); }
            set{ SetValue(FillPercentageProperty, value); FillBar.Width = ActualWidth * FillPercentage / 100; }
        }
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); FillBar.Fill = value; }
        }

        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(ProgressBar), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty ProgressTextProperty =
            DependencyProperty.Register("ProgressText", typeof(string), typeof(ProgressBar), new PropertyMetadata(""));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ProgressBar), new PropertyMetadata(""));
        public static readonly DependencyProperty FillPercentageProperty =
            DependencyProperty.Register("FillPercentage", typeof(double), typeof(ProgressBar), new PropertyMetadata(100d));

        public ProgressBar()
        {
            InitializeComponent();
        }
    }
}
