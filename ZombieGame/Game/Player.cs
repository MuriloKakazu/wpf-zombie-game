using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public class Player
    {
        public int Number { get; set; }
        public Character Character { get; set; }

        public Player()
        {
            Character = new Character();
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            if (Character.RigidBody.Velocity.Magnitude < Character.RigidBody.MaxSpeed)
            Character.RigidBody.AddForce(new Vector(Input.GetAxis(AxisTypes.Horizontal, Number), 0, 0));
            //if (Character.IsGrounded)
                Character.RigidBody.AddForce(new Vector(0, Input.GetAxis(AxisTypes.Vertical, Number), 0));
            var vector = Character.RigidBody.Velocity;
        }
    }
}
