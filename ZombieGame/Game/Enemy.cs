﻿using System;
using System.Collections.Generic;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Events;

namespace ZombieGame.Game
{
    public abstract class Enemy : Character
    {
        #region Properties
        /// <summary>
        /// Lista estática de todos os inimigos ativos
        /// </summary>
        private static List<Enemy> Enemies = new List<Enemy>();

        /// <summary>
        /// Retorna o tipo de inimigo
        /// </summary>
        public EnemyTypes Type { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna todos os inimigos ativos
        /// </summary>
        /// <returns></returns>
        public static Enemy[] GetAllActiveEnemies()
        {
            return Enemies.ToArray();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">O tipo de inimigo</param>
        public Enemy(EnemyTypes type) : base(type.ToString(), Tags.Enemy)
        {
            Type = type;
            RigidBody.UseRotation = true;
            SetMoney();
            SetHealth();
            SetExperience();
            Enemies.Add(this);
        }

        /// <summary>
        /// Método que aciona funções que precisam ser disparadas constantemente
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (!IsStunned)
                Chase(GetNearestPlayer(), 10);
            else
                RigidBody.SetVelocity(Vector.Zero);
        }

        /// <summary>
        /// Faz com que o inimigo persiga um alvo
        /// </summary>
        /// <param name="target">Alvo</param>
        /// <param name="speedMagnitude">Magnitude da velocidade de perseguição</param>
        protected virtual void Chase(Entity target, float speedMagnitude)
        {
            if (RigidBody.Acceleration.Magnitude > 0)
                RigidBody.Acceleration.Approximate(Vector.Zero, 10);
            RigidBody.SetForce(RigidBody.Force / 1.1f);
            RigidBody.SetVelocity(RigidBody.CenterPoint.PointedAt(target.RigidBody.CenterPoint).Opposite.Normalized * speedMagnitude);
            RigidBody.PointAt(RigidBody.CenterPoint.PointedAt(target.RigidBody.CenterPoint).Opposite);
        }

        /// <summary>
        /// Define quanto de dinheiro o inimigo dará ao morrer
        /// </summary>
        private void SetMoney()
        {
            int maxMoney = default(int), minMoney = default(int), avgLvls = 0;

            foreach (var p in GameMaster.Players)
                avgLvls += p.Character.Level;

            avgLvls /= GameMaster.Players.Length;

            switch (Type)
            {
                case EnemyTypes.Walker:
                    maxMoney = 20 * avgLvls + 10 * Level;
                    minMoney = 5 * avgLvls + 10 * Level;
                    break;
                case EnemyTypes.Boss:
                    maxMoney = 1000 * avgLvls + 300 * Level;
                    minMoney = 400 * avgLvls + 200 * Level;
                    break;
            }

            Money = new Random().Next(minMoney, maxMoney);
        }

        /// <summary>
        /// Define a vida do inimigo
        /// </summary>
        private void SetHealth()
        {
            switch (Type)
            {
                case EnemyTypes.Walker:
                    Health = 50 * Level;
                    break;
                case EnemyTypes.Boss:
                    Health = 400 * Level + 200;
                    break;
            }
        }

        /// <summary>
        /// Define a experiência que o inimigo dará ao morrer
        /// </summary>
        private void SetExperience()
        {
            switch (Type)
            {
                case EnemyTypes.Walker:
                    Experience = 25 * Level;
                    break;
                case EnemyTypes.Boss:
                    Experience = 20 * Level + 10 * Level * Level;
                    break;
            }
        }

        /// <summary>
        /// Destrói o objeto, liberando memória
        /// </summary>
        public override void Destroy()
        {
            Enemies.Remove(this);
            base.Destroy();
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade entra em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionEnter(object sender, CollisionEventArgs e)
        {
            if (e.Collider.IsPlayer)
            {
                var colliderChar = Character.FromHashCode(e.Collider.Hash);
                if (colliderChar != null)
                    colliderChar.RigidBody.AddForce(e.CollisionDirection.Opposite.Normalized * 1000);
            }
            //else if (e.Collider.IsEnemy)
            //{
            //    RigidBody.SetVelocity(e.CollisionDirection.Normalized * 200);
            //}
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade mantém-se em colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionStay(object sender, CollisionEventArgs e)
        {
            //base.OnCollisionStay(sender, e);
            if (e.Collider.IsCamera)
                return;

            if (e.Collider.Tag != Tags.Projectile && e.Collider.Tag != Tags.Wall)
            {
                var colliderChar = Character.FromHashCode(e.Collider.Hash);
                if (colliderChar != null && colliderChar.IsPlayer)
                    colliderChar.Damage(damager: this, quantity: 1);

                RigidBody.SetVelocity(e.CollisionDirection.Normalized * e.Collider.RigidBody.Velocity.Magnitude);
                RigidBody.AddForce(e.CollisionDirection.Normalized * 5);
            }
        }

        /// <summary>
        /// Evento a ser disparado quando a entidade sai de uma colisão com outra entidade
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito da colisão</param>
        protected override void OnCollisionLeave(object sender, CollisionEventArgs e)
        {
            //if (e.Collider.IsEnemy)
            //    RigidBody.SetForce(Vector.Zero);
        }
        #endregion
    }
}
