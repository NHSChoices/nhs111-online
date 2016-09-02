﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web
{
    using System.Threading.Tasks;

    public class DosViewModel : DosCase
    {
        public Guid SessionId { get; set; }
        public DosCheckCapacitySummaryResult DosCheckCapacitySummaryResult { get; set; }
        public DosServicesByClinicalTermResult DosServicesByClinicalTermResult { get; set; }
        public string CheckCapacitySummaryResultListJson { get; set; }
        public IEnumerable<CareAdvice> CareAdvices { get; set; }
        public IEnumerable<string> CareAdviceMarkers { get; set; }
        public List<int> SearchDistances { get; set; }
        public string Title { get; set; }
        public string SelectedServiceId { get; set; }
        public string JourneyJson { get; set; }
        public string PathwayNo { get; set; }
        public DosService SelectedService
        {
            get
            {
                return DosCheckCapacitySummaryResult.Success == null ? null : DosCheckCapacitySummaryResult.Success.Services.FirstOrDefault(s => s.Id == Convert.ToInt32(SelectedServiceId));
            }
        }

        public IEnumerable<OutcomeViewModel> Dispositions { get; set; }

        public DosViewModel()
        {
            CareAdvices = new List<CareAdvice>();
            CareAdviceMarkers = new List<string>();
            SearchDistances = new List<int>() { 0, 10, 30, 60, 99 };
            DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
        }
    }
}