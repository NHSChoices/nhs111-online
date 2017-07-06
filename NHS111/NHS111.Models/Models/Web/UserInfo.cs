﻿using System;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;


namespace NHS111.Models.Models.Web
{
    [Validator(typeof(UserInfoValidator))]
    public class UserInfo
    {
        public AgeGenderViewModel Demography { get; set; }
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
                {
                    try
                    {
                        _dob = new DateTime(Year.Value, Month.Value, Day.Value);
                        return _dob;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        private string _telephoneNumber;
        public string TelephoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(_telephoneNumber)) return string.Empty;

                _telephoneNumber = _telephoneNumber.Replace(" ", "");

                if (_telephoneNumber.LastIndexOf("+44", StringComparison.Ordinal) == 0)
                    _telephoneNumber = _telephoneNumber.Replace("+44", "0");

                return _telephoneNumber;
            }
            set { _telephoneNumber = value; }
        }

        public string Email { get; set; }

        public AddressInfoViewModel HomeAddress { get; set; }
        public FindServicesAddressViewModel CurrentAddress { get; set; }

        public UserInfo()
        {
            HomeAddress = new AddressInfoViewModel();
            CurrentAddress = new FindServicesAddressViewModel();
        }
    }
}