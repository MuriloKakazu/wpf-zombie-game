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
    public class RigidBody
    {
        /// <summary>
        /// Retângulo do corpo
        /// </summary>
        public Rect Bounds { get { return new Rect(Position.X, Position.Y, Size.X, Size.Y); } }
        /// <summary>
        /// Massa do corpo (kg)
        /// </summary>
        public float Mass { get; protected set; }
        /// <summary>
        /// Vetor posição do corpo
        /// </summary>
        public Vector Position { get; protected set; }
        /// <summary>
        /// Vetor posição central do corpo
        /// </summary>
        public Vector CenterPoint { get { return Bounds.GetVector(RectPositions.Center); } }
        /// <summary>
        /// Vetor tamanho do corpo em pixels
        /// </summary>
        public Vector Size { get; protected set; }
        /// <summary>
        /// Vetor força resultante aplicado ao corpo (N)
        /// </summary>
        public Vector Force { get; protected set; }
        /// <summary>
        /// Vetor velocidade aplicado ao corpo (m/s)
        /// </summary>
        public Vector Velocity { get; protected set; }
        /// <summary>
        /// Vetor aceleração aplicado ao corpo (m/s^2)
        /// </summary>
        public Vector Acceleration { get; protected set; }
        /// <summary>
        /// Rotação do corpo (graus)
        /// </summary>
        public float Rotation { get; protected set; }
        /// <summary>
        /// Coeficiente de arrasto do corpo
        /// </summary>
        public float Drag { get; protected set; }
        /// <summary>
        /// Define se deve-se permitir a rotação do corpo
        /// </summary>
        public bool UseRotation { get; set; }
        /// <summary>
        /// Define se o corpo estará sujeito à ação da gravidade
        /// </summary>
        public bool UseGravity { get; set; }
        /// <summary>
        /// Define se o corpo estará sujeito à mudança automática de posição
        /// </summary>
        public bool FixedPosition { get; set; }
        /// <summary>
        /// Define se o corpo está aterrado
        /// </summary>
        public bool IsGrounded { get; set; }
        /// <summary>
        /// Define se o corpo ignorará colisões
        /// </summary>
        public bool IgnoreCollisions { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public RigidBody()
        {
            Mass = 1f;
            Position = Vector.Zero;
            Size = Vector.Zero;
            Acceleration = Vector.Zero;
            Velocity = Vector.Zero;
            Force = Vector.Zero;
            UseGravity = false;
            Drag = 0.1f;
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
            if (!FixedPosition)
            {
                //Acceleration = Force / Mass;

                //if (UseGravity)
                //    Acceleration += Vector.EarthGravity;

                //Velocity += Acceleration * Time.Delta;
                //Position += Velocity * Time.Delta;

                //Force.Approximate(Vector.Zero, Drag);

                //if (UseGravity)
                //    Velocity.Approximate(Vector.EarthGravity, Drag);
                //else
                //    Velocity.Approximate(Vector.Zero, Drag);

                if (UseGravity && !IsGrounded)
                    Force += Vector.EarthGravity * Mass / Time.Delta;

                Acceleration = Force / Mass;

                //if (UseGravity && !IsGrounded)
                //    Acceleration += Vector.EarthGravity * 1000;

                Velocity += Acceleration * Time.Delta;
                Position += Velocity * Time.Delta;

                Force.Approximate(Vector.Zero, 10);

                //if (UseGravity)
                //    Velocity.Approximate(Vector.EarthGravity, Drag);
                //else
                //    Velocity.Approximate(Vector.Zero, Drag);
            }
        }
    }
}
