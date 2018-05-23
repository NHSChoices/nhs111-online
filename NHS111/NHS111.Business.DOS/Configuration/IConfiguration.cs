﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NodaTime;

namespace NHS111.Business.DOS.Configuration
{
    public interface IConfiguration
    {
        LocalTime WorkingDayGenericInHoursStartTime { get; }
        LocalTime WorkingDayGenericInHoursEndTime { get; }
        LocalTime WorkingDayGenericInHoursShoulderEndTime { get; }

        LocalTime WorkingDayPrimaryCareInHoursStartTime { get; }
        LocalTime WorkingDayPrimaryCareInHoursEndTime { get; }
        LocalTime WorkingDayPrimaryCareInHoursShoulderEndTime { get; }

        LocalTime WorkingDayDentalInHoursStartTime { get; }
        LocalTime WorkingDayDentalInHoursEndTime { get; }
        LocalTime WorkingDayDentalInHoursShoulderEndTime { get; }

        string DomainDosApiBaseUrl { get; }
        string DomainDosApiCheckCapacitySummaryUrl { get; }
        string DomainDosApiServiceDetailsByIdUrl { get; }
        string DomainDosApiMonitorHealthUrl { get; }
        string DomainDosApiServicesByClinicalTermUrl { get; }
        string FilteredGenericDispositionCodes { get; }
        string FilteredPrimaryCareDispositionCodes { get; }
        string FilteredDentalDispositionCodes { get; }
        string FilteredGenericDosServiceIds { get; }
        string FilteredPrimaryCareDosServiceIds { get; }
        string FilteredDentalDosServiceIds { get; }
        string FilteredClinicianCallbackDispositionCodes { get; }
        string FilteredClinicianCallbackDosServiceIds { get; }
        string DosUsername { get; }
        string DosPassword { get; }
        string CCGApiGetCCGByPostcode { get; }
        string CCGApiBaseUrl { get; }
        int DoSSearchDistance { get; }
    }
}
