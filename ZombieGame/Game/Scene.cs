using System;
using ZombieGame.Physics;

namespace ZombieGame.Game
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
        public Sprite Background { get; set; }
        /// <summary>
        /// Objetos (enfeites) do mapa
        /// </summary>
        public Entity[] Tiles { get; set; }
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
        public Scene()
        {
            
        }
        #endregion
    }
}
