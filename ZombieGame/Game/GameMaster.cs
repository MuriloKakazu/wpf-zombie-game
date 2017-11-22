using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        /// <summary>
        /// Retorna se o jogo acabou
        /// </summary>
        public static bool GameOver { get; private set; }
        /// <summary>
        /// Retorna se o jogo iniciou
        /// </summary>
        public static bool Started { get; set; }
        /// <summary>
        /// Canvas alvo onde serão desenhadas as entidades
        /// </summary>
        public static GameCanvas TargetCanvas { get; set; }
        /// <summary>
        /// Janela alvo da aplicação
        /// </summary>
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
        /// <summary>
        /// Pontuãção dos jogadores
        /// </summary>
        public static float Score { get; set; }
        /// <summary>
        /// Tempo em que a partida está ativa
        /// </summary>
        public static float RunningTime { get; set; }
        /// <summary>
        /// Bônus de dificuldade de jogo
        /// </summary>
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
        /// <summary>
        /// Configurações do jogo
        /// </summary>
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
            Database.Setup();
            GameOver = false;
            Started = true;
            ControlCache.Setup();
            Time.Setup();
            ResourceManager.Setup();
            SetupGameEntities();
            SetupInternalTimer();
            Resume();
            EnemySpawner.Setup();
            Store.SetSellingItems();
            SetupScene(Database.Scenes[0]);
            Money = 0;
            Score = 0;
            RunningTime = 0;
            Pause();
            TargetCanvas.Reset();
            Camera.ForceUpdate();
            ControlCache.PauseMenu.Refresh();
            GameMaster.TargetCanvas.AddChild(ControlCache.PauseMenu);
            ControlCache.PauseMenu.Refresh();
            ShowCursor();
            ShowStartScreen();
        }

        /// <summary>
        /// Mostra a tela inicial de escolha de dificuldade do jogo
        /// </summary>
        private static void ShowStartScreen()
        {
            TargetCanvas.Reset();
            TargetCanvas.HideUI();
            ControlCache.PauseMenu.PausedMenuContent.Visibility = Visibility.Collapsed;
            ControlCache.PauseMenu.Grid.Children.Add(new DifficultySelectionUI());
            //ControlCache.PauseMenu.Refresh();
        }


        /// <summary>
        /// Carrega as configurações do jogo
        /// </summary>
        public static void LoadSettings()
        {
            Settings = Settings.LoadFrom("settings.config");
        }

        /// <summary>
        /// Prepara o cenário atual no canvas
        /// </summary>
        /// <param name="s"></param>
        private static void SetupScene(Scene s)
        {
            Entity.RemoveAllOfType(Tag.Tile);
            if (CurrentScene != null)
                CurrentScene.Background.Destroy();
            CurrentScene = s;
            CurrentScene.Show();

            //if (GameMaster.GetPlayer(0).IsPlaying)
            //    GetPlayer(0).Character.RigidBody.SetPosition(s.Player1Spawn);
            //if (GameMaster.GetPlayer(1).IsPlaying)
            //    GetPlayer(1).Character.RigidBody.SetPosition(s.Player2Spawn);
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
            Player1.Character.RigidBody.SetPosition(new Physics.Vector(637, -345.5));
            Player2.Character.RigidBody.SetPosition(new Physics.Vector(637, -345.5));
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

        /// <summary>
        /// Timer de alta frequência
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HighFrequencyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { IncreaseRunningTime(); IncreaseScore(); });
        }

        /// <summary>
        /// Aumenta a pontuaçã odos jogadores conforme o tempo
        /// </summary>
        private static void IncreaseScore()
        {
            Score += 1 * Time.Delta * DifficultyBonus;
        }

        /// <summary>
        /// Destrói e finaliza a partida atual
        /// </summary>
        internal static void Destroy()
        {
            if (InternalTimer != null)
                InternalTimer.Dispose();
            Players = null;
            if (Camera != null)
                Camera.Destroy();
            GameMaster.TargetCanvas.RemoveChild(GameMaster.CurrentScene.Background.VisualComponent);
            GameMaster.TargetCanvas.RemoveChild(GameMaster.CurrentScene.Foreground.VisualComponent);
            Money = 0;
            RunningTime = 0;
            Score = 0;
        }

        /// <summary>
        /// Evento disparado quando o timer interno é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações do evento</param>
        private static void InternalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate { FixedUpdate(); });
        }

        /// <summary>
        /// Método a ser chamado pelo timer de alta frequência
        /// </summary>
        private static void FixedUpdate()
        {
            if (GameOver || !Started)
                return;

            try
            {
                CheckForUserInput();
                CheckGameOver();

                if (GameplayState == ExecutionState.Paused)
                {
                    TargetCanvas.Reset();
                    Camera.ForceUpdate();
                    ControlCache.PauseMenu.Refresh();
                    TargetCanvas.UpdateUI();
                }
            }
            catch { }
        }

        /// <summary>
        /// Para a partida completamente
        /// </summary>
        private static void PauseForever()
        {
            Pause();
            InternalTimer.Stop();
        }

        /// <summary>
        /// Verifica se os jogadores estão vivos e decide o que fazer nessa situação
        /// </summary>
        private static void CheckGameOver()
        {
            if (GameOver)
                return;

            if (Players == null)
                return;

            if(GetPlayer(0).Character == null && GetPlayer(1).Character == null ||
            GetPlayer(0).Character != null && !GetPlayer(0).Character.IsAlive &&
            GetPlayer(1).Character != null && !GetPlayer(1).Character.IsAlive)
            {
                GameOver = true;
                PauseForever();
                ShowGameOverScreen();
                ShowCursor();
            }
        }

        /// <summary>
        /// Mostra a tela de fim de jogo
        /// </summary>
        private static void ShowGameOverScreen()
        {
            TargetCanvas.Reset();
            EndGameUI endUI = new EndGameUI();
            Canvas.SetZIndex(endUI, 12);
            GameMaster.TargetCanvas.HideUI();
            GameMaster.TargetCanvas.AddChild(ControlCache.PauseMenu);
            ControlCache.PauseMenu.PausedMenuContent.Visibility = Visibility.Collapsed;
            ControlCache.PauseMenu.Grid.Children.Add(endUI);
            Camera.ForceUpdate();
            ControlCache.PauseMenu.Refresh();
            CurrentScene.Background.Destroy();
            CurrentScene.Foreground.Destroy();
        }

        /// <summary>
        /// Aumenta o tempo em que a partida está ativa
        /// </summary>
        private static void IncreaseRunningTime()
        {
            RunningTime += Time.Delta;
        }

        /// <summary>
        /// Esconde o cursor
        /// </summary>
        public static void HideCursor()
        {
            Mouse.OverrideCursor = Cursors.None;
        }

        /// <summary>
        /// Mostra o cursor
        /// </summary>
        public static void ShowCursor()
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Verifica a entrada de usuário
        /// </summary>
        private static void CheckForUserInput()
        {
            if (Score < 1 || GameOver)
                return;

            if (!IgnoreKeyPress)
            {
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    if (GameplayState == ExecutionState.Paused)
                    {
                        Resume();
                        GameMaster.TargetCanvas.RemoveChild(ControlCache.PauseMenu);
                        ControlCache.PauseMenu.Grid.Children.Clear();
                        ControlCache.PauseMenu = new PauseMenuUI();
                        HideCursor();
                    }
                    else if (GameplayState == ExecutionState.Running)
                    {
                        Pause();
                        ControlCache.PauseMenu.Refresh();
                        GameMaster.TargetCanvas.AddChild(ControlCache.PauseMenu);
                        ShowCursor();
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
                    GetPlayer(0).Character.SetHealth(0);

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
                    Money += 10000;
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
