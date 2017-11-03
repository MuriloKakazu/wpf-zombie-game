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
        public int PlayerNumber { get; set; }
        public Character Character { get; set; }

        public Player(int playerNumber)
        {
            PlayerNumber = playerNumber;
            Character = new Character(string.Format("Player{0}", playerNumber.ToString()));
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            Character.RigidBody.Stop();
            Character.IsSprinting = Convert.ToBoolean(Input.GetAxis(AxisTypes.Sprint, PlayerNumber));
            if (Character.IsSprinting)
            {
                Character.RigidBody.AddVelocity(new Vector(Input.GetAxis(AxisTypes.Horizontal, PlayerNumber)) * 500);
                Character.RigidBody.AddVelocity(new Vector(0, Input.GetAxis(AxisTypes.Vertical, PlayerNumber)) * 500);
            }
            else
            {
                Character.RigidBody.AddVelocity(new Vector(Input.GetAxis(AxisTypes.Horizontal, PlayerNumber)) * 250);
                Character.RigidBody.AddVelocity(new Vector(0, Input.GetAxis(AxisTypes.Vertical, PlayerNumber)) * 250);
            }
        }
    }
}
