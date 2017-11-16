using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ZombieGame.Game
{
    public class Sprite
    {
        public static string UnavailableSprite { get { return IO.GlobalPaths.Sprites + "unavailable.png"; } }
        public static string TransparentSprite { get { return IO.GlobalPaths.Sprites + "transparent.png"; } }
        public static string DebugSprite { get { return IO.GlobalPaths.Sprites + "debug.png"; } }

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
                    return new BitmapImage(new Uri(UnavailableSprite));
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
