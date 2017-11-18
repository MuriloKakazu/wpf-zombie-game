using System;
using System.Diagnostics;
using System.Timers;

namespace ZombieGame.Physics
{
    public static class Time
    {
        #region Properties
        public static Timer LowFrequencyTimer { get; private set; }
        /// <summary>
        /// Timer interno
        /// </summary>
        public static Timer HighFrequencyTimer { get; private set; }
        /// <summary>
        /// Retorna a diferença de tempo em segundos desde a última decorrência do timer de atualização
        /// </summary>
        public static float Delta { get; private set; }
        /// <summary>
        /// Retorna o momento da última decorrência do timer de atualização
        /// </summary>
        private static DateTime LastUpdate { get; set; }
        private static Stopwatch Stopwatch { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Instancia o timer interno do objeto
        /// </summary>
        public static void Setup()
        {
            Stopwatch = new Stopwatch();
            HighFrequencyTimer = new Timer();
            HighFrequencyTimer.Elapsed += OnHighPriorityTimerElapsed;
            HighFrequencyTimer.Interval = 1;
            LowFrequencyTimer = new Timer();
            LowFrequencyTimer.Interval = 50;
        }

        /// <summary>
        /// Pausa o timer interno
        /// </summary>
        public static void Pause()
        {
            HighFrequencyTimer.Stop();
            LowFrequencyTimer.Stop();
        }

        /// <summary>
        /// Resume o timer interno
        /// </summary>
        public static void Resume()
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            HighFrequencyTimer.Start();
            LowFrequencyTimer.Start();
        }

        /// <summary>
        /// Evento a ser disparado quando o timer de atualização é decorrido
        /// </summary>
        /// <param name="sender">Objeto que invocou o evento</param>
        /// <param name="e">Informações a respeito do evento</param>
        private static void OnHighPriorityTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Stopwatch.Stop();
            Delta = Stopwatch.ElapsedMilliseconds / 1000f;
            //Console.WriteLine(Delta);
            Stopwatch.Reset();
            Stopwatch.Start();
        }
        #endregion
    }
}
