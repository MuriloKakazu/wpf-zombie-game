using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using ZombieGame.Game;

namespace ZombieGame.Debug
{
    /// <summary>
    /// Interaction logic for DebugMonitor.xaml
    /// </summary>
    public partial class DebugMonitor : Window
    {
        public static bool HasAnOpenInstance { get; protected set; }
        protected Timer Updater { get; set; }

        public DebugMonitor()
        {
            InitializeComponent();
            Updater = new Timer();
            Updater.Elapsed += UpdateTimer_Elapsed;
            Updater.Interval = 1000;
            Updater.Enabled = true;
            HasAnOpenInstance = true;
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
                foreach (var v in Character.GetAllActiveCharacters())
                {
                    if (v.IsCamera || v.Tag == Game.Enums.Tags.Wall || v.IsEnemy)
                    { }
                    else
                    {
                        List.Items.Add(new Label() { Content = "-------------------" });
                        List.Items.Add(new Label() { Content = "Name: " + v.Name });
                        //List.Items.Add(new Label() { Content = "Hash: " + v.Hash });
                        List.Items.Add(new Label() { Content = "Health: " + v.Health });
                        List.Items.Add(new Label() { Content = "Money: " + v.Money });
                        List.Items.Add(new Label() { Content = "Level: " + v.Level });
                        List.Items.Add(new Label() { Content = "XP: " + v.Experience });
                        List.Items.Add(new Label() { Content = "Weapon: " + v.Weapon.Name });
                        //List.Items.Add(new Label() { Content = "Tag: " + v.Tag.ToString() });
                        //List.Items.Add(new Label() { Content = "IsPlayer: " + v.IsPlayer });
                        //List.Items.Add(new Label() { Content = "Sprite: " + v.Sprite.Uri });
                        //List.Items.Add(new Label() { Content = "IgnoreCollision: " + v.RigidBody.IgnoreCollisions });
                        List.Items.Add(new Label() { Content = "Mass: " + v.RigidBody.Mass });
                        //List.Items.Add(new Label() { Content = "UseRotation: " + v.RigidBody.UseRotation });
                        List.Items.Add(new Label() { Content = "Rotation: " + v.RigidBody.Rotation });
                        List.Items.Add(new Label()
                        {
                            Content = string.Format("Position: X: {0} Y: {1} Z: {2}",
                                                    v.RigidBody.Position.X,
                                                    v.RigidBody.Position.Y,
                                                    v.RigidBody.Position.Z)
                        });
                        List.Items.Add(new Label()
                        {
                            Content = string.Format("Size: X: {0} Y: {1} Z: {2}",
                                v.RigidBody.Size.X,
                                v.RigidBody.Size.Y,
                                v.RigidBody.Size.Z)
                        });
                        List.Items.Add(new Label()
                        {
                            Content = string.Format("Velocity: X: {0} Y: {1} Z: {2}",
                                v.RigidBody.Velocity.X,
                                v.RigidBody.Velocity.Y,
                                v.RigidBody.Velocity.Z)
                        });
                        List.Items.Add(new Label()
                        {
                            Content = string.Format("Force: X: {0} Y: {1} Z: {2}",
                                                    v.RigidBody.Force.X,
                                                    v.RigidBody.Force.Y,
                                                    v.RigidBody.Force.Z)
                        });
                        List.Items.Add(new Label()
                        {
                            Content = string.Format("Acceleration: X: {0} Y: {1} Z: {2}",
                                v.RigidBody.Acceleration.X,
                                v.RigidBody.Acceleration.Y,
                                v.RigidBody.Acceleration.Z)
                        });
                    }
                }
            }));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            HasAnOpenInstance = false;
        }
    }
}
