using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class GameMaster
    {
        /// <summary>
        /// Timer de atualização da Física
        /// </summary>
        public static Timer UpdateTimer { get; private set; }
        /// <summary>
        /// Jogador 1
        /// </summary>
        public static Player Player1 { get; private set; }
        /// <summary>
        /// Jogador 2
        /// </summary>
        public static Player Player2 { get; private set; }

        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            UpdateTimer = new Timer();
            UpdateTimer.Interval = 1;
            UpdateTimer.Enabled = true;
            Time.ListenToTimer(UpdateTimer);
            Player1 = new Player(1);
            Player2 = new Player(2);
            Player1.Character.RigidBody.Resize(new Vector(50, 50));
            Player2.Character.RigidBody.Resize(new Vector(50, 50));
        }
    }
}
