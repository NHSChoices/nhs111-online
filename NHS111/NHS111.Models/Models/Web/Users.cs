//TODO here only for the time being

using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Models.Models.Web
{
    public class Users
    {
        public string NHSNumber { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Postcode { get; set; }
        public DateTime DoB { get; set; }
        public string Gender { get; set; }

        public static Users GetRandomUser()
        {
            var usersList = new List<Users>();

            usersList.Add(new Users { NHSNumber = "9990248753", FamilyName = "XXTESTPATIENTRAAD", GivenName = "NIC-QTP-DONOTUSE", Title = "MRS", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1977, 5, 1), Gender = "Female" });
            usersList.Add(new Users { NHSNumber = "9990248761", FamilyName = "XXTESTPATIENTRAAE", GivenName = "NIC-QTP-DONOTUSE", Title = "MS", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1946, 1, 13), Gender = "Female" });
            usersList.Add(new Users { NHSNumber = "9990248796", FamilyName = "XXTESTPATIENTRAAG", GivenName = "NIC-QTP-DONOTUSE", Title = "MR", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(2007, 7, 18), Gender = "Male" });
            usersList.Add(new Users { NHSNumber = "9990248958", FamilyName = "XXTESTPATIENTRAAU", GivenName = "NIC-QTP-DONOTUSE", Title = "MS", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(2006, 10, 4), Gender = "Female" });
            usersList.Add(new Users { NHSNumber = "9990248966", FamilyName = "XXTESTPATIENTRAAV", GivenName = "NIC-QTP-DONOTUSE", Title = "MR", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1943, 9, 13), Gender = "Male" });
            usersList.Add(new Users { NHSNumber = "9990249040", FamilyName = "XXTESTPATIENTRABD", GivenName = "NIC-QTP-DONOTUSE", Title = "MR", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1987, 1, 6), Gender = "Male" });
            usersList.Add(new Users { NHSNumber = "9990249059", FamilyName = "XXTESTPATIENTRABE", GivenName = "NIC-QTP-DONOTUSE", Title = "MR", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1992, 9, 11), Gender = "Male" });
            usersList.Add(new Users { NHSNumber = "9990249172", FamilyName = "XXTESTPATIENTRABP", GivenName = "NIC-QTP-DONOTUSE", Title = "MISS", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1969, 11, 29), Gender = "Female" });
            usersList.Add(new Users { NHSNumber = "9990249199", FamilyName = "XXTESTPATIENTRABR", GivenName = "NIC-QTP-DONOTUSE", Title = "MS", AddressLine1 = "C/O NPFIT TEST DATA MGR", AddressLine2 = "PRINCES EXCHANGE", AddressLine3 = "PRINCES SQUARE", AddressLine4 = "LEEDS", AddressLine5 = "WEST YORKSHIRE", Postcode = "LS1 4HY", DoB = new DateTime(1937, 9, 19), Gender = "Female" });


            var rnd = new Random();
            var numUser = rnd.Next(0, 9);

            return usersList.ElementAt(numUser);

        }
    }
}