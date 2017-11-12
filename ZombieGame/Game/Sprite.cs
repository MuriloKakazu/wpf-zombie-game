using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ZombieGame.Game
{
    public class Sprite
    {
        private static BitmapSource UnavailableSprite {  get { return new BitmapImage(new Uri(IO.GlobalPaths.Sprites + "unavailable.png")); } }

        /// <summary>
        /// Caminho do arquivo
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Componente visual cuja imagem tem como fonte o caminho do arquivo
        /// </summary>
        public BitmapSource Image
        {
            get
            {
                if (File.Exists(Uri))
                    return new BitmapImage(new Uri(Uri));
                else
                    return UnavailableSprite;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Sprite()
        {
            Uri = IO.GlobalPaths.Sprites + "unavailable.png";
        }

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
