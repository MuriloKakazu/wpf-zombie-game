using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Projectiles;
using ZombieGame.Physics;
using ZombieGame.Physics.Enums;
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

        #region Properties
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
        [XmlIgnore]
        public Character Owner { get; set; }
        /// <summary>
        /// Tipo do projétil
        /// </summary>
        public ProjectileTypes Type { get; protected set; }

        /// <summary>
        /// O valor primitivo da velocidade
        /// </summary>
        private float speedMagnitude = 40;

        /// <summary>
        /// Módulo da velocidade do projétil
        /// </summary>
        public float SpeedMagnitude
        {
            get { return speedMagnitude; }
            protected set
            {
                if (value < 25)
                    speedMagnitude = 25;
                else if (value > 150)
                    speedMagnitude = 150;
                else
                    speedMagnitude = value;
            }
        }
        public float KnockbackMagnitude { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">Tipo de projétil</param>
        public Projectile(ProjectileTypes type) : base(type.ToString() + "Projectile", Tags.Projectile)
        {
            Projectiles.Add(this);
        }

        /// <summary>
        /// Lança o projétil em uma direção
        /// </summary>
        /// <param name="dir">Direção de lançamento</param>
        public void Launch(Vector dir)
        {
            RigidBody.SetPosition(new Vector(Owner.RigidBody.CenterPoint.X - RigidBody.Size.X / 2, Owner.RigidBody.CenterPoint.Y + RigidBody.Size.Y / 2));
            RigidBody.UseRotation = true;
            RigidBody.PointAt(dir);
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
                    if (!t.IsPlayer)
                    {
                        t.Stun(3000);
                        t.Damage(damager: this.Owner, quantity: HitDamage);
                    }
                    else
                    {
                        t.Stun(1000);
                        t.Damage(damager: this.Owner, quantity: HitDamage / 10);
                    }
                    //t.RigidBody.SetForce(Vector.Zero);
                    t.RigidBody.AddForce(t.RigidBody.CenterPoint.PointedAt(RigidBody.CenterPoint).Normalized * 1000);
                    //t.RigidBody.SetVelocity(t.RigidBody.CenterPoint.PointedAt(RigidBody.CenterPoint).Normalized * KnockbackMagnitude * 10);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} exploded on {0}", Name, t.Name);
                    Console.ResetColor();
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

            if (type == ProjectileTypes.Buckshot)
                return new HeavyMGProjectile();
            else if (type == ProjectileTypes.Explosive)
                return new Missile();
            else if (type == ProjectileTypes.LessLethal)
                return new PistolProjectile();
            else if (type == ProjectileTypes.Arrow)
                return new RifleProjectile();
            else if (type == ProjectileTypes.HollowPoint)
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
        /// Evento a ser disparado quando o projétil entra em colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            if (e.Collider.IsCamera)
                return;

            if (e.Collider != Owner)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("{0} collided with {1}", Name, e.Collider.Name);
                Console.ResetColor();
            }

            var colliderChar = Character.FromHashCode(e.Collider.Hash);

            if (e.Collider != Owner)
            {
                if (IsExplosive)
                {
                    ExplodeAt(RigidBody.CenterPoint, explosionRadius: 200);
                    Destroy();
                }
                else if (colliderChar != null)
                {
                    if (!colliderChar.IsPlayer)
                        colliderChar.Damage(damager: this.Owner, quantity: HitDamage);
                    else
                        colliderChar.Damage(damager: this.Owner, quantity: HitDamage / 10);
                    colliderChar.RigidBody.AddVelocity(e.CollisionDirection.Opposite.Normalized * KnockbackMagnitude);
                    Destroy();
                }
                else if (e.Collider.Tag == Tags.Projectile)
                {
                    var p = Projectile.FromHashCode(e.Collider.Hash);
                    if (p != null && p.Owner != Owner)
                    {
                        if (p.IsExplosive)
                            p.ExplodeAt(RigidBody.CenterPoint, explosionRadius: 200);
                        if (this.IsExplosive)
                            ExplodeAt(RigidBody.CenterPoint, explosionRadius: 200);
                        p.Destroy();
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
                    colliderChar.RigidBody.AddVelocity(e.Collider.RigidBody.Front.Opposite * 20);
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
        public override void Destroy()
        {
            base.Destroy();
            Projectiles.Remove(this);
        }
        #endregion
    }
}
