using System.Threading.Tasks;

namespace NHS111.Utils.Monitoring
{
    public abstract class BaseMonitor : IMonitor
    {
        public string Ping()
        {
            return "pong";
        }

        public abstract string Metrics();

        public abstract Task<bool> Health();

        public abstract string Version();
    }
}