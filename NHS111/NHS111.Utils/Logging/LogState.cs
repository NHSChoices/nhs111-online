using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace NHS111.Utils.Logging
{
    public static class LogState
    {
        private static readonly Level state = new Level(50000, "STATE");

        public static void State(this ILog log, string message)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, state, message, null);
        }

        public static void State(this ILog log, string message, params object[] args)
        {
            var formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, state, formattedMessage, null);
        }

    }
}
