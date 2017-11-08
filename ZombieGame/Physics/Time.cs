using System;
using System.Timers;

namespace ZombieGame.Physics
{
    public static class Time
    {
        #region Properties
        /// <summary>
        /// Timer interno
        /// </summary>
        public static Timer InternalTimer { get; private set; }
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
            InternalTimer = new Timer();
            InternalTimer.Elapsed += OnElapsed;
            InternalTimer.Interval = 10;
            InternalTimer.Enabled = true;
        }

        /// <summary>
        /// Evento a ser disparado quando o timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito do evento</param>
        private static void OnElapsed(object sender, ElapsedEventArgs e)
        {
            float d = DateTime.Now.Millisecond - LastUpdate.Millisecond;
            if (d > 0)
                Delta = d / 1000;
            LastUpdate = DateTime.Now;
        }
        #endregion
    }
}
