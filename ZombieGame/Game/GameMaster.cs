using System;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Entities;
using ZombieGame.Game.Serializable;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class GameMaster
    {
        #region Properties
        /// <summary>
        /// Timer interno do jogo (não relacionado aos cálculos de Física)
        /// </summary>
        private static Timer InternalTimer { get; set; }
        /// <summary>
        /// Última decorrência do timer interno
        /// </summary>
        private static DateTime LastUpdate { get; set; }
        /// <summary>
        /// Tempo decorrido ignorando a entrada de usuário, em milisegundos
        /// </summary>
        private static float IgnoringKeyPressTimeMs { get; set; }
        /// <summary>
        /// Returna se o programa estará ignorando a entrada de usuário
        /// </summary>
        private static bool IgnoreKeyPress { get; set; }
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
        /// <summary>
        /// Estado do jogo
        /// </summary>
        public static GameplayStates GameplayState { get; private set; }
        /// <summary>
        /// Dificuldade do jogo
        /// </summary>
        public static Difficulties Difficulty { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            Time.Setup();
            SetupInternalTimer();
            Resume();
            SetupDatabase();
            Store.SetSellingItems();
            SetupGameEntities();
            SetupScene(new Scene());
            Pause();
        }

        private static void SetupScene(Scene s)
        {
            CurrentScene = s;
        }

        /// <summary>
        /// Define as configurações do banco de dados
        /// </summary>
        private static void SetupDatabase()
        {
            Database.Weapons = Database.Weapons.LoadFrom(IO.GlobalPaths.DB + "weapons.db");
            Database.Projectiles = Database.Projectiles.LoadFrom(IO.GlobalPaths.DB + "projectiles.db");
        }

        /// <summary>
        /// Define as configurações das entidades iniciais do jogo
        /// </summary>
        private static void SetupGameEntities()
        {
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
        /// Define as configurações do timer interno
        /// </summary>
        private static void SetupInternalTimer()
        {
            InternalTimer = new Timer();
            InternalTimer.Interval = 1;
            InternalTimer.Elapsed += InternalTimer_Elapsed;
            InternalTimer.Start();
        }

        /// <summary>
        /// Evento disparado quando o timer interno é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações do evento</param>
        private static void InternalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckForUserInput();
        }

        /// <summary>
        /// Verifica a entrada de usuário
        /// </summary>
        private static void CheckForUserInput()
        {
            if (!IgnoreKeyPress)
            {
                App.Current.Dispatcher.Invoke(delegate
                {
                    if (Keyboard.IsKeyDown(Key.Escape))
                    {
                        if (GameplayState == GameplayStates.Paused)
                            Resume();
                        else if (GameplayState == GameplayStates.Running)
                            Pause();

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.F1))
                    {
                        Console.WriteLine("F2: spawn enemies");
                        Console.WriteLine("F3: kill all enemies");

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.F2))
                    {
                        //if (Enemy.GetAllActiveEnemies().Length < 20)
                        //for (int i = 0; i < 10; i++)
                        //    EnemySpawner.SpawnZombie();

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.F3))
                    {
                        foreach (var ae in Enemy.GetAllActiveEnemies())
                            ae.Kill(killer: GameMaster.GetPlayer(1).Character);

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                });
            }
            else
            {
                float dt = (DateTime.Now.Millisecond - LastUpdate.Millisecond);
                if (dt > 0)
                    IgnoringKeyPressTimeMs += DateTime.Now.Millisecond - LastUpdate.Millisecond;
                else
                    IgnoringKeyPressTimeMs += 16f; // Average update time
                LastUpdate = DateTime.Now;

                if (IgnoringKeyPressTimeMs >= 200)
                {
                    IgnoringKeyPressTimeMs = 0;
                    IgnoreKeyPress = false;
                }
            }
        }

        /// <summary>
        /// Cessa os cálculos de Física do jogo
        /// </summary>
        public static void Pause()
        {
            Time.Pause();
            GameplayState = GameplayStates.Paused;
            App.Current.Windows.OfType<MainWindow>().FirstOrDefault().SetCameraOpacity(0.5f);
            Console.WriteLine("Game paused");
        }

        /// <summary>
        /// Resume os cálculos de Física do jogo
        /// </summary>
        public static void Resume()
        {
            Time.Resume();
            GameplayState = GameplayStates.Running;
            App.Current.Windows.OfType<MainWindow>().FirstOrDefault().SetCameraOpacity(1f);

            Console.WriteLine("Game resumed");
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
        #endregion
    }
}
