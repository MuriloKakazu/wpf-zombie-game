using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZombieGame.Game;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Debug
{
    /// <summary>
    /// Interaction logic for DebugMonitor.xaml
    /// </summary>
    public partial class DebugMonitor : Window
    {
        public DebugMonitor()
        {
            InitializeComponent();
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        /// <summary>
        /// Atualiza as labels da tela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                List.Items.Clear();
                var player = GameMaster.Player1;
                var pos1 = player.Character.RigidBody.Position;

                List.Items.Add(new Label() { Content = string.Format("Player 1:") });
                List.Items.Add(new Label() { Content = string.Format("IsSprinting: {0}", player.Character.IsSprinting) });
                List.Items.Add(new Label() { Content = string.Format("IsGrounded: {0}", player.Character.RigidBody.IsGrounded) });
                List.Items.Add(new Label()
                {
                    Content = string.Format("Force: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Force.X,
                                            player.Character.RigidBody.Force.Y,
                                            player.Character.RigidBody.Force.Z)
                });

                List.Items.Add(new Label()
                {
                    Content = string.Format("Velocity: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Velocity.X,
                                            player.Character.RigidBody.Velocity.Y,
                                            player.Character.RigidBody.Velocity.Z)
                });

                List.Items.Add(new Label()
                {
                    Content = string.Format("Acceleration: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Acceleration.X,
                                            player.Character.RigidBody.Acceleration.Y,
                                            player.Character.RigidBody.Acceleration.Z)
                });

                List.Items.Add(new Label()
                {
                    Content = string.Format("Bounds: X: {0} Y: {1} Width: {2} Height: {3}\n" +
                                            "TL: {4},{5} TR: {6},{7} BL: {8},{9} BR: {10},{11} CT: {12},{13} CL: {14},{15} CR: {16},{17} CB: {18},{19} C: {20},{21}",
                                            player.Character.RigidBody.Bounds.Left, player.Character.RigidBody.Bounds.Top,
                                            player.Character.RigidBody.Bounds.Width, player.Character.RigidBody.Bounds.Height,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.TopLeft).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.TopLeft).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.TopRight).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.TopRight).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.BottomLeft).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.BottomLeft).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.BottomRight).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.BottomRight).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterTop).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterTop).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterLeft).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterLeft).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterRight).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterRight).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterBottom).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.CenterBottom).Y,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.Center).X,
                                            player.Character.RigidBody.Bounds.GetVector(Physics.Enums.RectPositions.Center).Y)
                });

                player = GameMaster.Player2;
                var pos2 = player.Character.RigidBody.Position;

                List.Items.Add(new Label() { Content = string.Format("Player 2:") });
                List.Items.Add(new Label() { Content = string.Format("IsSprinting: {0}", player.Character.IsSprinting) });
                List.Items.Add(new Label() { Content = string.Format("IsGrounded: {0}", player.Character.RigidBody.IsGrounded) });
                List.Items.Add(new Label()
                {
                    Content = string.Format("Force: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Force.X,
                                            player.Character.RigidBody.Force.Y,
                                            player.Character.RigidBody.Force.Z)
                });

                List.Items.Add(new Label()
                {
                    Content = string.Format("Velocity: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Velocity.X,
                                            player.Character.RigidBody.Velocity.Y,
                                            player.Character.RigidBody.Velocity.Z)
                });

                List.Items.Add(new Label()
                {
                    Content = string.Format("Acceleration: X: {0} Y: {1} Z: {2}",
                                            player.Character.RigidBody.Acceleration.X,
                                            player.Character.RigidBody.Acceleration.Y,
                                            player.Character.RigidBody.Acceleration.Z)
                });

                List.Items.Add(new Label()
                {
                    Content = string.Format("Bounds: X: {0} Y: {1} Width: {2} Height: {3}",
                                            player.Character.RigidBody.Bounds.Left,
                                            player.Character.RigidBody.Bounds.Top,
                                            player.Character.RigidBody.Bounds.Width,
                                            player.Character.RigidBody.Bounds.Height)
                });
            }));
        }
    }
}
