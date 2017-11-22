using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ZombieGame.Game.Controls;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Background
    {
        [XmlIgnore]
        public VisualControl VisualComponent { get; set; }
        protected bool Visible { get; set; }
        public string SpriteFileName { get; set; }
        public Vector Position { get; set; }


        public Background()
        {
            VisualComponent = new VisualControl();
            Canvas.SetZIndex(VisualComponent, (int)ZIndex.Background);

            var CachedBitmap = new BitmapCache();
            CachedBitmap.RenderAtScale = 0.35 * GameMaster.Settings.RenderScale;
            CachedBitmap.EnableClearType = false;
            CachedBitmap.SnapsToDevicePixels = true;
            VisualComponent.CacheMode = CachedBitmap;
            RenderOptions.SetBitmapScalingMode(VisualComponent, BitmapScalingMode.LowQuality);
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

        public virtual void Show()
        {
            if (!Visible)
            {
                Visible = true;
                SetPosition(Position);
                VisualComponent.Image.Source = new BitmapImage(new Uri(IO.GlobalPaths.BackgroundSprites + SpriteFileName));
                GameMaster.TargetCanvas.AddChild(VisualComponent);
            }
        }

        public void Destroy()
        {
            if (Visible)
            {
                Visible = false;
                GameMaster.TargetCanvas.RemoveChild(VisualComponent);
            }
        }


    }
}
