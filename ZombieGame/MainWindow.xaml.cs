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
using ZombieGame.Physics;

namespace ZombieGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GameMaster.Setup();
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                DebugList.Items.Clear();
                var player = GameMaster.Player1;
                var pos1 = player.Character.RigidBody.Position;

                DebugList.Items.Add(new Label() { Content = string.Format("Axis:") });
                DebugList.Items.Add(new Label() { Content = string.Format("Horizontal1: {0}", Input.HorAxis1) });
                DebugList.Items.Add(new Label() { Content = string.Format("Horizontal2: {0}", Input.HorAxis2) });
                DebugList.Items.Add(new Label() { Content = string.Format("Vertical1: {0}", Input.VerAxis1) });
                DebugList.Items.Add(new Label() { Content = string.Format("Vertical2: {0}", Input.VerAxis2) });

                DebugList.Items.Add(new Label() { Content = string.Format("Player 1:")});
                DebugList.Items.Add(new Label() { Content = string.Format("IsSprinting: {0}", player.Character.IsSprinting) });
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
                    Content = string.Format("Position: X: {0} Y: {1} Z: {2}",
                                            pos1.X,
                                            pos1.Y,
                                            pos1.Z)
                });

                player = GameMaster.Player2;
                var pos2 = player.Character.RigidBody.Position;

                DebugList.Items.Add(new Label() { Content = string.Format("Player 2:") });
                DebugList.Items.Add(new Label() { Content = string.Format("IsSprinting: {0}", player.Character.IsSprinting) });
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
                    Content = string.Format("Position: X: {0} Y: {1} Z: {2}",
                                            pos2.X,
                                            pos2.Y,
                                            pos2.Z)
                });

                Canvas.SetLeft(Rectangle1, pos1.X);
                Canvas.SetTop(Rectangle1, - pos1.Y);
                Canvas.SetLeft(Rectangle2, pos2.X);
                Canvas.SetTop(Rectangle2, -pos2.Y);
            }));

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            #region Player 1
            if (e.Key == Key.A)
                Input.HorAxis1 = -1;
            if (e.Key == Key.D)
                Input.HorAxis1 = 1;
            if (e.Key == Key.W)
                Input.VerAxis1 = 1;
            if (e.Key == Key.S)
                Input.VerAxis1 = -1;
            if (e.Key == Key.Space)
                Input.FireAxis1 = 1;
            if (e.Key == Key.LeftShift)
                Input.SprintAxis1 = 1;
            #endregion

            #region Player 2
            if (e.Key == Key.Left)
                Input.HorAxis2 = -1;
            if (e.Key == Key.Right)
                Input.HorAxis2 = 1;
            if (e.Key == Key.Up)
                Input.VerAxis2 = 1;
            if (e.Key == Key.Down)
                Input.VerAxis2 = -1;
            if (e.Key == Key.NumPad0)
                Input.FireAxis2 = 1;
            if (e.Key == Key.Enter)
                Input.SprintAxis2 = 1;
            #endregion
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            #region Player 1
            if (e.Key == Key.A || e.Key == Key.D)
                Input.HorAxis1 = 0;
            if (e.Key == Key.W || e.Key == Key.S)
                Input.VerAxis1 = 0;
            if (e.Key == Key.Space)
                Input.FireAxis1 = 0;
            if (e.Key == Key.LeftShift)
                Input.SprintAxis1 = 0;
            #endregion

            #region Player 2
            if (e.Key == Key.Left || e.Key == Key.Right)
                Input.HorAxis2 = 0;
            if (e.Key == Key.Up || e.Key == Key.Down)
                Input.VerAxis2 = 0;
            if (e.Key == Key.NumPad0)
                Input.FireAxis2 = 0;
            if (e.Key == Key.Enter)
                Input.SprintAxis2 = 0;
            #endregion
        }
    }
}
