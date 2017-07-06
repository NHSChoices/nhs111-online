﻿using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(AgeGenderViewModelValidator))]
    public class AgeGenderViewModel
    {
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
