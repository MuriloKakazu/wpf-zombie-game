using ZombieGame.Game.Prefabs.Entities;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class GameMaster
    {
        #region Properties
        /// <summary>
        /// Câmera do jogo
        /// </summary>
        public static Camera Camera { get; private set; }
        /// <summary>
        /// Jogador 1
        /// </summary>
        public static Player[] Players { get; private set; }
        /// <summary>
        /// Retorna o cenário atual do jogo
        /// </summary>
        public static Scene CurrentScene { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            Time.Setup();
            Database.Weapons = Database.Weapons.LoadFrom(IO.GlobalPaths.DB + "weapons.db");
            Database.Projectiles = Database.Projectiles.LoadFrom(IO.GlobalPaths.DB + "projectiles.db");

            FixSpritePaths();
            Store.SetSellingItems();
            Camera = new Camera();
            var Player1 = new Player(1, "Player1");
            var Player2 = new Player(2, "Player2");
            Player1.Character.RigidBody.UseRotation = true;
            Player2.Character.RigidBody.UseRotation = true;
            Player1.Character.RigidBody.SetPosition(new Vector(100, 0));
            Player2.Character.RigidBody.SetPosition(new Vector(500, 0));
            Player1.Character.RigidBody.Resize(new Vector(50, 50));
            Player2.Character.RigidBody.Resize(new Vector(50, 50));
            Players = new Player[2];
            Players[0] = Player1;
            Players[1] = Player2;
        }

        /// <summary>
        /// Retorna a instância de um jogador
        /// </summary>
        /// <param name="number">Número do jogador</param>
        /// <returns>Player</returns>
        public static Player GetPlayer(int number)
        {
            if (Players.Length >= number)
                return Players[number + 1];
            else return null;
        }

        /// <summary>
        /// Adiciona o caminho de execução da aplicação aos caminhos dos recursos serializados
        /// </summary>
        private static void FixSpritePaths()
        {
            foreach (var p in Database.Projectiles)
                p.Sprite.Uri = IO.GlobalPaths.ProjectileSprites + p.Sprite.Uri;
        }
        #endregion
    }
}
