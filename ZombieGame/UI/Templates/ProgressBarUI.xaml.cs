using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBarUI : UserControl
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
            DependencyProperty.Register("Background", typeof(Brush), typeof(ProgressBarUI), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
        public static readonly DependencyProperty ProgressTextProperty =
            DependencyProperty.Register("ProgressText", typeof(string), typeof(ProgressBarUI), new PropertyMetadata(""));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ProgressBarUI), new PropertyMetadata(""));
        public static readonly DependencyProperty FillPercentageProperty =
            DependencyProperty.Register("FillPercentage", typeof(double), typeof(ProgressBarUI), new PropertyMetadata(100d));

        public ProgressBarUI()
        {
            InitializeComponent();
        }
    }
}
