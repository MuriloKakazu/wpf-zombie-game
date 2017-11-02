using System;
using System.Timers;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Character : Entity
    {
        #region Properties

        #region Level and Exeperience

        public int Level { get; set; } //The max is 50
        public float CurrentExp { get; set; }
        public float NeededExp { get { return 500 + 10 * (float)Math.Pow(Level, 2); } }
        public float TotalExp { get; set; }
        public float RemainingExp { get { return NeededExp - CurrentExp; } }
        #endregion
        public Weapon Weapon { get; set; }
        public Vector AimDirection { get; set; }

        public Character()
        {
            Weapon = new Weapon();
            AimDirection = Vector.Zero;
            RigidBody.SpeedMultiplier = 1.5f;
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            
        }

        #endregion
    }
}
