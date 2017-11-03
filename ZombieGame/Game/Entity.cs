using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game
{
    public class Entity
    {
        #region Properties
        /// <summary>
        /// Lista estática de todas as entidades ativas
        /// </summary>
        public static List<Entity> Entities = new List<Entity>();

        public delegate void CollisionHandler(object sender, CollisionEventArgs e);
        public event CollisionHandler CollisionEnter;
        public event CollisionHandler CollisionLeave;

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
        /// Retorna um booleano afirmando se a entidade está aterrada
        /// </summary>
        public bool IsGrounded { get; protected set; }
        /// <summary>
        /// Retorna uma lista com todas as entidades as quais se está colidindo
        /// </summary>
        protected List<Entity> Collisions { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome da entidade</param>
        /// <param name="tag">Tag da entidade</param>
        public Entity(string name, Tags tag)
        {
            Name = name;
            Tag = tag;
            RigidBody = new RigidBody();
            Collisions = new List<Entity>();
            Entities.Add(this);
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            CollisionEnter += OnCollisionEnter;
            CollisionLeave += OnCollisionLeave;
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade entra em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            if (Tag != Tags.Ground && Tag != Tags.Ceiling && e.Collider.Tag != Tags.Ground && e.Collider.Tag != Tags.Ceiling)
                RigidBody.AddVelocity(e.CollisionDirection * e.Collider.RigidBody.Velocity.Magnitude);
            else if (Tag == Tags.Ground)
                e.Collider.RigidBody.AddVelocity(Vector.Up * -e.Collider.RigidBody.Velocity);
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            Console.WriteLine(e.Collider.Name);
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
            CheckCollision();
        }

        /// <summary>
        /// Checa colisões com todas as entidades ativas
        /// </summary>
        protected void CheckCollision()
        {
            List<Entity> currentCollisions = new List<Entity>();
            foreach (var e in Entities)
            {
                if (RigidBody.Bounds.IntersectsWith(e.RigidBody.Bounds))
                {
                    CollisionEnter?.Invoke(this, new CollisionEventArgs(e, RigidBody.Bounds.GetVector(RectPositions.Center).PointedAt(e.RigidBody.Bounds.GetVector(RectPositions.Center)).Normalized));
                    currentCollisions.Add(e);
                }
            }

            bool grounded = false;

            foreach (var e in currentCollisions)
            {
                if (e.Tag == Tags.Ground)
                {
                    grounded = true;
                    break;
                }
            }

            IsGrounded = grounded;

            foreach (var e in Collisions)
            {
                if (!currentCollisions.Contains(e))
                    CollisionLeave.Invoke(this, new CollisionEventArgs(e, RigidBody.Bounds.GetVector(RectPositions.Center).PointedAt(e.RigidBody.Bounds.GetVector(RectPositions.Center)).Normalized));
            }

            Collisions = currentCollisions;
        }

        /// <summary>
        /// Destrói a entidade caso ela não precise mais ser utilizada
        /// </summary>
        public void Destroy()
        {

        }
        #endregion
    }
}
