using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ZombieGame.Game
{
    public static class GameMaster
    {
        public static Timer UpdateTimer { get; private set; }
        public static Player Player1 { get; private set; }
        public static Player Player2 { get; private set; }

        public static void Setup()
        {
            UpdateTimer = new Timer();
            UpdateTimer.Interval = 1;
            UpdateTimer.Enabled = true;
            Time.Setup();
            Player1 = new Player() { Number = 1 };
            //Player2 = new Player() { Number = 2 };

            Entity ground = new Entity() { Tag = Enums.Tags.Ground, RigidBody = new Physics.RigidBody() { Bounds = new Physics.Bounds(new Physics.Vector(0, 0), new Physics.Vector(10000, 10)) } };
        }
    }
}
