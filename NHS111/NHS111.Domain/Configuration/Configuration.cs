using System.Configuration;

namespace NHS111.Domain.Configuration
{
    public class Configuration : IConfiguration
    {
        public string GetGraphDbUrl()
        {
            return ConfigurationManager.AppSettings["GraphDbUrl"];
        }
    }

    public interface IConfiguration
    {
        string GetGraphDbUrl();
    }
}