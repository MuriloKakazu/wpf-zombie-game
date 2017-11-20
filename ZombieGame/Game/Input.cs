using System;
using System.Windows;
using System.Windows.Input;
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
                    else if (type == AxisTypes.Reload)
                    {
                        if (Keyboard.IsKeyDown(Key.R))
                            output = 1;
                    }
                }
                else if (player == 2)
                {
                    if (type == AxisTypes.Horizontal)
                    {
                        if (Keyboard.IsKeyDown(Key.J))
                            output += -1;
                        if (Keyboard.IsKeyDown(Key.L))
                            output += 1;
                    }
                    else if (type == AxisTypes.Vertical)
                    {
                        if (Keyboard.IsKeyDown(Key.K))
                            output += -1;
                        if (Keyboard.IsKeyDown(Key.I))
                            output += 1;
                    }
                    else if (type == AxisTypes.Fire)
                    {
                        if (Keyboard.IsKeyDown(Key.Delete))
                            output = 1;
                    }
                    else if (type == AxisTypes.Sprint)
                    {
                        if (Keyboard.IsKeyDown(Key.End))
                            output = 1;
                    }
                    else if (type == AxisTypes.Reload)
                    {
                        if (Keyboard.IsKeyDown(Key.P))
                            output = 1;
                    }
                }
            });
            return output;
        }
    }
}
