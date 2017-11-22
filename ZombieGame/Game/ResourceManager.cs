using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game.Entities;
using ZombieGame.Physics;

namespace ZombieGame.Game
{
    public static class ResourceManager
    {
        /// <summary>
        /// Diferença de tempo desde a última limpeza de recursos inativos
        /// </summary>
        private static float DeltaT { get; set; }
        /// <summary>
        /// Tempo intervalo entre chamadas de limpeza de recursos inativos
        /// </summary>
        private static float ThresholdTimeMs { get; set; }

        /// <summary>
        /// Prepara o gerenciador de recursos
        /// </summary>
        public static void Setup()
        {
            DeltaT = 0;
            ThresholdTimeMs = 1000;
            Time.HighFrequencyTimer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Evento a ser disparado conforme o timer de atualização
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações do evento</param>
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.InvokeAsync(delegate { IncreaseDeltaT(); });
        }

        /// <summary>
        /// Aumenta a difereça de tempo desde a última chamada de limpeza de recursos
        /// </summary>
        private static void IncreaseDeltaT()
        {
            DeltaT += Time.Delta * 1000;

            if (DeltaT >= ThresholdTimeMs)
            {
                DeltaT = 0;
                DestroyAllInactiveEntities();
            }
        }

        /// <summary>
        /// Destrói todos os recursos carregados
        /// </summary>
        public static void DestroyEverything()
        {
            if (Time.HighFrequencyTimer != null)
                Time.HighFrequencyTimer.Dispose();
            if (Time.LowFrequencyTimer != null)
                Time.LowFrequencyTimer.Dispose();
            GameMaster.Destroy();
            if (GameMaster.CurrentScene != null)
            {
                GameMaster.TargetCanvas.RemoveChild(GameMaster.CurrentScene.Background.VisualComponent);
                GameMaster.TargetCanvas.RemoveChild(GameMaster.CurrentScene.Foreground.VisualComponent);
            }

            foreach (var e in AnimatedEntity.Instances)
                e.Destroy();

            foreach (var e in Enemy.Instances)
                e.Destroy();

            foreach (var e in Character.Instances)
                e.Destroy();

            foreach (var e in Projectile.Instances)
                e.Destroy();

            foreach (var e in Entity.Instances)
                e.Destroy();
        }

        /// <summary>
        /// Destrói recursos inativos e suas referências para que o GarbageCollector passe a identificá-los com maior facilidade
        /// </summary>
        public static void DestroyAllInactiveEntities()
        {
            foreach (var e in AnimatedEntity.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Enemy.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Character.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Projectile.Instances)
                if (!e.IsActive) e.Destroy();

            foreach (var e in Entity.Instances)
                if (!e.IsActive) e.Destroy();
        }
    }
}
