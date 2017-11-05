using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ZombieGame.Game.Prefabs.DataBase;
using ZombieGame.IO.Serialization;
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
        /// Retorna o cenário atual do jogo
        /// </summary>
        public static Scene CurrentScene { get; private set; }
        /// <summary>
        /// Retorna todos os cenários de jogo disponíveis
        /// </summary>
        public static Scene[] Scenes { get; private set; }
        /// <summary>
        /// Retorna os dados de todos os projéteis
        /// </summary>
        public static ProjectilesDB PDB { get; private set; }

        /// <summary>
        /// Retorna os dados de todas as armas
        /// </summary>
        public static WeaponsDB WDB { get; private set; }

        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            PDB = PDB.LoadFrom(IO.GlobalPaths.DB + "Projectiles.xlm");
            WDB = WDB.LoadFrom(IO.GlobalPaths.DB + "Weapons.xlm");
            UpdateTimer = new Timer();
            UpdateTimer.Interval = 1;
            UpdateTimer.Enabled = true;
            Time.ListenToTimer(UpdateTimer);
            Player1 = new Player(1, "Player1");
            Player2 = new Player(2, "Player2");
            Player1.Character.RigidBody.UseRotation = true;
            Player2.Character.RigidBody.UseRotation = true;
            Player1.Character.RigidBody.SetPosition(new Vector(100, 0));
            Player2.Character.RigidBody.SetPosition(new Vector(500, 0));
            Player1.Character.RigidBody.Resize(new Vector(50, 50));
            Player2.Character.RigidBody.Resize(new Vector(50, 50));
            

         
            /**ProjectilesDB p = new ProjectilesDB();
            WeaponsDB w = new WeaponsDB();

            p.HMGdmg = 7;
            p.HMGspd = 450;
            w.HMGfr = 700;
            w.HMGammo = 150;
            w.HMGrt = 5.4f;

            p.missileDmg = 100;
            p.missileSpd = 250;
            w.missileFR = 30;
            w.missileAmmo = 1;
            w.missileRT = 4.7f;

            p.pistolDmg = 5;
            p.pistolSpd = 300;
            w.pistolFR = 300;
            w.pistolAmmo = 15;
            w.pistolRT = 1.2f;

            p.rifleDmg = 10;
            p.rifleSpd = 450;
            w.rifleFR = 800;
            w.rifleAmmo = 30;
            w.rifleRT = 2.4f;

            p.sniperDmg = 50;
            p.sniperSpd = 1500;
            w.sniperFR = 45;
            w.sniperAmmo = 5;
            w.sniperRT = 4.5f;

            p.SaveTo(IO.GlobalPaths.DB + "Projectiles.xlm");
            w.SaveTo(IO.GlobalPaths.DB + "Weapons.xlm");*/
        }
    }
}
