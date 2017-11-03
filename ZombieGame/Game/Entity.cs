using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

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
        public string Name { get; set; }
        /// <summary>
        /// Retorna a Tag da entidade
        /// </summary>
        public Tags Tag { get; set; }
        /// <summary>
        /// Retorna o RigidBody da entidade
        /// </summary>
        public RigidBody RigidBody { get; set; }
        /// <summary>
        /// Retorna um booleano afirmando se a entidade está aterrada
        /// </summary>
        public bool IsGrounded { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome da entidade</param>
        public Entity(string name)
        {
            Name = name;
            Tag = Tags.Undefined;
            RigidBody = new RigidBody();
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
            Console.WriteLine("Collision with {0}", e.Collider.Name);
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            throw new NotImplementedException();
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
            foreach (var e in Entities)
            {
                if (RigidBody.Bounds.IntersectsWith(e.RigidBody.Bounds))
                    CollisionEnter?.Invoke(this, new CollisionEventArgs(e, RigidBody.Position.PointedAt(e.RigidBody.Position)));
            }
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
