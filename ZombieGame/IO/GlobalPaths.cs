using System;

namespace ZombieGame.IO
{
    public static class GlobalPaths
    {
        /// <summary>
        /// Diretório de execução da aplicação
        /// </summary>
        public static string BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        /// <summary>
        /// Diretório de arquivos de registro
        /// </summary>
        public static string Logs { get { return BaseDirectory + "logs/"; } }
        /// <summary>
        /// Diretório de pontuações salvas
        /// </summary>
        public static string Scores { get { return BaseDirectory + "scores/"; } }
        /// <summary>
        /// Diretório de recursos da aplicação
        /// </summary>
        public static string Resources { get { return BaseDirectory + "resources/"; } }
        /// <summary>
        /// Diretório de recursos de áudio
        /// </summary>
        public static string Audio { get { return Resources + "audio/"; } }
        /// <summary>
        /// Diretório de cenários do jogo
        /// </summary>
        public static string Scenes { get { return Resources + "scenes/"; } }
        /// <summary>
        /// Diretório de sprites do jogo
        /// </summary>
        public static string Sprites { get { return Resources + "sprites/"; } }
        /// <summary>
        /// Diretório de dados das armas do jogo
        /// </summary>
        public static string DB { get { return Resources + "db/"; } }
        /// <summary>
        /// Diretório de sprites de personagens
        /// </summary>
        public static string CharacterSprites { get { return Sprites + "characters/"; } }
        /// <summary>
        /// Diretório de sprites de armas
        /// </summary>
        public static string WeaponSprites { get { return Sprites + "weapons/"; } }
        /// <summary>
        /// Diretório de sprites de projéteis
        /// </summary>
        public static string ProjectileSprites { get { return Sprites + "projectiles/"; } }
        /// <summary>
        /// Diretório de sprites de inimigos
        /// </summary>
        public static string EnemySprites { get { return Sprites + "enemies/"; } }
        /// <summary>
        /// Diretório de sprites de animação
        /// </summary>
        public static string AnimatedSprites { get { return Sprites + "animated/"; } }
    }
}
