using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Projectiles;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Game
{
    public abstract class Projectile : Entity
    {
        /// <summary>
        /// Lista de todos os projéteis ativos
        /// </summary>
        protected static List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// Retorna se o projétil explode com o impacto
        /// </summary>
        public bool IsExplosive { get; set; }
        /// <summary>
        /// Dano de impacto do projétil
        /// </summary>
        public int HitDamage { get; protected set; }
        /// <summary>
        /// Entidade que invocou o projétil
        /// </summary>
        public Character Owner { get; set; }
        /// <summary>
        /// Tipo do projétil
        /// </summary>
        public ProjectileTypes Type { get; protected set; }
        /// <summary>
        /// Módulo da velocidade do projétil
        /// </summary>
        public float SpeedMagnitude { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">Tipo de projétil</param>
        public Projectile(ProjectileTypes type) : base(type.ToString(), Tags.Projectile)
        {
            Projectiles.Add(this);
        }

        /// <summary>
        /// Lança o projétil em uma direção
        /// </summary>
        /// <param name="dir">Direção de lançamento</param>
        public void Launch(Vector dir)
        {
            RigidBody.SetPosition(Owner.RigidBody.CenterPoint);
            RigidBody.UseRotation = true;
            RigidBody.SetVelocity(dir.Normalized * SpeedMagnitude);
        }

        /// <summary>
        /// Cria uma explosão na posição solicitada
        /// </summary>
        /// <param name="pos">Posição da explosão</param>
        protected void ExplodeAt(Vector pos, float explosionRadius)
        {
            var targets = Character.GetNearbyCharacters(pos, explosionRadius);

            if (targets != null)
            {
                foreach (var t in targets)
                {
                    t.RigidBody.AddForce(t.RigidBody.CenterPoint.PointedAt(RigidBody.CenterPoint).Normalized * 500);
                    Console.WriteLine("Projectile exploded on {0}", t.Name);
                }
            }
        }

        /// <summary>
        /// Retorna uma instância de projétil dependendo do tipo fornecido
        /// </summary>
        /// <param name="type">Tipo de projétil</param>
        /// <returns>Projectile</returns>
        public static Projectile OfType(ProjectileTypes type)
        {
            if (type == ProjectileTypes.HeavyMGProjectile)
                return new HeavyMGProjectile();
            else if (type == ProjectileTypes.Missile)
                return new Missile();
            else if (type == ProjectileTypes.PistolProjectile)
                return new PistolProjectile();
            else if (type == ProjectileTypes.RifleProjectile)
                return new RifleProjectile();
            else if (type == ProjectileTypes.SniperProjectile)
                return new SniperProjectile();
            return null;
        }

        /// <summary>
        /// Retorna todos os projéteis ativos
        /// </summary>
        /// <returns></returns>
        public static Projectile[] GetAllActiveProjectiles()
        {
            return Projectiles.ToArray();
        }

        /// <summary>
        /// Checa colisão com outras entidades
        /// </summary>
        protected override void CheckCollision()
        {
            try
            {
                List<Entity> currentCollisions = new List<Entity>();
                foreach (var e in Entities.ToArray())
                {
                    if (e != null && RigidBody.Bounds.RelativeToWindow().IntersectsWith(e.RigidBody.Bounds.RelativeToWindow()) && e.Hash != Hash)
                    {
                        currentCollisions.Add(e);
                        if (!Collisions.ToArray().Contains(e))
                        {
                            var d = RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint);
                            OnCollisionEnter(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                        }
                    }
                }

                foreach (var e in Collisions.ToArray())
                {
                    if (e != null)
                    {
                        if (!currentCollisions.Contains(e))
                            OnCollisionLeave(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                        else if (currentCollisions.Contains(e))
                            OnCollisionStay(this, new CollisionEventArgs(e, RigidBody.CenterPoint.PointedAt(e.RigidBody.CenterPoint).Normalized));
                    }
                }

                Collisions = currentCollisions;
            }
            catch { }
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil entra em colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            Console.WriteLine("Projectile collided with {0}", e.Collider.Name);

            var colliderChar = Character.FromHashCode(e.Collider.Hash);

            if (e.Collider != Owner)
            {
                if (colliderChar != null)
                {
                    colliderChar.Damage(HitDamage);
                    colliderChar.RigidBody.AddForce(e.CollisionDirection.Opposite.Normalized * 100);
                    if (IsExplosive)
                        ExplodeAt(RigidBody.CenterPoint, 200);
                    Destroy();
                }
                else if (e.Collider.Tag == Tags.Projectile)
                {
                    var p = Projectile.FromHashCode(e.Collider.Hash);
                    if (p != null && p.Owner != Owner)
                    {
                        Destroy();
                    }
                }
                else
                {
                    Destroy();
                }
            }
            else
            {
                if (colliderChar != null)
                {
                    colliderChar.RigidBody.AddForce(colliderChar.RigidBody.Front.Opposite * 1000);
                }
            }
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil continua em colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            //
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil sai de colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            //base.OnCollisionLeave(sender, e);
        }

        /// <summary>
        /// Retorna uma instância do objeto Projectile
        /// </summary>
        /// <param name="hash">Código de identificação do objeto</param>
        /// <returns>Projectile</returns>
        public static Projectile FromHashCode(Guid hash)
        {
            try
            {
                foreach (Projectile p in Projectiles.ToArray())
                    if (p.Hash == hash)
                        return p;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Destrói o projétil para liberar espaço na memória
        /// </summary>
        protected override void Destroy()
        {
            base.Destroy();
            Projectiles.Remove(this);
        }
    }
}
