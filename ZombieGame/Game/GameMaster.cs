using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using ZombieGame.Game.Entities;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Entities;
using ZombieGame.Game.Serializable;
using ZombieGame.IO;
using ZombieGame.IO.Serialization;
using ZombieGame.Physics;
using ZombieGame.UI;

namespace ZombieGame.Game
{
    public static class GameMaster
    {
        #region Properties
        public static bool Started { get; set; }
        public static GameCanvas TargetCanvas { get; set; }
        public static MainWindow TargetWindow { get; set; }
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
        public static float RunningTime { get; set; }
        public static float DifficultyBonus
        {
            get
            {
                if (Settings.Difficulty == Difficulty.Easy) return 1;
                else if (Settings.Difficulty == Difficulty.Medium) return 1.5f;
                else return 2f;
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
        public static Settings Settings { get; private set; }
        /// <summary>
        /// Estado do jogo
        /// </summary>
        public static ExecutionState GameplayState { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Define as configurações iniciais do jogo
        /// </summary>
        public static void Setup()
        {
            Started = true;
            Time.Setup();
            GarbageCollector.Setup();
            SetupGameEntities();
            SetupInternalTimer();
            Resume();
            EnemySpawner.Setup();
            Store.SetSellingItems();
            SetupScene(Database.Scenes[0]);
            Money = 0;
            UserControls.Setup();
            Pause();
        }

        public static void LoadSettings()
        {
            Settings = Settings.LoadFrom("settings.config");
        }

        private static void SetupScene(Scene s)
        {
            Entity.RemoveAllOfType(Tag.Tile);
            if (CurrentScene != null)
                CurrentScene.Background.Destroy();
            CurrentScene = s;
            CurrentScene.Show();

            if (GameMaster.GetPlayer(0).IsPlaying)
                GetPlayer(0).Character.RigidBody.SetPosition(s.Player1Spawn);
            if (GameMaster.GetPlayer(1).IsPlaying)
                GetPlayer(1).Character.RigidBody.SetPosition(s.Player2Spawn);
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
            Player1.Character.RigidBody.SetPosition(new Physics.Vector(100, 0));
            Player2.Character.RigidBody.SetPosition(new Physics.Vector(500, 0));
            Player1.Character.RigidBody.Resize(new Physics.Vector(50, 50));
            Player2.Character.RigidBody.Resize(new Physics.Vector(50, 50));
            Players = new Player[2];
            Players[0] = Player1;
            Players[1] = Player2;
            Players[0].IsPlaying = true;
            Players[1].IsPlaying = false;
            Players[1].Character.MarkAsNoLongerNeeded();
        }

        /// <summary>
        /// Define as configurações do timer interno
        /// </summary>
        private static void SetupInternalTimer()
        {
            InternalTimer = new Timer { Interval = 1 };
            InternalTimer.Elapsed += InternalTimer_Elapsed;
            InternalTimer.Start();
            Time.HighFrequencyTimer.Elapsed += HighFrequencyTimer_Elapsed;
        }

        private static void HighFrequencyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { IncreaseRunningTime(); IncreaseScore(); });
        }

        private static void IncreaseScore()
        {
            Score += 1 * Time.Delta * DifficultyBonus;
        }

        /// <summary>
        /// Evento disparado quando o timer interno é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações do evento</param>
        private static void InternalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { CheckForUserInput(); });
        }

        private static void IncreaseRunningTime()
        {
            RunningTime += Time.Delta;
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
                    if (Keyboard.IsKeyDown(Key.Escape))
                    {
                        if (GameplayState == ExecutionState.Paused)
                        {
                            Resume();
                            HideCursor();
                        }
                        else if (GameplayState == ExecutionState.Running)
                        {
                            Pause();
                            ShowCursor();
                        }

                        if (UserControls.StoreControl.IsOpen)
                        {
                            GameMaster.TargetCanvas.RemoveChild(UserControls.StoreControl);
                            UserControls.StoreControl.IsOpen = false;
                        }

                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
                    else if (Keyboard.IsKeyDown(Key.Enter))
                    {
                        if (GameplayState == ExecutionState.Running && !GetPlayer(1).IsPlaying)
                        {
                            GetPlayer(1).IsPlaying = true;
                            GetPlayer(1).Character = new Character("Player2", Tag.Player);
                            GetPlayer(1).Character.MaxHealth = 100;
                            GetPlayer(1).Character.SetHealth(100);
                            GetPlayer(1).Character.LoadSprite(GlobalPaths.CharacterSprites + "player2.png");
                            GetPlayer(1).Character.RigidBody.SetPosition(GetPlayer(0).Character.RigidBody.Position);
                            GetPlayer(1).Character.RigidBody.Resize(new Physics.Vector(50, 50));
                            GetPlayer(1).Character.RigidBody.UseRotation = true;
                        }
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
                            GameMaster.TargetCanvas.AddChild(UserControls.StoreControl);
                            UserControls.StoreControl.IsOpen = true;
                            UserControls.StoreControl.Refresh();
                        }
                        IgnoreKeyPress = true;
                        LastUpdate = DateTime.Now;
                    }
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
            foreach (var e in AnimatedEntity.Instances)
                e.PauseAnimation();
            GameplayState = ExecutionState.Paused;
            Console.WriteLine("Game paused");
        }

        /// <summary>
        /// Resume os cálculos de Física do jogo
        /// </summary>
        public static void Resume()
        {
            Time.Resume();
            foreach (var e in AnimatedEntity.Instances)
                e.ResumeAnimation();
            GameplayState = ExecutionState.Running;

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
