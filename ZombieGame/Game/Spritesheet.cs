using System.Collections.Generic;
using System.IO;

namespace ZombieGame.Game
{
    public class Spritesheet
    {
        /// <summary>
        /// Sprites da animação
        /// </summary>
        public Sprite[] Sprites { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        public Spritesheet() { }

        /// <summary>
        /// Carrega as sprites de animação a partir de um diretório
        /// </summary>
        /// <param name="path">Diretório</param>
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
