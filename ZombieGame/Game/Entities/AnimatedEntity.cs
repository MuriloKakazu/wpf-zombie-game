﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Entities
{
    public class AnimatedEntity : Entity
    {
        private static List<AnimatedEntity> AnimatedEntities = new List<AnimatedEntity>();

        protected Timer AnimationTimer { get; set; }
        public Spritesheet Spritesheet { get; protected set; }
        public float AnimationDuration { get; protected set; }
        public bool DestroyOnAnimationEnd { get; set; }
        public bool LoopAnimation { get; set; }
        protected int SpriteIndex { get; set; }

        public new static AnimatedEntity[] AllInstances
        {
            get { return AnimatedEntities.ToArray(); }
        }

        public AnimatedEntity(string name, Tags tag) : base(name, tag)
        {
            Sprite.Uri = IO.GlobalPaths.Sprites + "transparent.png";
            Spritesheet = new Spritesheet();
            AnimationTimer = new Timer();
            AnimatedEntities.Add(this);
        }

        public void SetTimer()
        {
            AnimationTimer.Interval = AnimationDuration / Spritesheet.Sprites.Length;
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
            if (!IsActive)
                return;

            App.Current.Dispatcher.Invoke(delegate
            {
                VisualControl.Image.Source = Sprite.Image;
                base.UpdateVisualControl();
            });
        }

        public void PauseAnimation()
        {
            AnimationTimer.Stop();
        }

        public void ResumeAnimation()
        {
            AnimationTimer.Start();
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsActive)
                return;

            if (SpriteIndex < Spritesheet.Sprites.Length)
            {
                Sprite = Spritesheet.Sprites[SpriteIndex];
                SpriteIndex++;
                UpdateVisualControl();
            }
            else if (DestroyOnAnimationEnd)
                MarkAsNoLongerNeeded();
            else if (LoopAnimation)
            {
                SpriteIndex = 0;
                Sprite = Spritesheet.Sprites[SpriteIndex];
                UpdateVisualControl();
            }
        }

        public override void MarkAsNoLongerNeeded()
        {
            UnsubscribeFromEvents();
            base.MarkAsNoLongerNeeded();
        }

        protected override void UnsubscribeFromEvents()
        {
            AnimationTimer.Elapsed -= AnimationTimer_Elapsed;
            base.UnsubscribeFromEvents();
        }

        public override void Destroy()
        {
            AnimationTimer.Dispose();
            AnimatedEntities.Remove(this);
            base.Destroy();
        }
    }
}
