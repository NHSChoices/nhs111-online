﻿using System;
using System.Collections.Generic;
using NHS111.Models.Models.Business.Enums;

namespace NHS111.Business.DOS.EndpointFilter
{
    public interface IServiceAvailabilityProfile
    {
        int ProfileId { get; set; }

        string ProfileName { get; set; }

        DispositionTimePeriod GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes);

        IEnumerable<int> ServiceTypeIdBlacklist { get; }
    }
}
