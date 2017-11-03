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
            Character.IsSprinting = Convert.ToBoolean(Input.GetAxis(AxisTypes.Sprint, PlayerNumber));
            Character.RigidBody.AddForce(new Vector(Input.GetAxis(AxisTypes.Horizontal, PlayerNumber)) * 100);
            Character.RigidBody.AddForce(new Vector(0, Input.GetAxis(AxisTypes.Vertical, PlayerNumber)) * 100);
        }
    }
}
