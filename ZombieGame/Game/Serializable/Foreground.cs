using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Foreground : Background
    {
        public Foreground() : base()
        {
            Canvas.SetZIndex(VisualComponent, (int)ZIndexes.Foreground);
        }

        public override void Show()
        {
            if (!Visible)
            {
                Visible = true;
                SetPosition(Position);
                VisualComponent.Image.Source = new BitmapImage(new Uri(IO.GlobalPaths.ForegroundSprites + SpriteFileName));
                App.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddToCamera(VisualComponent);
            }
        }
    }
}
