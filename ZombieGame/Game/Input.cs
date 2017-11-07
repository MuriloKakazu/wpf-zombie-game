using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZombieGame.Debug;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game
{
    public static class Input
    {
        /// <summary>
        /// Retorna o valor de um eixo de entrada de usuário
        /// </summary>
        /// <param name="type">Tipo de eixo de entrada de usuário</param>
        /// <param name="player">Número do jogador ao qual o eixo se aplica</param>
        /// <returns>Valor do eixo</returns>
        public static float GetAxis(AxisTypes type, int player = 1)
        {
            float output = 0;
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (player == 1)
                {
                    if (type == AxisTypes.Horizontal)
                    {
                        if (Keyboard.IsKeyDown(Key.A))
                            output += -1;
                        if (Keyboard.IsKeyDown(Key.D))
                            output += 1;
                    }
                    else if (type == AxisTypes.Vertical)
                    {
                        if (Keyboard.IsKeyDown(Key.S))
                            output += -1;
                        if (Keyboard.IsKeyDown(Key.W))
                            output += 1;
                        //if (Keyboard.IsKeyDown(Key.W))
                        //    output = 1;
                    }
                    else if (type == AxisTypes.Fire)
                    {
                        if (Keyboard.IsKeyDown(Key.Space))
                            output = 1;
                    }
                    else if (type == AxisTypes.Sprint)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                            output = 1;
                    }
                }
                else if (player == 2)
                {
                    if (type == AxisTypes.Horizontal)
                    {
                        if (Keyboard.IsKeyDown(Key.Left))
                            output += -1;
                        if (Keyboard.IsKeyDown(Key.Right))
                            output += 1;
                    }
                    else if (type == AxisTypes.Vertical)
                    {
                        if (Keyboard.IsKeyDown(Key.Down))
                            output += -1;
                        if (Keyboard.IsKeyDown(Key.Up))
                            output += 1;
                        //if (Keyboard.IsKeyDown(Key.Up))
                        //    output = 1;
                    }
                    else if (type == AxisTypes.Fire)
                    {
                        if (Keyboard.IsKeyDown(Key.NumPad0))
                            output = 1;
                    }
                    else if (type == AxisTypes.Sprint)
                    {
                        if (Keyboard.IsKeyDown(Key.Enter))
                            output = 1;
                    }
                }

                if (Keyboard.IsKeyDown(Key.F5))
                {
                    GameMaster.Player1.Character.RigidBody.SetForce(GameMaster.Player1.Character.RigidBody.Front.Opposite.Normalized * 100);
                }
                else if (Keyboard.IsKeyDown(Key.F1))
                {
                    if (!DebugMonitor.HasAnOpenInstance)
                    {
                        DebugMonitor dm = new DebugMonitor();
                        dm.Show();
                    }
                }
                else if (Keyboard.IsKeyDown(Key.F2))
                {
                    if (Enemy.Enemies.Count < 10)
                    {
                        for (int i = 0; i < 10; i++)
                            EnemySpawner.SpawnZombie();
                    }
                }
                else if (Keyboard.IsKeyDown(Key.F3))
                {
                    foreach (var e in Enemy.Enemies.ToArray())
                        e.Kill(GameMaster.GetPlayer(player).Character);
                }
            });
            return output;
        }
    }
}
