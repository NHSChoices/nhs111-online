using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class LivePathwaysCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LivePathwaysElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LivePathwaysElement)element).PathwayNumber;
        }
    }
}
