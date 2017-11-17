using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Entities;

namespace ZombieGame.Game
{
    public static class GarbageCollector
    {
        public static Timer InternalTimer { get; private set; }

        public static void Setup()
        {
            InternalTimer = new Timer();
            InternalTimer.Elapsed += InternalTimer_Elapsed;
            InternalTimer.Interval = 1000;
            InternalTimer.Start();
        }

        private static void InternalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
           DestroyAllInactiveEntities();
        }

        public static async void DestroyAllInactiveEntities()
        {
            await Task.Run(delegate
            {
                foreach (var e in AnimatedEntity.AllInstances)
                    if (!e.IsActive) e.Destroy();

                foreach (var e in Enemy.AllInstances)
                    if (!e.IsActive) e.Destroy();

                foreach (var e in Character.AllInstances)
                    if (!e.IsActive) e.Destroy();

                foreach (var e in Projectile.AllInstances)
                    if (!e.IsActive) e.Destroy();

                foreach (var e in Entity.AllInstances)
                    if (!e.IsActive) e.Destroy();
            });
        }
    }
}
