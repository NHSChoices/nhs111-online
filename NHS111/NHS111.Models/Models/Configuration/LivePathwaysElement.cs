using System.Configuration;

namespace NHS111.Models.Models.Configuration
{
    public class LivePathwaysElement : ConfigurationElement
    {
        [ConfigurationProperty("title", IsRequired = true)]
        public string Title
        {
            get
            {
                return (string)this["title"];
            }
            set
            {
                this["title"] = value;
            }
        }

        [ConfigurationProperty("pathwayNo", IsRequired = true)]
        public string PathwayNumber
        {
            get
            {
                return (string)this["pathwayNo"];
            }
            set
            {
                this["pathwayNo"] = value;
            }
        }
    }
}
