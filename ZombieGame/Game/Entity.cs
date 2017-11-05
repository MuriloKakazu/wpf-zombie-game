using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ZombieGame.Game.Controls;
using ZombieGame.Game.Enums;
using ZombieGame.IO;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game
{
    public abstract class Entity
    {
        #region Properties
        /// <summary>
        /// Lista estática copntendo todas as entidades ativas
        /// </summary>
        public static List<Entity> Entities = new List<Entity>();

        public delegate void CollisionHandler(object sender, CollisionEventArgs e);
        public event CollisionHandler CollisionEnter;
        public event CollisionHandler CollisionStay;
        public event CollisionHandler CollisionLeave;

        /// <summary>
        /// Código de identificação única do objeto
        /// </summary>
        public Guid Hash { get; protected set; }
        /// <summary>
        /// Retorna o nome da entidade
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Retorna a Tag da entidade
        /// </summary>
        public Tags Tag { get; protected set; }
        /// <summary>
        /// Retorna o RigidBody da entidade
        /// </summary>
        public RigidBody RigidBody { get; protected set; }
        /// <summary>
        /// Retorna uma lista com todas as entidades as quais se está colidindo
        /// </summary>
        protected List<Entity> Collisions { get; set; }
        /// <summary>
        /// Retorna o componente visual da entidade
        /// </summary>
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
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome da entidade</param>
        /// <param name="tag">Tag da entidade</param>
        public Entity(string name, Tags tag)
        {
            VisualControl = new VisualControl();
            Sprite = new Sprite("Not set");
            Hash = Guid.NewGuid();
            Name = name;
            Tag = tag;
            RigidBody = new RigidBody();
            Collisions = new List<Entity>();
            Entities.Add(this);
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            CollisionEnter += OnCollisionEnter;
            CollisionStay += OnCollisionStay;
            CollisionLeave += OnCollisionLeave;
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
        /// Evento a ser disparado quando a entidade entra em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            //Console.WriteLine("Collision enter: '{0}' and '{1}'", Name, e.Collider.Name);
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade mantém-se em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            if (e.Collider.Tag != Tags.Projectile && e.Collider.Tag != Tags.Wall)
            {
                //RigidBody.AddForce(e.CollisionDirection.Normalized * 50);
                RigidBody.AddVelocity(e.CollisionDirection * e.Collider.RigidBody.Velocity.Magnitude);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            //Console.WriteLine("Collision leave: '{0}' and '{1}'", Name, e.Collider.Name);
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

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        protected void Update()
        {
            UpdateVisualControl();
            CheckCollision();
        }

        /// <summary>
        /// Atualiza o componente visual associado a essa entidade
        /// </summary>
        protected void UpdateVisualControl()
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

        /// <summary>
        /// Checa colisões com todas as entidades ativas
        /// </summary>
        protected virtual void CheckCollision()
        {
            try
            {
                List<Entity> currentCollisions = new List<Entity>();
                foreach (var e in Entities)
                {
                    if (RigidBody.Bounds.RelativeToWindow().IntersectsWith(e.RigidBody.Bounds.RelativeToWindow()) && e.Hash != Hash)
                    {
                        currentCollisions.Add(e);
                        if (!Collisions.Contains(e))
                        {
                            var d = RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint);
                            CollisionEnter.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                        }
                    }
                }

                foreach (var e in Collisions)
                {
                    if (!currentCollisions.Contains(e))
                        CollisionLeave.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                    else if (currentCollisions.Contains(e))
                        CollisionStay.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                }

                Collisions = currentCollisions;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Retorna um conjunto de entidades no raio especificado
        /// </summary>
        /// <param name="radius">Raio de procura</param>
        /// <returns>Entity(Array)</returns>
        public Entity[] GetNearbyEntities(float radius)
        {
            List<Entity> entities = new List<Entity>();
            foreach (var e in Entities)
                if ((e.RigidBody.CenterPoint + RigidBody.CenterPoint).Magnitude <= radius)
                    entities.Add(e);
            return entities.ToArray();
        }

        /// <summary>
        /// Destrói a entidade caso ela não precise mais ser utilizada
        /// </summary>
        protected virtual void Destroy()
        {
            Entities.Remove(this);
            GameMaster.UpdateTimer.Elapsed -= UpdateTimer_Elapsed;
            CollisionEnter -= OnCollisionEnter;
            CollisionStay -= OnCollisionStay;
            CollisionLeave -= OnCollisionLeave;
            Collisions.Clear();
        }
        #endregion
    }
}
