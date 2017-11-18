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
        public Vector RenderPosition { get; set; }
        public Vector Size { get; set; }
        /// <summary>
        /// Imagem de fundo do cenário
        /// </summary>
        public Background Background { get; set; }
        public Foreground Foreground { get; set; }
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
            Foreground.Show();
            foreach (var e in Tiles)
                e.Mount().Show();
        }
        #endregion
    }
}
