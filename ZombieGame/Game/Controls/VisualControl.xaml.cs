using System.Windows.Controls;
using System.Windows.Media;

namespace ZombieGame.Game.Controls
{
    /// <summary>
    /// Interaction logic for EntityVisualControl.xaml
    /// </summary>
    public partial class VisualControl : UserControl
    {
        public VisualControl()
        {
            InitializeComponent();

            if (GameMaster.Settings.AntiAliasingEnabled)
                RenderOptions.SetBitmapScalingMode(Image, BitmapScalingMode.HighQuality);
            else
                RenderOptions.SetBitmapScalingMode(Image, BitmapScalingMode.LowQuality);
        }
    }
}
