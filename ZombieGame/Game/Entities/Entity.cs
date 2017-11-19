using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ZombieGame.Game.Controls;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Interfaces;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game.Entities
{
    public abstract class Entity : IDestroyable
    {
        #region Properties
        /// <summary>
        /// Lista estática copntendo todas as entidades ativas
        /// </summary>
        private static List<Entity> Entities = new List<Entity>();

        public delegate void CollisionHandler(object sender, CollisionEventArgs e);
        public event CollisionHandler CollisionEnter;
        public event CollisionHandler CollisionStay;
        public event CollisionHandler CollisionLeave;

        /// <summary>
        /// Código de identificação única do objeto
        /// </summary>
        public virtual Guid Hash { get; protected set; }
        /// <summary>
        /// Retorna o nome da entidade
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// Retorna a Tag da entidade
        /// </summary>
        public virtual Tags Tag { get; protected set; }
        /// <summary>
        /// Retorna o RigidBody da entidade
        /// </summary>
        public virtual RigidBody RigidBody { get; protected set; }
        /// <summary>
        /// Retorna uma lista com todas as entidades as quais se está colidindo
        /// </summary>
        protected virtual List<Entity> Collisions { get; set; }
        /// <summary>
        /// Retorna o componente visual da entidade
        public virtual VisualControl VisualControl { get; protected set; }
        private BitmapCache CachedBitmap { get; set; }
        /// <summary>
        /// Retorna a sprite da entidade
        /// </summary>
        public virtual Sprite Sprite { get; protected set; }
        /// <summary>
        /// Retorna se a entidade pertence a um jogador
        /// </summary>
        public virtual bool IsPlayer { get { return Tag == Tags.Player; } }
        /// <summary>
        /// Retorna se a entidade é um inimigo
        /// </summary>
        public virtual bool IsEnemy { get { return Tag == Tags.Enemy; } }
        /// <summary>
        /// Retorna se a entidade é uma câmera
        /// </summary>
        public virtual bool IsCamera { get { return Tag == Tags.Camera; } }
        public virtual bool IsVisualFX { get { return Tag == Tags.VisualFX; } }
        public virtual bool IsWall { get { return Tag == Tags.Wall; } }
        /// <summary>
        /// Retorna se a entidade é visível
        /// </summary>
        public virtual bool Visible { get; protected set; }
        /// <summary>
        /// Retorna se a entidade tem um componente visual instanciado
        /// </summary>
        public virtual bool HasVisualControl { get { return VisualControl != null; } }
        protected virtual int ZIndex { get; private set; }
        public bool IsActive { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todas as entidades ativas
        /// </summary>
        /// <returns></returns>
        public static Entity[] Instances
        {
            get { return Entities.ToArray(); }
        }

        public static void RemoveAllOfType(Tags type)
        {
            foreach (var v in Entities)
                if (v.Tag == type)
                    v.MarkAsNoLongerNeeded();
        }

        protected Entity() { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome da entidade</param>
        /// <param name="tag">Tag da entidade</param>
        public Entity(string name = "Unknown", Tags tag = Tags.Undefined)
        {
            IsActive = true;
            Sprite = new Sprite();
            Hash = Guid.NewGuid();
            Name = name;
            Tag = tag;
            RigidBody = new RigidBody();
            Collisions = new List<Entity>();
            Entities.Add(this);
            CreateVisualControl();
            SetZIndex(ZIndexes.BackTile);
            Time.HighFrequencyTimer.Elapsed += HighFrequencyTimer_Elapsed;
            Time.LowFrequencyTimer.Elapsed += LowFrequencyTimer_Elapsed;
            CollisionEnter += OnCollisionEnter;
            CollisionStay += OnCollisionStay;
            CollisionLeave += OnCollisionLeave;

            CachedBitmap = new BitmapCache();
            CachedBitmap.RenderAtScale = 2;
            VisualControl.CacheMode = CachedBitmap;
        }

        public virtual void CreateVisualControl()
        {
            VisualControl = new VisualControl();
        }

        protected virtual void SetZIndex(ZIndexes index)
        {
            ZIndex = (int)index;
            Canvas.SetZIndex(VisualControl, (int)index);
        }

        /// <summary>
        /// Carrega a sprite para a entidade atual
        /// </summary>
        /// <param name="path">Caminho do arquivo</param>
        public virtual void LoadSprite(string path)
        {
            Sprite = new Sprite(path);
            VisualControl.Image.Source = Sprite.Image;
        }

        /// <summary>
        /// Torna visível o componente visual da entidade
        /// </summary>
        public virtual void Show()
        {
            if (!Visible)
            {
                Visible = true;
                VisualControl.Image.Source = Sprite.Image;
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddToCamera(VisualControl);
                UpdateVisualControl();
            }
        }

        /// <summary>
        /// Torna invisível o compoente visual da entidade
        /// </summary>
        public virtual void Hide()
        {
            if (Visible)
            {
                Visible = false;
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().RemoveFromCamera(VisualControl);
                UpdateVisualControl();
            }
        }

        public virtual void MarkAsNoLongerNeeded()
        {
            IsActive = false;
            UnsubscribeFromEvents();
            Hide();
        }

        /// <summary>
        /// Atualiza o componente visual associado a essa entidade
        /// </summary>
        protected virtual void FixedUpdateVisualControl()
        {
            if (IsActive)
                VisualControl.RenderTransform = new TranslateTransform(RigidBody.Position.X, -RigidBody.Position.Y);
        }

        protected virtual void UpdateVisualControl()
        {
            if (Visible)
            {
                VisualControl.Width = RigidBody.Size.X;
                VisualControl.Height = RigidBody.Size.Y;
                VisualControl.Image.RenderTransform = new RotateTransform(RigidBody.Rotation + 45);
            }
        }

        /// <summary>
        /// Checa colisões com todas as entidades ativas
        /// </summary>
        protected virtual void CheckCollision()
        {
            try
            {
                List<Entity> currentCollisions = new List<Entity>();
                foreach (var e in Entities.ToArray())
                {
                    if (e != null && e.Hash != Hash && e.IsActive && this.IsActive && !RigidBody.IgnoreCollisions && !e.RigidBody.IgnoreCollisions && RigidBody.Bounds.RelativeToWindow().IntersectsWith(e.RigidBody.Bounds.RelativeToWindow()))
                    {
                        currentCollisions.Add(e);
                        if (!Collisions.ToArray().Contains(e))
                        {
                            var d = RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint);
                            CollisionEnter?.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                        }
                    }
                }

                foreach (var e in Collisions.ToArray())
                {
                    if (e != null && e.IsActive && this.IsActive && !RigidBody.IgnoreCollisions && !e.RigidBody.IgnoreCollisions)
                    {
                        if (!currentCollisions.Contains(e))
                            CollisionLeave?.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                        else if (currentCollisions.Contains(e))
                            CollisionStay?.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                    }
                }

                Collisions = currentCollisions;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Retorna a instância da entidade do jogador mais próximo
        /// </summary>
        /// <returns>Entity</returns>
        public virtual Entity GetNearestPlayer()
        {
            try
            {
                Dictionary<float, Entity> candidates = new Dictionary<float, Entity>();

                foreach (var player in GameMaster.Players)
                {
                    if (player.IsPlaying && player.Character.IsActive)
                    {
                        var distance = player.Character.RigidBody.CenterPoint.DistanceBetween(RigidBody.CenterPoint);
                        if (!candidates.TryGetValue(distance, out Entity e))
                            candidates.Add(distance, player.Character);
                    }
                }

                Entity returnValue;

                var success = candidates.TryGetValue(candidates.Keys.Min(), out returnValue);

                if (success)
                    return returnValue;
            }
            catch { /*Same distance to both players. Can't add to dictionary*/ }
            return GameMaster.GetPlayer(0).Character;
        }

        /// <summary>
        /// Retorna um conjunto de entidades no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Entity(Array)</returns>
        public virtual Entity[] GetNearbyEntities(float radius, int maxValue)
        {
            try
            {
                List<Entity> entities = new List<Entity>();
                foreach (var e in Entities.ToArray())
                    if ((e.RigidBody.CenterPoint - RigidBody.CenterPoint).Magnitude <= radius && e.Hash != Hash && entities.Count < maxValue)
                        entities.Add(e);
                return entities.ToArray();
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Destrói a entidade caso ela não precise mais ser utilizada
        /// </summary>
        public virtual void Destroy()
        {
            IsActive = false;
            Hide();
            VisualControl = null;
            Entities.Remove(this);
            Collisions.Clear();
        }

        protected virtual void UnsubscribeFromEvents()
        {
            Time.HighFrequencyTimer.Elapsed -= HighFrequencyTimer_Elapsed;
            Time.LowFrequencyTimer.Elapsed -= LowFrequencyTimer_Elapsed;
            CollisionEnter -= OnCollisionEnter;
            CollisionStay -= OnCollisionStay;
            CollisionLeave -= OnCollisionLeave;
        }
      
        /// <summary>
        /// Evento a ser disparado quando a entidade entra em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            // Por padrão, nada acontece
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade mantém-se em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            if (e.Collider.Tag != Tags.Projectile && e.Collider.Tag != Tags.Wall && !e.Collider.IsCamera)
            {
                RigidBody.SetMomentum(e.CollisionDirection * e.Collider.RigidBody.Momentum.Magnitude);
                RigidBody.PointAt(e.CollisionDirection.Opposite);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            // Por padrão, nada acontece
        }

        /// <summary>
        /// Evento a ser disparado quando o intervalo do timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito do evento</param>
        protected virtual void HighFrequencyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { FixedUpdate(); });
        }

        protected virtual void LowFrequencyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { Update(); });
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        protected virtual void FixedUpdate()
        {
            RigidBody.FixedUpdate();
            FixedUpdateVisualControl();
            CheckCollision();
        }

        protected virtual void Update()
        {
            UpdateVisualControl();
            Test();
        }

        private void Test()
        {
            if (!Collisions.Contains(GameMaster.Camera) && Visible && IsActive && !this.IsVisualFX)
                Hide();
            else if (Collisions.Contains(GameMaster.Camera) && !Visible && IsActive && !this.IsVisualFX)
                Show();
        }
        #endregion
    }
}
