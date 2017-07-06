﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateOfBirthValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public DateOfBirthValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Date")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as UserInfo;

            return IsAValidDate(userInfo.Day, userInfo.Month, userInfo.Year);
        }

        private bool IsAValidDate(int? day, int? month, int? year)
        {
            DateTime date;
            if (!day.HasValue || !month.HasValue || !year.HasValue) return false;
            return DateTime.TryParseExact(String.Format("{0}/{1}/{2}", day.Value, month.Value, year.Value).ToString(),
                "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleDate = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "dateofbirth" // name of the validatoin which will be used inside unobtrusive library
            };

            ruleDate.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleDate;
        }
    }
}
