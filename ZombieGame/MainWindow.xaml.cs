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
                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("IsGrounded: {0} \n", GameMaster.Player1.Character.IsGrounded)
                });
                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Velocity: \nX: {0} \nY: {1} \nZ: {2} \n",
                                                                          GameMaster.Player1.Character.RigidBody.Velocity.X,
                                                                          GameMaster.Player1.Character.RigidBody.Velocity.Y,
                                                                          GameMaster.Player1.Character.RigidBody.Velocity.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Acceleration: \nX: {0} \nY: {1} \nZ: {2} \n",
                                                                                      GameMaster.Player1.Character.RigidBody.Acceleration.X,
                                                                                      GameMaster.Player1.Character.RigidBody.Acceleration.Y,
                                                                                      GameMaster.Player1.Character.RigidBody.Acceleration.Z)
                });

                DebugList.Items.Add(new Label()
                {
                    Content = string.Format("Position: \nX: {0} \nY: {1} \nZ: {2} \n",
                                                                      GameMaster.Player1.Character.RigidBody.Position.X,
                                                                      GameMaster.Player1.Character.RigidBody.Position.Y,
                                                                      GameMaster.Player1.Character.RigidBody.Position.Z)
                });

                var pos = GameMaster.Player1.Character.RigidBody.Position;

                Canvas.SetLeft(Rectangle, pos.X * 25);
                Canvas.SetTop(Rectangle, - pos.Y * 25);
            }));

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
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
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.D)
                Input.HorAxis1 = 0;
            if (e.Key == Key.W || e.Key == Key.S)
                Input.VerAxis1 = 0;
            if (e.Key == Key.Space)
                Input.FireAxis1 = 0;
            if (e.Key == Key.LeftShift)
                Input.SprintAxis1 = 0;
        }
    }
}
