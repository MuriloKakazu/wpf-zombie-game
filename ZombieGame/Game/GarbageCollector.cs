using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Entities;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class GarbageCollector
    {
        private static float DeltaT { get; set; }
        private static float ThresholdTimeMs { get; set; }

        public static void Setup()
        {
            DeltaT = 0;
            ThresholdTimeMs = 1000;
            Time.HighFrequencyTimer.Elapsed += Timer_Elapsed;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.InvokeAsync(delegate { IncreaseDeltaT(); });
        }

        private static void IncreaseDeltaT()
        {
            DeltaT += Time.Delta * 1000;

            if (DeltaT >= ThresholdTimeMs)
            {
                DeltaT = 0;
                DestroyAllInactiveEntities();
            }
        }

        public static void DestroyAllInactiveEntities()
        {
            foreach (var e in AnimatedEntity.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Enemy.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Character.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Projectile.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Entity.Instances)
                if (!e.IsActive) e.Destroy();
        }
    }
}
