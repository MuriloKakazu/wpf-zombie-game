using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using ZombieGame.Audio;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Entities;
using ZombieGame.Game.Serializable;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;
using ZombieGame.UI;

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
        /// Retorna a quantidade de dinheiro dos personagens
        /// </summary>
        public static float Money { get; set; }
        public static float Score { get; set; }
        public static float DifficultyBonus
        {
            get
            {
                if (Difficulty == Difficulties.Easy) return 1;
                else if (Difficulty == Difficulties.Medium) return 1.25f;
                else return 1.5f;
            }
        }
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
        public static Wall BottomWall { get; set; }
        public static Wall TopWall { get; set; }
        public static Wall LeftWall { get; set; }
        public static Wall RightWall { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            Time.Setup();
            GarbageCollector.Setup();
            SetupDatabase();
            SetupGameEntities();
            SetupInternalTimer();
            Resume();
            Store.SetSellingItems();
            SetupScene(Database.Scenes[0]);
            Difficulty = Difficulties.Easy;
            UserControls.Setup();
            Pause();
        }

        private static void SetupScene(Scene s)
        {
            Entity.RemoveAllOfType(Tags.Tile);
            if (CurrentScene != null)
                CurrentScene.Background.Destroy();
            CurrentScene = s;
            CurrentScene.Show();

            if (GameMaster.Players.Length > 0)
                GetPlayer(0).Character.RigidBody.SetPosition(s.Player1Spawn);
            if (GameMaster.Players.Length > 1)
                GetPlayer(1).Character.RigidBody.SetPosition(s.Player2Spawn);
        }

        /// <summary>
        /// Define as configurações do banco de dados
        /// </summary>
        private static void SetupDatabase()
        {
            Database.Setup();
        }

        /// <summary>
        /// Define as configurações das entidades iniciais do jogo
        /// </summary>
        private static void SetupGameEntities()
        {
            Camera = new Camera();
            var Player1 = new Player(1, "Player1");
            //var Player2 = new Player(2, "Player2");
            Player1.Character.RigidBody.UseRotation = true;
            //Player2.Character.RigidBody.UseRotation = true;
            Player1.Character.RigidBody.SetPosition(new Physics.Vector(100, 0));
            //Player2.Character.RigidBody.SetPosition(new Physics.Vector(500, 0));
            Player1.Character.RigidBody.Resize(new Physics.Vector(50, 50));
            //Player2.Character.RigidBody.Resize(new Physics.Vector(50, 50));
            Players = new Player[1];
            Players[0] = Player1;
            //Players[1] = Player2;

            BottomWall = new Wall(WallTypes.BottomWall);
            BottomWall.RigidBody.SetPosition(new Physics.Vector(Camera.RigidBody.Position.X, Camera.RigidBody.Position.Y - Camera.RigidBody.Size.Y));
            BottomWall.RigidBody.Resize(new Physics.Vector(Camera.RigidBody.Size.X, 100));
            BottomWall.RigidBody.Freeze();
            TopWall = new Wall(WallTypes.TopWall);
            TopWall.RigidBody.SetPosition(new Physics.Vector(Camera.RigidBody.Position.X, Camera.RigidBody.Position.Y));
            TopWall.RigidBody.Resize(new Physics.Vector(Camera.RigidBody.Size.X, 100));
            TopWall.RigidBody.Freeze();
            LeftWall = new Wall(WallTypes.LeftWall);
            LeftWall.RigidBody.SetPosition(new Physics.Vector(Camera.RigidBody.Position.X, Camera.RigidBody.Position.Y));
            LeftWall.RigidBody.Resize(new Physics.Vector(100, Camera.RigidBody.Size.Y));
            LeftWall.RigidBody.Freeze();
            RightWall = new Wall(WallTypes.RightWall);
            RightWall.RigidBody.SetPosition(new Physics.Vector(Camera.RigidBody.Size.X, 0));
            RightWall.RigidBody.Resize(new Physics.Vector(100, Camera.RigidBody.Size.Y));
            RightWall.RigidBody.Freeze();
        }

        /// <summary>
        /// Define as configurações do timer interno
        /// </summary>
        private static void SetupInternalTimer()
        {
            InternalTimer = new Timer { Interval = 1 };
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
            //ManageBackground();
        }

        public static void HideCursor()
        {
            Mouse.OverrideCursor = Cursors.None;
        }

        public static void ShowCursor()
        {
            Mouse.OverrideCursor = Cursors.Arrow;
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
                        {
                            Resume();
                            HideCursor();
                        }
                        else if (GameplayState == GameplayStates.Running)
                        {
                            Pause();
                            ShowCursor();
                        }

                        if (UserControls.StoreControl.IsOpen)
                        {
                            Application.Current.Windows.OfType<MainWindow>().
                           FirstOrDefault().RemoveVisualComponent(UserControls.StoreControl);
                            UserControls.StoreControl.IsOpen = false;
                        }

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.F1))
                    {


                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.F2))
                    {
                        EnemySpawner.SpawnRandomEnemy();

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.F3))
                    {
                        if (!UserControls.StoreControl.IsOpen)
                        {
                            Application.Current.Windows.OfType<MainWindow>().
                           FirstOrDefault().AddVisualComponent(UserControls.StoreControl);
                            UserControls.StoreControl.IsOpen = true;
                            UserControls.StoreControl.Refresh();
                        }
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
            foreach (var e in AnimatedEntity.AllInstances)
                e.PauseAnimation();
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
            foreach (var e in AnimatedEntity.AllInstances)
                e.ResumeAnimation();
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
            try
            {
                if (Players.Length >= number)
                    return Players[number];
                else return null;
            }
            catch { }
            return null;
        }
        #endregion
    }
}
