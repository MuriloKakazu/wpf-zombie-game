using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using static ZombieGame.Game.GameMaster;

namespace ZombieGame.Game
{
    public class Zombie : Character
    {
        /// <summary>
        /// Retorna o tipo de zumbi
        /// </summary>
        public ZombieTag ZbTag { get; set; }
        public bool IsAZombie { get { return Tag == Tags.Enemy; } }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="zbTag">O tipo de zumbi</param>
        public Zombie(ZombieTag zbTag) : base(zbTag.ToString(), Tags.Enemy)
        {
            ZbTag = zbTag;
            SetMoney();
            SetHealth();
            SetExperience();
        }

        /// <summary>
        /// O método para definir quando de dinheiro o zumbi dropará.
        /// <para>Se trata de um valor definido entre um range</para>
        /// </summary>
        private void SetMoney()
        {
            int maxMoney = default(int), minMoney = default(int), avgLvls;
            if (Player2.IsPlaying)
                avgLvls = (Player1.Character.Level + Player2.Character.Level) / 2;
            else
                avgLvls = Player1.Character.Level;

            switch (ZbTag)
            {
                case ZombieTag.Normal:
                    maxMoney = 20 * avgLvls + 10 * Level;
                    minMoney = 5 * avgLvls + 10 * Level;
                    break;
                case ZombieTag.Runner:
                    maxMoney = 50 * avgLvls + 20 * Level;
                    minMoney = 20 * avgLvls + 15 * Level;
                    break;
                case ZombieTag.Tank:
                    maxMoney = 100 * avgLvls + 30 * Level;
                    minMoney = 40 * avgLvls + 20 * Level;
                    break;
                case ZombieTag.Boss:
                    maxMoney = 1000 * avgLvls + 300 * Level;
                    minMoney = 400 * avgLvls + 200 * Level;
                    break;
            }

            Money = new Random().Next(minMoney, maxMoney);
        }

        /// <summary>
        /// O método para definir a vida do zumbi.
        /// </summary>
        private void SetHealth()
        {
            switch (ZbTag)
            {
                case ZombieTag.Normal:
                    Health = 50 * Level;
                    break;
                case ZombieTag.Runner:
                    Health = 30 * Level;
                    break;
                case ZombieTag.Tank:
                    Health = 100 * Level + 100;
                    break;
                case ZombieTag.Boss:
                    Health = 400 * Level + 200;
                    break;
            }
        }

        /// <summary>
        /// O método para definir a experiência que o zumbi dará ao morrer.
        /// </summary>
        private void SetExperience()
        {
            switch (ZbTag)
            {
                case ZombieTag.Normal:
                    Experience = 25 * Level;
                    break;
                case ZombieTag.Runner:
                    Experience = 35 * Level;
                    break;
                case ZombieTag.Tank:
                    Experience = 40 * Level;
                    break;
                case ZombieTag.Boss:
                    Experience = 20 * Level + 10 * Level * Level;
                    break;
            }
        }

        /// <summary>
        /// Método chamado quando se mata um zumbi
        /// </summary>
        /// <param name="killer">O personagem que matou o zumbi</param>
        public void Kill(Character killer)
        {
            if(killer.IsAPlayer)
            {
                killer.Money += Money;
                killer.Experience += Experience;

                Destroy();
            }
        }

        /// <summary>
        /// Método chamado para dar dano ao zumbi
        /// </summary>
        /// <param name="attacker">O personagem que atacou o zumbi</param>
        public void DoDamage(Character attacker)
        {
            if(attacker.IsAPlayer)
            {
                if (Health - attacker.Weapon.Damage <= 0)
                    Kill(attacker);
                else
                    Health -= attacker.Weapon.Damage;
            }
        }
    }
}
