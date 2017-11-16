using System;
using System.Timers;

namespace ZombieGame.Physics
{
    public static class Time
    {
        #region Properties
        public static Timer LowPriorityTimer { get; private set; }
        /// <summary>
        /// Timer interno
        /// </summary>
        public static Timer HighPriorityTimer { get; private set; }
        /// <summary>
        /// Retorna a diferença de tempo em segundos desde a última decorrência do timer de atualização
        /// </summary>
        public static float Delta { get; private set; }
        /// <summary>
        /// Retorna o momento da última decorrência do timer de atualização
        /// </summary>
        private static DateTime LastUpdate { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Instancia o timer interno do objeto
        /// </summary>
        public static void Setup()
        {
            HighPriorityTimer = new Timer();
            HighPriorityTimer.Elapsed += OnHighPriorityTimerElapsed;
            HighPriorityTimer.Interval = 1;
            LowPriorityTimer = new Timer();
            LowPriorityTimer.Interval = 50;
        }

        /// <summary>
        /// Pausa o timer interno
        /// </summary>
        public static void Pause()
        {
            HighPriorityTimer.Stop();
            LowPriorityTimer.Stop();
        }

        /// <summary>
        /// Resume o timer interno
        /// </summary>
        public static void Resume()
        {
            LastUpdate = DateTime.Now;
            HighPriorityTimer.Start();
            LowPriorityTimer.Start();
        }

        /// <summary>
        /// Evento a ser disparado quando o timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito do evento</param>
        private static void OnHighPriorityTimerElapsed(object sender, ElapsedEventArgs e)
        {
            float d = DateTime.Now.Millisecond - LastUpdate.Millisecond;
            if (d > 0)
                Delta = d / 1000;
            LastUpdate = DateTime.Now;
        }
        #endregion
    }
}
