using System.Threading.Tasks;

namespace NHS111.Utils.Monitoring
{
    public interface IMonitor
    {
        string Ping();
        string Metrics();
        Task<bool> Health();
        string Version();
    }
}