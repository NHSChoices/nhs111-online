﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web
{
    public class PageDataViewModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PageType
        {
            ModuleZero = 0,
            Demographics,
            Search,
            SearchResults,
            Categories,
            FirstQuestion,
            Question,
            InlineCareAdvice,
            PostcodeFirst,
            Outcome,
            ServiceDetails,
            ServiceList,
            PersonalDetails,
            Confirmation,
            DuplicateBooking,
            BookingFailure,
            BookingUnavailable,
            Error
        }

        public PageDataViewModel()
        {
            Page = PageType.ModuleZero;
        }
        public PageDataViewModel(PageType page, string campaign, string source)
        {
            Page = page;
            Campaign = campaign;
            Source = source;
        }

        public PageDataViewModel(PageType page, JourneyViewModel journey)
        {
            Page = page;
            Campaign = journey.Campaign;
            Source = journey.Source;
            Gender = journey.UserInfo.Demography.Gender;
            Age = new AgeCategory(journey.UserInfo.Demography.Age).Value;
            SearchString = journey.EntrySearchTerm;
            QuestionId = journey.OutcomeGroup == null ? journey.Id : null;
            TxNumber = journey.OutcomeGroup == null ? journey.QuestionNo : null;
            StartingPathwayNo = journey.PathwayNo;
            StartingPathwayTitle = journey.PathwayTitle;
            DxCode = journey.OutcomeGroup != null ? journey.Id : null;
        }

        public PageType Page { get; set; }
        public string TxNumber { get; set; }
        public string QuestionId { get; set; }
        public string StartingPathwayNo { get; set; }
        public string StartingPathwayTitle { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string PathwayNo { get; set; }
        public string PathwayTitle { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DxCode { get; set; }
        public string SearchString { get; set; }
        public string Campaign { get; set; }
        public string Source { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
