using System;
using System.Linq;
using System.Timers;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game
{
    public class AnimatedEntity : Entity
    {
        protected Timer AnimationTimer { get; set; }
        public Spritesheet Spritesheet { get; protected set; }
        public float Duration { get; protected set; }
        public bool DestroyOnAnimationEnd { get; set; }
        protected int SpriteIndex { get; set; }

        public AnimatedEntity(string name, Tags tag) : base(name, tag)
        {
            Sprite.Uri = IO.GlobalPaths.Sprites + "transparent.png";
            Spritesheet = new Spritesheet();
            AnimationTimer = new Timer();
        }

        public void SetTimer()
        {
            AnimationTimer.Interval = Duration / Spritesheet.Sprites.Length;
            AnimationTimer.Elapsed += AnimationTimer_Elapsed;
        }

        public override void Show()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                if (!Visible)
                {
                    Visible = true;
                    App.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddVisualComponent(VisualControl);
                    AnimationTimer.Start();
                }
            });
        }

        protected override void UpdateVisualControl()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                VisualControl.Image.Source = Sprite.Image;
                base.UpdateVisualControl();
            });
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SpriteIndex < Spritesheet.Sprites.Length)
            {
                Sprite = Spritesheet.Sprites[SpriteIndex];
                SpriteIndex++;
                UpdateVisualControl();
            }
            else if (DestroyOnAnimationEnd)
                Destroy();
        }

        public override void Destroy()
        {
            base.Destroy();
            AnimationTimer.Elapsed -= AnimationTimer_Elapsed;
        }
    }
}
