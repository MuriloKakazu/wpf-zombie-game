using System.Collections.Generic;
using System.IO;

namespace ZombieGame.Game
{
    public class Spritesheet
    {
        public Sprite[] Sprites { get; protected set; }

        public Spritesheet()
        {

        }

        public void LoadFrom(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                List<Sprite> sprites = new List<Sprite>();
                
                foreach(var f in files)
                    sprites.Add(new Sprite() { Uri = f });

                Sprites = sprites.ToArray();
            }
            else
                throw new DirectoryNotFoundException();
        }
    }
}
