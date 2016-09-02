﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Models;

namespace NHS111.Web.Presentation.Converters
{
    public class DosCheckCapacitySummaryRequestTypeConverter : ITypeConverter<DosViewModel, DosCase>
    {
        public DosCheckCapacitySummaryRequestTypeConverter(string surgeryId)
        {
            _surgeryid = surgeryId;
        }

        public DosCheckCapacitySummaryRequestTypeConverter()
        {
        }

        private string _surgeryid = "UKN";
        public DosCase Convert(ResolutionContext context)
        {
            var dosviewModel = context.SourceValue as DosViewModel;
            if (dosviewModel == null) return null;

            var ageFormat = AgeFormatType.Years;
            throw new NotImplementedException();
        }
    }

    public class DispositionResolver : ValueResolver<String, int>{
        protected override int ResolveCore(string source)
        {
 	       if(!source.StartsWith("Dx")) throw new FormatException("Dx code does not have prefix \"Dx\". Cannot convert");

            return Convert.ToInt32(source.Replace("Dx", "10"));
        }
    }

      



}
