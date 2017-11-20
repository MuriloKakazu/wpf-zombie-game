using System;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Dificuldade do jogo
        /// </summary>
        public Difficulty Difficulty { get; set; }
        public float Volume { get; set; }
        public bool AntiAliasingEnabled { get; set; }
        public int RenderScale { get; set; }

        public Settings() { }
    }
}
