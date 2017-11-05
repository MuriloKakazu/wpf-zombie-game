using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZombieGame.Game.Controls;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Scene
    {
        /// <summary>
        /// Nome do cenário
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Imagem de fundo do cenário
        /// </summary>
        public Image Background { get; set; }
        /// <summary>
        /// Objetos (enfeites) do mapa
        /// </summary>
        public Entity Tiles { get; set; }
        /// <summary>
        /// Posição de spawn do Jogador 1
        /// </summary>
        public Vector Player1Spawn { get; set; }
        /// <summary>
        /// Posição de spawn do Jogador 2
        /// </summary>
        public Vector Player2Spawn { get; set; }
        /// <summary>
        /// Componente visual do cenário
        /// </summary>
        protected VisualControl VisualControl { get; set; }

        public Scene()
        {
            VisualControl = new VisualControl();
        }

        public void Setup()
        {
            // Add children
        }
    }
}
