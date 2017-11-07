using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ZombieGame.Game;

namespace ZombieGame.Physics
{
    public static class Time
    {
        public static Timer InternalTimer { get; private set; }
        /// <summary>
        /// Retorna a diferença de tempo em milisegundos desde a última decorrência do timer de atualização
        /// </summary>
        public static float Delta { get; private set; }
        /// <summary>
        /// Retorna o momento da última decorrência do timer de atualização
        /// </summary>
        private static DateTime LastUpdate { get; set; }

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
    }
}
