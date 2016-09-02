using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NHS111.Utils.Cache;

namespace NHS111.Utils.Itk
{
    public sealed class ItkMessageBuilder
    {
        private readonly ICacheManager<string, string> _cacheManager;

        public ItkMessageBuilder(ICacheManager<string,string> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public sealed class SummaryItem
        {
            public string Name { get; private set; }
            public string Caption { get; private set; }
            public IEnumerable<string> Values { get; private set; }

            public SummaryItem(string name, string caption, IEnumerable<string> values)
            {
                Name = name;
                Caption = caption;
                Values = values;
            }

            public SummaryItem(string name, string caption, string value) : this(name, caption, new List<string> { value }) { }

            public XElement AsXElement()
            {
                return new XElement("SummaryItem",
                    new XElement("Name", Name),
                    new XElement("Caption", Caption),
                    new XElement("Values",
                        Values.Select(value => new XElement("Value", value))
                    )
                );
            }
        }

        public sealed class Address
        {
            public string StreetAddressLine1 { get; private set; }
            public string StreetAddressLine2 { get; private set; }
            public string PostalCode { get; set; }

            public Address(string streetAddressLine1, string streetAddressLine2, string postalCode)
            {
                StreetAddressLine1 = streetAddressLine1;
                StreetAddressLine2 = streetAddressLine2;
                PostalCode = postalCode;
            }

            public IEnumerable<XElement> AsXElements()
            {
                return new List<XElement>
                {
                    new XElement("StreetAddressLine1", StreetAddressLine1),
                    new XElement("StreetAddressLine2", StreetAddressLine2),
                    new XElement("PostalCode", PostalCode)
                };
            }
        }

        public sealed class Service
        {
            public string Id { get; private set; }
            public string Name { get; private set; }
            public string OdsCode { get; private set; }
            public string ContactDetails { get; private set; }
            public string Address { get; private set; }
            public string PostCode { get; private set; }

            public Service(string id, string name, string odsCode, string contactDetails, string address, string postcode)
            {
                Id = id;
                Name = name;
                OdsCode = odsCode;
                ContactDetails = contactDetails;
                Address = address;
                PostCode = postcode;
            }

            public XElement AsXElement()
            {
                return new XElement("ServiceDetails",
                    new XElement("id", Id),
                    new XElement("name", Name),
                    new XElement("odsCode", OdsCode),
                    new XElement("contactDetails", ContactDetails),
                    new XElement("address", Address),
                    new XElement("postcode", PostCode)
                );
            }
        }

        private string Username { get; set; }
        private string Password { get; set; }
        private string ExternalReference { get; set; }
        private string DispositionCode { get; set; }
        private string DispositionName { get; set; }
        private IEnumerable<SummaryItem> SummaryItems { get; set; }
        private string Provider { get; set; }
        private string Forename { get; set; }
        private string Surname { get; set; }
        private string DateOfBirth { get; set; }
        private string Gender { get; set; }
        private string InformantType { get; set; }
        private Address CurrentAddress { get; set; }
        private Address HomeAddress { get; set; }
        private string TelephoneNumber { get; set; }
        private Service ServiceDetails { get; set; }
        private bool SendToRepeatCaller { get; set; }

        public ItkMessageBuilder SetUsername(string username)
        {
            Username = username;
            return this;
        }

        public ItkMessageBuilder SetPassword(string password)
        {
            Password = password;
            return this;
        }

        public ItkMessageBuilder SetExternalReference(string externalReference)
        {
            ExternalReference = externalReference;
            return this;
        }

        public ItkMessageBuilder SetDispositionCode(string dispositionCode)
        {
            DispositionCode = dispositionCode;
            return this;
        }

        public ItkMessageBuilder SetDispositionName(string dispositionName)
        {
            DispositionName = dispositionName;
            return this;
        }

        public ItkMessageBuilder SetSummaryItems(IEnumerable<SummaryItem> summaryItems)
        {
            SummaryItems = summaryItems;
            return this;
        }

        public ItkMessageBuilder SetProvider(string provider)
        {
            Provider = provider;
            return this;
        }

        public ItkMessageBuilder SetForename(string forename)
        {
            Forename = forename;
            return this;
        }

        public ItkMessageBuilder SetSurname(string surname)
        {
            Surname = surname;
            return this;
        }

        public ItkMessageBuilder SetDateOfBirth(string dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
            return this;
        }

        public ItkMessageBuilder SetGender(string gender)
        {
            Gender = gender;
            return this;
        }

        public ItkMessageBuilder SetInformantType(string informantType)
        {
            InformantType = informantType;
            return this;
        }

        public ItkMessageBuilder SetCurrentAddress(Address currentAddress)
        {
            CurrentAddress = currentAddress;
            return this;
        }

        public ItkMessageBuilder SetHomeAddress(Address homeAddress)
        {
            HomeAddress = homeAddress;
            return this;
        }

        public ItkMessageBuilder SetTelephoneNumber(string telephoneNumber)
        {
            TelephoneNumber = telephoneNumber;
            return this;
        }

        public ItkMessageBuilder SetServiceDetails(Service serviceDetails)
        {
            ServiceDetails = serviceDetails;
            return this;
        }

        public ItkMessageBuilder SetSendToRepeatCaller(bool sendToRepeatCaller)
        {
            SendToRepeatCaller = sendToRepeatCaller;
            return this;
        }

        public ItkMessageBuilder WithExample()
        {
            Username = "admn";
            Password = "KdtyX3Fp27IM4qMw1J";
            ExternalReference = "94487d5d-b5af-4f0c-bf2e-163f27a68cf0";
            DispositionCode = "13";
            DispositionName = "GPUrgent";
            SummaryItems = new List<SummaryItem>
            {
                new SummaryItem("SC_Or_HaSC_CandF", "Symptom checker or self care", "Health and symptom checker"),
                new SummaryItem("Worst_Symptom", "Which of your symptoms would you like advice about?", "Sore throat"),
                new SummaryItem("Throat_Symptoms", "Possible swollen throat", "Your throat is not swollen just painful"),
                new SummaryItem("High_Temperature", "High temperature", "Yes"),
                new SummaryItem("Immunecompromise_CF", "Existing health conditions", "Yes"),
                new SummaryItem("Early_meningitis", "Early signs of possible meningitis", "['Unusually pale or mottled skin']"),
                new SummaryItem("SymptomDiscriminatorList", "SymptomDiscriminatorList", "1159"),
                new SummaryItem("SymptomGroup", "SymptomGroup", "4003"),
                new SummaryItem("GP_Service_Status", "GP OOH Availability Status", "OOHAvailable"),
                new SummaryItem("GP_Service_Details_Address", "", "Test St Charles Hospital, Exmoor Street, London")
            };
            Provider = "nhschoices";
            Forename = "Test";
            Surname = "Test";
            DateOfBirth = "1999-01-01";
            Gender = "Male";
            InformantType = "NotSpecified";
            CurrentAddress = new Address("Test current", "Test current town", "w10 6dz");
            HomeAddress = new Address("Test", "Test", "ox1 1dj");
            TelephoneNumber = "07770728206";
            ServiceDetails = new Service("1329375103", "GP OOH: Care UK, Wandsworth, London (speak to GP)", "1329375103", "", "Various Locations", "SW17 0QT");
            SendToRepeatCaller = false;

            return this;
        }

        public string Build(string sessionId)
        {
            XNamespace ns = "services.nhsd.messages";
            XNamespace empty = "";

            var body = new XElement(ns + "SubmitHaSCToService",
                new XElement(empty + "Authentication",
                    new XElement("Username", Username),
                    new XElement("Password", Password)
                ),
                new XElement(empty + "SubmitEncounterToServiceRequest",
                    new XElement("CaseDetails",
                        new XElement("ExternalReference", ExternalReference),
                        new XElement("DispositionCode", DispositionCode),
                        new XElement("DispositionName", DispositionName),
                        new XElement("CaseSummary",
                            SummaryItems.Select(summaryItem => summaryItem.AsXElement())
                        ),
                        new XElement("Provider", Provider)
                    ),
                    new XElement("PatientDetails",
                        new XElement("Forename", Forename),
                        new XElement("Surname", Surname),
                        new XElement("DateOfBirth",
                            new XElement("dateOfBirth", DateOfBirth)
                        ),
                        new XElement("Gender", Gender),
                        new XElement("InformantType", InformantType),
                        new XElement("CurrentAddress",
                            CurrentAddress.AsXElements()
                        ),
                        new XElement("HomeAddress",
                            HomeAddress.AsXElements()
                        ),
                        new XElement("TelephoneNumber", TelephoneNumber)
                    ),
                    ServiceDetails.AsXElement(),
                    new XElement("SendToRepeatCaller", SendToRepeatCaller.ToString())
                )
            );

            var soapMsg = string.Format(@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body>{0}</soap:Body></soap:Envelope>", body);
            _cacheManager.Set(sessionId, soapMsg);
            return soapMsg;
        }
    }
}