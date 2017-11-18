using System;
using System.Windows;
using System.Xml.Serialization;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Extensions;

namespace ZombieGame.Physics
{
    public sealed class RigidBody
    {
        #region Properties
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
        /// Vetor força resultante aplicado ao corpo (kg*pixels/s^2 * 10)
        /// </summary>
        public Vector Force { get; private set; }
        /// <summary>
        /// Vetor velocidade aplicado ao corpo (pixels/s * 10)
        /// </summary>
        public Vector Velocity { get; private set; }
        /// <summary>
        /// Vetor aceleração aplicado ao corpo (pixels/s^2 * 10)
        /// </summary>
        public Vector Acceleration { get { return Force / Mass; } }
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
        public float SpeedMultiplier { get; set; }
        /// <summary>
        /// Define se deve-se permitir a rotação do corpo
        /// </summary>
        public bool UseRotation { get; set; }
        /// <summary>
        /// Define se o corpo terá movimento
        /// </summary>
        public bool Frozen { get; private set; }
        /// <summary>
        /// Define se o corpo ignorará colisões
        /// </summary>
        public bool IgnoreCollisions { get; set; }
        /// <summary>
        /// Retorna se o corpo está acelerando
        /// </summary>
        public bool IsAccelerating { get { return Acceleration.Magnitude != 0; } }
        #endregion

        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        public RigidBody()
        {
            Mass = 1f;
            Position = Vector.OffScreen;
            Front = Vector.Right;
            Size = Vector.Zero;
            Velocity = Vector.Zero;
            Force = Vector.Zero;
            Rotation = 0;
            Drag = 1f;
            SpeedMultiplier = 1f;
        }
        
        /// <summary>
        /// Congela o corpo, impedindo seu movimento
        /// </summary>
        public void Freeze()
        {
            Frozen = true;
        }

        /// <summary>
        /// Descongela o corpo, permitindo seu movimento
        /// </summary>
        public void Unfreeze()
        {
            Frozen = false;
        }

        /// <summary>
        /// Aponta a frente do corpo a uma direção
        /// </summary>
        /// <param name="location"></param>
        public void PointAt(Vector location)
        {
            Front = location.Normalized;
            RotateToFront();
        }

        /// <summary>
        /// Rotaciona o corpo à sua direção frontal
        /// </summary>
        private void RotateToFront()
        {
            Rotation = MathExtension.RadiansToDegrees(Front.AngleBetween(Vector.Right));
        }

        public void SetRotation(float value)
        {
            Rotation = value;
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
        /// Redefine o coeficiente de arrasto do corpo
        /// </summary>
        /// <param name="d"></param>
        public void SetDrag(float d)
        {
            Drag = d;
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        public void FixedUpdate()
        {
            if (Velocity.Magnitude > 0 && UseRotation)
                RotateToFront();

            if (!Frozen)
            {
                Velocity += Acceleration * Time.Delta * SpeedMultiplier;
                Position += Velocity * Time.Delta * 10;
            }

            Force.Approximate(Vector.Zero, Time.Delta * 1000);
        }
        #endregion
    }
}
