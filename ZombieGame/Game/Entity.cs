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
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Nome da entidade</param>
        /// <param name="tag">Tag da entidade</param>
        public Entity(string name, Tags tag)
        {
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
        /// Evento a ser disparado quando a entidade entra em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            Console.WriteLine("Collision enter: '{0}' and '{1}'", Name, e.Collider.Name);
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade mantém-se em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            Console.WriteLine("Collision stay: '{0}' and '{1}'", Name, e.Collider.Name);

            if (Tag == Tags.TopWall &&
            e.Collider.Tag != Tags.BottomWall &&
            e.Collider.Tag != Tags.LeftWall &&
            e.Collider.Tag != Tags.RightWall &&
            e.Collider.Tag != Tags.TopWall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y < RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y > RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y)
                    e.Collider.RigidBody.SetPosition(new Vector(e.Collider.RigidBody.Position.X, RigidBody.Position.Y + e.Collider.RigidBody.Size.Y));
            }
            else if (Tag == Tags.BottomWall &&
            e.Collider.Tag != Tags.BottomWall &&
            e.Collider.Tag != Tags.LeftWall &&
            e.Collider.Tag != Tags.RightWall &&
            e.Collider.Tag != Tags.TopWall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y > RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y < RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y)
                    e.Collider.RigidBody.SetPosition(new Vector(e.Collider.RigidBody.Position.X, RigidBody.Position.Y - RigidBody.Size.Y));
            }
            else if (Tag == Tags.LeftWall &&
            e.Collider.Tag != Tags.BottomWall &&
            e.Collider.Tag != Tags.LeftWall &&
            e.Collider.Tag != Tags.RightWall &&
            e.Collider.Tag != Tags.TopWall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterRight).X > RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterRight).X < RigidBody.Bounds.GetVector(RectPositions.CenterRight).X)
                    e.Collider.RigidBody.SetPosition(new Vector(RigidBody.Position.X - e.Collider.RigidBody.Size.X, e.Collider.RigidBody.Position.Y));
            }
            else if (Tag == Tags.RightWall &&
            e.Collider.Tag != Tags.BottomWall &&
            e.Collider.Tag != Tags.LeftWall &&
            e.Collider.Tag != Tags.RightWall &&
            e.Collider.Tag != Tags.TopWall)
            {
                if (e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X < RigidBody.Bounds.GetVector(RectPositions.CenterRight).X &&
                    e.Collider.RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X > RigidBody.Bounds.GetVector(RectPositions.CenterLeft).X)
                    e.Collider.RigidBody.SetPosition(new Vector(RigidBody.Position.X + RigidBody.Size.X, e.Collider.RigidBody.Position.Y));
            }
            else if (Tag != Tags.TopWall && 
            Tag != Tags.BottomWall && 
            Tag != Tags.LeftWall && 
            Tag != Tags.RightWall && 
            e.Collider.Tag != Tags.TopWall && 
            e.Collider.Tag != Tags.BottomWall &&
            e.Collider.Tag != Tags.LeftWall &&
            e.Collider.Tag != Tags.RightWall)
            {
                RigidBody.AddVelocity(e.CollisionDirection.Normalized * e.Collider.RigidBody.Velocity.Magnitude);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected virtual void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            Console.WriteLine("Collision leave: '{0}' and '{1}'", Name, e.Collider.Name);
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

            //bool grounded = false;

            //foreach (var e in currentCollisions)
            //{
            //    if (e.Tag == Tags.TopWall && e.RigidBody.Bounds.GetVector(RectPositions.CenterBottom).Y - RigidBody.Bounds.GetVector(RectPositions.CenterTop).Y < 0)
            //    {
            //        grounded = true;
            //        RigidBody.SetForce(Vector.Zero);
            //        break;
            //    }
            //}

            //RigidBody.IsGrounded = grounded;

            foreach (var e in Collisions)
            {
                if (!currentCollisions.Contains(e))
                    CollisionLeave.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                else if (currentCollisions.Contains(e))
                    CollisionStay.Invoke(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
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
