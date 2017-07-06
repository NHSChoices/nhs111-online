﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateOfBirthYearValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public DateOfBirthYearValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Year")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as UserInfo;

            return IsAValidYear(userInfo.Year);
        }

        private bool IsAValidYear(int? year)
        {
           return (year > 1900 && year < (DateTime.Now.Year + 1));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleYear = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "year" // name of the validatoin which will be used inside unobtrusive library
            };

            ruleYear.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleYear;
        }
    }
}
