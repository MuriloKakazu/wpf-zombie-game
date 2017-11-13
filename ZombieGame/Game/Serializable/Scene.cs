using System;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Prefabs.Entities;
using ZombieGame.Physics;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Scene
    {
        #region Properties
        /// <summary>
        /// Nome do cenário
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Imagem de fundo do cenário
        /// </summary>
        public Background Background { get; set; }
        /// <summary>
        /// Entidades do mapa
        /// </summary>
        public SimpleTile[] Tiles { get; set; }
        /// <summary>
        /// Posição de spawn do Jogador 1
        /// </summary>
        public Vector Player1Spawn { get; set; }
        /// <summary>
        /// Posição de spawn do Jogador 2
        /// </summary>
        public Vector Player2Spawn { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        public Scene() { }

        public void Show()
        {
            Background.Show();
            foreach (var e in Tiles)
                e.Mount().Show();
        }

        /// <summary>
        /// Cria uma explosão no cenário
        /// </summary>
        /// <param name="pos">Posição da explosão</param>
        /// <param name="radius">Raio da explosão</param>
        /// <param name="applyPhysics">Retorna se será necessário aplicar Física à explosão</param>
        public void SpawnExplosionAt(Vector pos, float radius, bool applyPhysics)
        {
            var explosion = new Explosion(new Vector(pos.X - radius / 2, pos.Y + radius / 2), new Vector(radius, radius), 750);
            if (applyPhysics)
            {
                var targets = Character.GetNearbyCharacters(pos, radius);
                if (targets != null)
                {
                    foreach (var t in targets)
                    {
                        t.Stun(2000);
                        t.Damage(damager: explosion, quantity: 10);
                        //t.RigidBody.PointAt(pos.Normalized);
                        t.RigidBody.AddForce(t.RigidBody.CenterPoint.PointedAt(pos).Normalized * (radius * 5));
                    }
                }
            }
            explosion.Show();
        }
        #endregion
    }
}
