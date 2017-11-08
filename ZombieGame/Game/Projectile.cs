using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game
{
    [Serializable]
    public class Projectile : Entity
    {
        #region Properties
        /// <summary>
        /// Lista de todos os projéteis ativos
        /// </summary>
        private static List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// Retorna se o projétil atordoa ao impacto
        /// </summary>
        public bool IsStunner { get; set; }
        /// <summary>
        /// Retorna se o projétil explode com o impacto
        /// </summary>
        public bool IsExplosive { get; set; }
        /// <summary>
        /// Dano de impacto do projétil
        /// </summary>
        public int HitDamage { get; set; }
        /// <summary>
        /// Entidade que invocou o projétil
        /// </summary>
        [XmlIgnore]
        public Character Owner { get; set; }
        /// <summary>
        /// Tipo do projétil
        /// </summary>
        public ProjectileTypes Type { get; set; }
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
            set
            {
                if (value < 25)
                    speedMagnitude = 25;
                else if (value > 150)
                    speedMagnitude = 150;
                else
                    speedMagnitude = value;
            }
        }
        /// <summary>
        /// Coeficiente de ação do projétil sobre um corpo
        /// </summary>
        public float KnockbackMagnitude { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todos os projéteis ativos
        /// </summary>
        /// <returns></returns>
        public static Projectile[] GetAllActiveProjectiles()
        {
            return Projectiles.ToArray();
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
        /// ctor
        /// </summary>
        public Projectile() : base("Projectile", Tags.Projectile)
        {
            Projectiles.Add(this);
        }

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
            Show();
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

                    t.RigidBody.AddForce(t.RigidBody.CenterPoint.PointedAt(RigidBody.CenterPoint).Normalized * 1000);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} exploded on {0}", Name, t.Name);
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Destrói o projétil para liberar espaço na memória
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            Projectiles.Remove(this);
        }

        /// <summary>
        /// Retorna um clone profundo da instância atual
        /// </summary>
        /// <returns>Weapon</returns>
        public Projectile Clone()
        {
            var copy = new Projectile()
            {
                Hash = Guid.NewGuid(),
                HitDamage = HitDamage,
                IsExplosive = IsExplosive,
                Collisions = Collisions,
                IsStunner = IsStunner,
                KnockbackMagnitude = KnockbackMagnitude,
                Name = Name,
                Owner = Owner,
                RigidBody = new RigidBody() { Size = RigidBody.Size },
                SpeedMagnitude = SpeedMagnitude,
                Sprite = Sprite,
                Tag = Tag,
                Type = Type,
                Visible = Visible,
                VisualControl = new Controls.VisualControl()
            };
            if (copy.Visible)
            {
                System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().AddVisualComponent(VisualControl);
                UpdateVisualControl();
            }
            return copy;
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
                if (colliderChar != null)
                {
                    if (!colliderChar.IsPlayer)
                        colliderChar.Damage(damager: this.Owner, quantity: HitDamage);
                    else
                        colliderChar.Damage(damager: this.Owner, quantity: HitDamage / 10);
                    colliderChar.RigidBody.AddVelocity(e.CollisionDirection.Opposite.Normalized * KnockbackMagnitude);
                    if (IsExplosive)
                        ExplodeAt(RigidBody.CenterPoint, explosionRadius: 200);
                    if (IsStunner)
                        colliderChar.Stun(1000);
                    Destroy();
                }
                else if (e.Collider.Tag == Tags.Projectile)
                {
                    var p = Projectile.FromHashCode(e.Collider.Hash);
                    if (p != null && p.Owner != Owner)
                    {
                        if (p.IsExplosive)
                            p.ExplodeAt(p.RigidBody.CenterPoint, explosionRadius: 200);
                        if (IsExplosive)
                            ExplodeAt(RigidBody.CenterPoint, explosionRadius: 200);
                        p.Destroy();
                        Destroy();
                    }
                }
                else
                {
                    if (IsExplosive)
                        ExplodeAt(RigidBody.CenterPoint, explosionRadius: 200);
                    Destroy();
                }
            }
            else
            {
                if (colliderChar != null)
                {
                    colliderChar.RigidBody.AddVelocity(e.Collider.RigidBody.Front.Opposite * 10);
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
            // Override para garantir que nada seja feito
        }

        /// <summary>
        /// Evento a ser disparado quando o projétil sai de colisão com outro objeto
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações da colisão</param>
        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            // Override para garantir que nada seja feito
        }
        #endregion
    }
}
