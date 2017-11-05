using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZombieGame.Game;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Physics
{
    public sealed class RigidBody
    {
        /// <summary>
        /// Retângulo do corpo
        /// </summary>
        public Rect Bounds { get { return new Rect(Position.X, Position.Y, Size.X, Size.Y); } }
        /// <summary>
        /// Massa do corpo (kg)
        /// </summary>
        public float Mass { get; private set; }
        /// <summary>
        /// Vetor posição do corpo
        /// </summary>
        public Vector Position { get; private set; }
        /// <summary>
        /// Vetor que aponta para frente do corpo
        /// </summary>
        public Vector Front { get; private set; }
        /// <summary>
        /// Vetor posição central do corpo
        /// </summary>
        public Vector CenterPoint { get { return Bounds.GetVector(RectPositions.Center); } }
        /// <summary>
        /// Vetor tamanho do corpo em pixels
        /// </summary>
        public Vector Size { get; private set; }
        /// <summary>
        /// Vetor força resultante aplicado ao corpo (N)
        /// </summary>
        public Vector Force { get; private set; }
        /// <summary>
        /// Vetor velocidade aplicado ao corpo (m/s)
        /// </summary>
        public Vector Velocity { get; private set; }
        /// <summary>
        /// Vetor aceleração aplicado ao corpo (m/s^2)
        /// </summary>
        public Vector Acceleration { get; private set; }
        /// <summary>
        /// Rotação do corpo (graus)
        /// </summary>
        public float Rotation { get; private set; }
        /// <summary>
        /// Coeficiente de arrasto do corpo
        /// </summary>
        public float Drag { get; private set; }
        /// <summary>
        /// Multiplicador do módulo da velocidade
        /// </summary>
        public float SpeedMultiplier { get; private set; }
        /// <summary>
        /// Define se deve-se permitir a rotação do corpo
        /// </summary>
        public bool UseRotation { get; set; }
        /// <summary>
        /// Define se o corpo terá uma posição fixa
        /// </summary>
        public bool FixedPosition { get; set; }
        /// <summary>
        /// Define se o corpo ignorará colisões
        /// </summary>
        public bool IgnoreCollisions { get; set; }
        /// <summary>
        /// Retorna se o corpo está acelerando
        /// </summary>
        public bool IsAccelerating { get { return Acceleration.Magnitude != 0; } }

        /// <summary>
        /// ctor
        /// </summary>
        public RigidBody()
        {
            Mass = 1f;
            Position = Vector.OffScreen;
            Front = Vector.Right;
            Size = Vector.Zero;
            Acceleration = Vector.Zero;
            Velocity = Vector.Zero;
            Force = Vector.Zero;
            Rotation = 0;
            Drag = 1f;
            SpeedMultiplier = 1f;
        }

        /// <summary>
        /// Redefine a posição do corpo
        /// </summary>
        /// <param name="p">Nova posição</param>
        public void SetPosition(Vector p)
        {
            Position = p;
        }

        /// <summary>
        /// Redimensiona o corpo
        /// </summary>
        /// <param name="s">Novo tamanho</param>
        public void Resize(Vector s)
        {
            Size = s;
        }

        /// <summary>
        /// Adiciona um vetor força ao corpo
        /// </summary>
        /// <param name="f">Vetor força</param>
        public void AddForce(Vector f)
        {
            Force += f;
        }

        /// <summary>
        /// Subtrai um vetor força do corpo
        /// </summary>
        /// <param name="f">Vetor força</param>
        public void RemoveForce(Vector f)
        {
            Force -= f;
        }

        /// <summary>
        /// Redefine o vetor força aplicado ao corpo
        /// </summary>
        /// <param name="f"></param>
        public void SetForce(Vector f)
        {
            Force = f;
        }

        /// <summary>
        /// Redefine a massa do corpo
        /// </summary>
        /// <param name="m"></param>
        public void SetMass(float m)
        {
            Mass = m;
        }

        /// <summary>
        /// Zera os vetores aplicados ao movimento do corpo
        /// </summary>
        public void Stop()
        {
            Force = Vector.Zero;
            Velocity = Vector.Zero;
        }

        /// <summary>
        /// Adiciona um vetor velocidade ao corpo
        /// </summary>
        /// <param name="v">Vetor velocidade</param>
        public void AddVelocity(Vector v)
        {
            Velocity += v;
        }

        /// <summary>
        /// Redefine o vetor velocidade aplicado ao corpo
        /// </summary>
        /// <param name="v"></param>
        public void SetVelocity(Vector v)
        {
            Velocity = v;
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        public void Update()
        {
            Acceleration = Force / Mass;

            if (Velocity.Magnitude > 0)
            {
                if (UseRotation)
                    Rotation = MathExtension.RadiansToDegrees(Velocity.AngleBetween(Vector.Right));
                Front = Velocity.Normalized;
            }
            Velocity += Acceleration * Time.Delta;
            Velocity *= SpeedMultiplier;
            if (!FixedPosition)
                Position += Velocity * Time.Delta;

            Force.Approximate(Vector.Zero, 10);
        }
    }
}
