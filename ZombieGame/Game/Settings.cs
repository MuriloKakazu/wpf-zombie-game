using ZombieGame.Game.Enums;

namespace ZombieGame.Game
{
    public class Settings
    {
        /// <summary>
        /// Dificuldade do jogo
        /// </summary>
        public Difficulties Difficulty { get; set; }
        public float Volume { get; set; }

        public Settings()
        {

        }
    }
}
