using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Sprites;
using ZombieGame.Physics;

namespace ZombieGame.Game.Entities
{
    public class AnimatedEntity : Entity
    {
        private static List<AnimatedEntity> AnimatedEntities = new List<AnimatedEntity>();

        public Spritesheet Spritesheet { get; protected set; }
        public float AnimationDuration { get; protected set; }
        protected float DeltaT { get; set; }
        protected float TargetInterval { get; set; }
        protected ExecutionState ExecutionState { get; set; }
        public bool DestroyOnAnimationEnd { get; set; }
        public bool LoopAnimation { get; set; }
        protected int SpriteIndex { get; set; }

        public new static AnimatedEntity[] Instances
        {
            get { return AnimatedEntities.ToArray(); }
        }

        public AnimatedEntity(string name, Tag tag) : base(name, tag)
        {
            Sprite = new TransparentSprite();
            Spritesheet = new Spritesheet();
            AnimatedEntities.Add(this);
        }

        public void SetTimer()
        {
            TargetInterval = AnimationDuration / Spritesheet.Sprites.Length;
        }

        public override void Show()
        {
            if (!Visible)
            {
                Visible = true;
                GameMaster.TargetCanvas.AddChild(VisualControl);
                ExecutionState = ExecutionState.Running;
            }
        }

        protected override void UpdateVisualControl()
        {
            if (!IsActive)
                return;

            VisualControl.Image.Source = Sprite.Image;
            base.UpdateVisualControl();
        }

        public void PauseAnimation()
        {
            ExecutionState = ExecutionState.Paused;
        }

        public void ResumeAnimation()
        {
            ExecutionState = ExecutionState.Running;
        }

        protected override void FixedUpdate()
        {
            Animate();
            base.FixedUpdate();
        }

        public void Animate()
        {
            if (ExecutionState == ExecutionState.Running)
            {
                DeltaT += Time.Delta * 1000;

                if (DeltaT >= TargetInterval)
                {
                    DeltaT = 0;
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
            }
        }

        public override void Destroy()
        {
            AnimatedEntities.Remove(this);
            base.Destroy();
        }
    }
}
