using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZombieGame.Game.Controls;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Background
    {
        protected VisualControl VisualComponent { get; set; }
        protected bool Visible { get; set; }
        public string SpriteFileName { get; set; }
        public Vector Position { get; set; }


        public Background()
        {
            VisualComponent = new VisualControl();
            Canvas.SetZIndex(VisualComponent, 0);
        }

        public void SetPosition(Vector v)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Canvas.SetLeft(VisualComponent, v.X);
                Canvas.SetTop(VisualComponent, -v.Y);
                Position = v;
            });
        }

        public void Show()
        {
            if (!Visible)
            {
                Visible = true;
                SetPosition(Position);
                VisualComponent.Image.Source = new BitmapImage(new System.Uri(IO.GlobalPaths.BackgroundSprites + SpriteFileName));
                App.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddVisualComponent(VisualComponent);
            }
        }

        public void Destroy()
        {
            if (Visible)
            {
                Visible = false;
                App.Current.Windows.OfType<MainWindow>().FirstOrDefault().RemoveVisualComponent(VisualComponent);
            }
        }


    }
}
