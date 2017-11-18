using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ZombieGame.Game.Prefabs.Sprites;

namespace ZombieGame.Game
{
    public class Sprite
    {
        /// <summary>
        /// Caminho do arquivo
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Componente visual cuja imagem tem como fonte o caminho do arquivo
        /// </summary>
        public virtual BitmapSource Image
        {
            get
            {
                try
                {
                    if (File.Exists(Uri))
                        return new BitmapImage(new Uri(Uri));
                    else
                        return new UnavailableSprite().Image;
                }
                catch // File is not an image?
                {
                    return new UnavailableSprite().Image;
                }
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Sprite() { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uri">Caminho do arquivo</param>
        public Sprite(string uri)
        {
            Uri = uri;
        }
    }
}
