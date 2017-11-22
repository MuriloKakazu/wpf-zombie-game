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
        /// <summary>
        /// Lista estática de todas as entidades animadas
        /// </summary>
        private static List<AnimatedEntity> AnimatedEntities = new List<AnimatedEntity>();

        /// <summary>
        /// Conjunto de sprites da animação
        /// </summary>
        public Spritesheet Spritesheet { get; protected set; }
        /// <summary>
        /// Duração da animação, em segundos
        /// </summary>
        public float AnimationDuration { get; protected set; }
        /// <summary>
        /// Tempo decorrido desde a última troca de sprite
        /// </summary>
        protected float DeltaT { get; set; }
        /// <summary>
        /// Tempo alvo entre cada troca de sprite
        /// </summary>
        protected float TargetInterval { get; set; }
        /// <summary>
        /// Estado de execução da animação
        /// </summary>
        protected ExecutionState ExecutionState { get; set; }
        /// <summary>
        /// Retorna se a entidade será destruída após a animação
        /// </summary>
        public bool DestroyOnAnimationEnd { get; set; }
        /// <summary>
        /// Retorna se a entidade ficará em loop
        /// </summary>
        public bool LoopAnimation { get; set; }
        /// <summary>
        /// Retorna o índice atual da sprite da animação
        /// </summary>
        protected int SpriteIndex { get; set; }

        /// <summary>
        /// Retorna todas as instâncias do objeto em forma de array
        /// </summary>
        public new static AnimatedEntity[] Instances
        {
            get { return AnimatedEntities.ToArray(); }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tag"></param>
        public AnimatedEntity(string name, Tag tag) : base(name, tag)
        {
            Sprite = new TransparentSprite();
            Spritesheet = new Spritesheet();
            AnimatedEntities.Add(this);
        }

        /// <summary>
        /// Prepara o timer da animação
        /// </summary>
        public void SetTimer()
        {
            TargetInterval = AnimationDuration / Spritesheet.Sprites.Length;
        }

        /// <summary>
        /// Mostar a animação no canvas
        /// </summary>
        public override void Show()
        {
            if (!Visible)
            {
                Visible = true;
                GameMaster.TargetCanvas.AddChild(VisualControl);
                ExecutionState = ExecutionState.Running;
            }
        }

        /// <summary>
        /// Atualiza o componente visual da animação
        /// </summary>
        protected override void UpdateVisualControl()
        {
            if (!IsActive)
                return;

            VisualControl.Image.Source = Sprite.Image;
            base.UpdateVisualControl();
        }

        /// <summary>
        /// Pausa a animação
        /// </summary>
        public void PauseAnimation()
        {
            ExecutionState = ExecutionState.Paused;
        }

        /// <summary>
        /// Resume a execução da animação
        /// </summary>
        public void ResumeAnimation()
        {
            ExecutionState = ExecutionState.Running;
        }

        /// <summary>
        /// Método a ser chamado pelo timer de alta frequência
        /// </summary>
        protected override void FixedUpdate()
        {
            Animate();
            base.FixedUpdate();
        }

        /// <summary>
        /// Faz com que a animação mova para a próxima sprite, se necessário
        /// </summary>
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

        /// <summary>
        /// Destrói a entidade atual, marcando-a para ser coletada pelo Garbage Collector
        /// </summary>
        public override void Destroy()
        {
            AnimatedEntities.Remove(this);
            base.Destroy();
        }
    }
}
