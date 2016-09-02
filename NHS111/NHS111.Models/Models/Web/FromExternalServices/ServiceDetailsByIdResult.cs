namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class ServiceDetailsByIdResult
    {
        public ContactType TagField;

        public string NameField;

        public string ValueField;

        public int OrderField;
    }

    public enum ContactType
    {

        /// <remarks/>
        dts,

        /// <remarks/>
        itk,

        /// <remarks/>
        telno,

        /// <remarks/>
        email,

        /// <remarks/>
        faxno,
    }
}
