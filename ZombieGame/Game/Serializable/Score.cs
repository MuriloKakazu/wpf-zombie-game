using System;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Score
    {
        public string Name { get; set; }
        public float Highscore { get; set; }
        public DateTime Date { get; set; }

        public Score() { }
    }
}
