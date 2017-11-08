using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using ZombieGame.Game.Controls;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game
{
    [Serializable]
    public abstract class Entity
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
        [XmlIgnore]
        public Guid Hash { get; protected set; }
        /// <summary>
        /// Retorna o nome da entidade
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Retorna a Tag da entidade
        /// </summary>
        [XmlIgnore]
        public Tags Tag { get; protected set; }
        /// <summary>
        /// Retorna o RigidBody da entidade
        /// </summary>
        public RigidBody RigidBody { get; set; }
        /// <summary>
        /// Retorna uma lista com todas as entidades as quais se está colidindo
        /// </summary>
        protected List<Entity> Collisions { get; set; }
        /// <summary>
        /// Retorna o componente visual da entidade
        [XmlIgnore]
        public VisualControl VisualControl { get; set; }
        /// <summary>
        /// Retorna a sprite da entidade
        /// </summary>
        public Sprite Sprite { get; set; }
        /// <summary>
        /// Retorna se a entidade pertence a um jogador
        /// </summary>
        public bool IsPlayer { get { return Tag == Tags.Player; } }
        /// <summary>
        /// Retorna se a entidade é um inimigo
        /// </summary>
        public bool IsEnemy { get { return Tag == Tags.Enemy; } }
        /// <summary>
        /// Retorna se a entidade é uma câmera
        /// </summary>
        public bool IsCamera { get { return Tag == Tags.Camera; } }
        /// <summary>
        /// Retorna se a entidade é visível
        /// </summary>
        [XmlIgnore]
        public bool Visible { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todas as entidades ativas
        /// </summary>
        /// <returns></returns>
        public static Entity[] GetAllActiveEntities()
        {
            return Entities.ToArray();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome da entidade</param>
        /// <param name="tag">Tag da entidade</param>
        public Entity(string name, Tags tag)
        {
            Sprite = new Sprite();
            Hash = Guid.NewGuid();
            Name = name;
            Tag = tag;
            RigidBody = new RigidBody();
            Collisions = new List<Entity>();
            Entities.Add(this);
            VisualControl = new VisualControl();
            UpdateZIndex();
            Time.InternalTimer.Elapsed += UpdateTimer_Elapsed;
            CollisionEnter += OnCollisionEnter;
            CollisionStay += OnCollisionStay;
            CollisionLeave += OnCollisionLeave;
            if (Tag == Tags.Wall)
                Visible = true;
        }

        /// <summary>
        /// Carrega a sprite para a entidade atual
        /// </summary>
        /// <param name="path">Caminho do arquivo</param>
        public void LoadSprite(string path)
        {
            Sprite = new Sprite(path);
            VisualControl.Image.Source = Sprite.Image;
        }

        /// <summary>
        /// Torna visível o componente visual da entidade
        /// </summary>
        public void Show()
        {
            if (!Visible)
            {
                Visible = true;
                VisualControl.Image.Source = Sprite.Image;
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddVisualComponent(VisualControl);
                UpdateVisualControl();
            }
        }

        /// <summary>
        /// Torna invisível o compoente visual da entidade
        /// </summary>
        public void Hide()
        {
            if (Visible)
            {
                Visible = false;
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().RemoveVisualComponent(VisualControl);
                UpdateVisualControl();
            }
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        protected virtual void Update()
        {
            UpdateVisualControl();
            CheckCollision();
        }

        /// <summary>
        /// Atualiza o componente visual associado a essa entidade
        /// </summary>
        protected virtual void UpdateVisualControl()
        {
            if (Visible)
            {
                try
                {
                    VisualControl.Dispatcher.Invoke(new Action(() =>
                    {
                        Canvas.SetLeft(VisualControl, RigidBody.Position.X);
                        Canvas.SetTop(VisualControl, -RigidBody.Position.Y);
                        VisualControl.Width = RigidBody.Size.X;
                        VisualControl.Height = RigidBody.Size.Y;
                        VisualControl.Image.RenderTransform = new RotateTransform(RigidBody.Rotation + 45);
                    }));
                }
                catch { }
            }
        }

        /// <summary>
        /// Altera a ordem de renderização do compoente visual da entidade
        /// </summary>
        protected virtual void UpdateZIndex()
        {
            if (Tag == Tags.Projectile)
                Canvas.SetZIndex(VisualControl, 0);
            else if (Tag == Tags.Enemy)
                Canvas.SetZIndex(VisualControl, 1);
            else if (Tag == Tags.Player)
                Canvas.SetZIndex(VisualControl, 2);
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
                    if (e != null && RigidBody.Bounds.RelativeToWindow().IntersectsWith(e.RigidBody.Bounds.RelativeToWindow()) && e.Hash != Hash && e.Visible && this.Visible)
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
                    if (e != null && e.Visible && this.Visible)
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
        public Entity GetNearestPlayer()
        {
            Dictionary<float, Entity> candidates = new Dictionary<float, Entity>();

            foreach (var player in GameMaster.Players)
            {
                var distance = player.Character.RigidBody.CenterPoint.DistanceBetween(RigidBody.CenterPoint);
                candidates.Add(distance, player.Character);
            }

            Entity returnValue;

            var success = candidates.TryGetValue(candidates.Keys.Min(), out returnValue);

            if (success)
                return returnValue;
            return null;
        }

        /// <summary>
        /// Retorna um conjunto de entidades no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Entity(Array)</returns>
        public Entity[] GetNearbyEntities(float radius)
        {
            try
            {
                List<Entity> entities = new List<Entity>();
                foreach (var e in Entities.ToArray())
                    if ((e.RigidBody.CenterPoint - RigidBody.CenterPoint).Magnitude <= radius)
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
            App.Current.Dispatcher.Invoke(delegate { Hide(); });
            Entities.Remove(this);
            Time.InternalTimer.Elapsed -= UpdateTimer_Elapsed;
            CollisionEnter -= OnCollisionEnter;
            CollisionStay -= OnCollisionStay;
            CollisionLeave -= OnCollisionLeave;
            Collisions.Clear();
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
                RigidBody.AddVelocity(e.CollisionDirection * e.Collider.RigidBody.Velocity.Magnitude);
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
        protected virtual void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
            RigidBody.Update();
        }
        #endregion
    }
}
