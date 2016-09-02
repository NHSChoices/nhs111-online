﻿using System;

namespace NHS111.Models.Models.Web
{
    public class UserInfo
    {
        public string Gender { get; set; }
        public int Age { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        private DateTime? _dob;
        public DateTime? DoB
        {
            get
            {
                if (Year != null && Month != null && Day != null)
                    return _dob = new DateTime(Year.Value, Month.Value, Day.Value);
                return _dob;
            }
        }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }


        public AddressInfo HomeAddress { get; set; }
        public AddressInfo CurrentAddress { get; set; }

        public UserInfo()
        {
            HomeAddress = new AddressInfo();
            CurrentAddress = new AddressInfo();
        }
    }
}