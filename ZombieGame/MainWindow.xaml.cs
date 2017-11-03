using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZombieGame.Game;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;

namespace ZombieGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entity GroundEntity { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GameMaster.Setup();
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;

            GroundEntity = new Entity("Ground", Tags.Ground);
            GroundEntity.RigidBody.SetPosition(new Physics.Vector(0, -250));
            GroundEntity.RigidBody.Resize(new Physics.Vector(800, 50));
            GroundEntity.RigidBody.FixedPosition = true;
        }

        /// <summary>
        /// Atualiza as labels da tela e move os retângulos de acordo com a posição dos jogadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                DebugList.Items.Clear();
                var player = GameMaster.Player1;
                var pos1 = player.Character.RigidBody.Position;

                DebugList.Items.Add(new Label() { Content = string.Format("Player 1:") });
                DebugList.Items.Add(new Label() { Content = string.Format("IsSprinting: {0}", player.Character.IsSprinting) });
                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Force: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Force.X,
                                            player.Character.RigidBody.Force.Y,
                                            player.Character.RigidBody.Force.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Velocity: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Velocity.X,
                                            player.Character.RigidBody.Velocity.Y,
                                            player.Character.RigidBody.Velocity.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Acceleration: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Acceleration.X,
                                            player.Character.RigidBody.Acceleration.Y,
                                            player.Character.RigidBody.Acceleration.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Bounds: X: {0} Y: {1} Width: {2} Height: {3}",
                                            player.Character.RigidBody.Bounds.Left,
                                            player.Character.RigidBody.Bounds.Top,
                                            player.Character.RigidBody.Bounds.Width,
                                            player.Character.RigidBody.Bounds.Height)
                });

                player = GameMaster.Player2;
                var pos2 = player.Character.RigidBody.Position;

                DebugList.Items.Add(new Label() { Content = string.Format("Player 2:") });
                DebugList.Items.Add(new Label() { Content = string.Format("IsSprinting: {0}", player.Character.IsSprinting) });
                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Force: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Force.X,
                                            player.Character.RigidBody.Force.Y,
                                            player.Character.RigidBody.Force.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Velocity: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Velocity.X,
                                            player.Character.RigidBody.Velocity.Y,
                                            player.Character.RigidBody.Velocity.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Acceleration: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Acceleration.X,
                                            player.Character.RigidBody.Acceleration.Y,
                                            player.Character.RigidBody.Acceleration.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Bounds: X: {0} Y: {1} Width: {2} Height: {3}",
                                            player.Character.RigidBody.Bounds.Left,
                                            player.Character.RigidBody.Bounds.Top,
                                            player.Character.RigidBody.Bounds.Width,
                                            player.Character.RigidBody.Bounds.Height)
                });

                Canvas.SetLeft(Rectangle1, pos1.X);
                Canvas.SetTop(Rectangle1, -pos1.Y);
                Canvas.SetLeft(Rectangle2, pos2.X);
                Canvas.SetTop(Rectangle2, -pos2.Y);
                Canvas.SetLeft(Ground, GroundEntity.RigidBody.Position.X);
                Canvas.SetTop(Ground, -GroundEntity.RigidBody.Position.Y);
            }));

        }
    }
}
