using System;

namespace ZombieGame.Game.Serializable
{
    [Serializable]
    public class Score
    {
        public string Name { get; set; }
        public long Highscore { get; set; }
        public DateTime Date { get; set; }

        public Score() { }
    }
}
