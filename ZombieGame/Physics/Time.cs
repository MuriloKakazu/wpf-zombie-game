using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game;

namespace ZombieGame.Physics
{
    public static class Time
    {
        public static float Delta { get; private set; }
        private static DateTime LastUpdate { get; set; }

        public static void Setup()
        {
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private static void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            float d = DateTime.Now.Millisecond - LastUpdate.Millisecond;
            if (d > 0)
                Delta = d / 1000;
            LastUpdate = DateTime.Now;
        }
    }
}
