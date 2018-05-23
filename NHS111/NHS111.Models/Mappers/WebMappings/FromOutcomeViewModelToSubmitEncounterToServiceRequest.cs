﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper.TypeConversion;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using ServiceDetails = NHS111.Models.Models.Web.ITK.ServiceDetails;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToSubmitEncounterToServiceRequest : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, CaseDetails>()
                .ConvertUsing<FromOutcomeViewModelToCaseDetailsConverter>();

            Mapper.CreateMap<PersonalDetailViewModel, PatientDetails>()
                .ConvertUsing<FromPersonalDetailViewModelToPatientDetailsConverter>();

            Mapper.CreateMap<OutcomeViewModel, ServiceDetails>()
                .ConvertUsing<FromOutcomeViewModelToServiceDetailsConverter>();

            Mapper.CreateMap<List<JourneyStep>, List<String>>()
              .ConvertUsing<FromJourneySetpsToReportTextStrings>();

        }
    }

    public class FromOutcomeViewModelToCaseDetailsConverter : ITypeConverter<OutcomeViewModel, CaseDetails>
    {
        public CaseDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var caseDetails = (CaseDetails)context.DestinationValue ?? new CaseDetails();

            caseDetails.ExternalReference = outcome.JourneyId.ToString();
            caseDetails.DispositionCode = outcome.Id;
            caseDetails.DispositionName = outcome.Title;
            caseDetails.Source = outcome.PathwayTitle;
            caseDetails.ReportItems = Mapper.Map<List<JourneyStep>, List<string>>(outcome.Journey.Steps);
            caseDetails.ConsultationSummaryItems = outcome.Journey.Steps.Where(s => !string.IsNullOrEmpty(s.Answer.DispositionDisplayText)).Select(s => s.Answer.ReportText).Distinct().ToList();
            return caseDetails;
        }
    }

    public class FromJourneySetpsToReportTextStrings : ITypeConverter<List<JourneyStep>, List<string>>
    {
        public List<string> Convert(ResolutionContext context)
        {
            var steps = (List<JourneyStep>)context.SourceValue;
            return steps.Where(s => !string.IsNullOrEmpty(s.Answer.ReportText)).Select(s => s.Answer.ReportText).ToList();
        }
    }

    public class FromPersonalDetailViewModelToPatientDetailsConverter : ITypeConverter<PersonalDetailViewModel, PatientDetails>
    {
        public PatientDetails Convert(ResolutionContext context)
        {
            var personalDetailViewModel = (PersonalDetailViewModel)context.SourceValue;
            var patientDetails = (PatientDetails)context.DestinationValue ?? new PatientDetails();

            patientDetails.Forename = personalDetailViewModel.UserInfo.FirstName;
            patientDetails.Surname = personalDetailViewModel.UserInfo.LastName;
            patientDetails.ServiceAddressPostcode = personalDetailViewModel.SelectedService.PostCode;
            patientDetails.TelephoneNumber = personalDetailViewModel.UserInfo.TelephoneNumber;
            patientDetails.CurrentAddress = MapAddress(personalDetailViewModel.AddressInformation.PatientCurrentAddress);
            if (personalDetailViewModel.AddressInformation.HomeAddressSameAsCurrent.HasValue)
            {
                if (personalDetailViewModel.AddressInformation.HomeAddressSameAsCurrent.Value ==
                    HomeAddressSameAsCurrent.Yes)
                {
                    patientDetails.HomeAddress =
                        MapAddress(personalDetailViewModel.AddressInformation.PatientCurrentAddress);
                }
                else if (personalDetailViewModel.AddressInformation.HomeAddressSameAsCurrent.Value ==
                         HomeAddressSameAsCurrent.No)
                {
                    patientDetails.HomeAddress =
                        MapAddress(personalDetailViewModel.AddressInformation.PatientHomeAddreess);
                }
            }
            if (personalDetailViewModel.UserInfo.Year != null && personalDetailViewModel.UserInfo.Month != null && personalDetailViewModel.UserInfo.Day != null)
                patientDetails.DateOfBirth =
                    new DateTime(personalDetailViewModel.UserInfo.Year.Value, personalDetailViewModel.UserInfo.Month.Value, personalDetailViewModel.UserInfo.Day.Value);

            patientDetails.Gender = personalDetailViewModel.UserInfo.Demography.Gender;
            
            patientDetails.Informant = new InformantDetails()
            {
                Forename = personalDetailViewModel.Informant.Forename,
                Surname = personalDetailViewModel.Informant.Surname,
                TelephoneNumber = personalDetailViewModel.UserInfo.TelephoneNumber,
                Type = personalDetailViewModel.Informant.IsInformantForPatient ? NHS111.Models.Models.Web.ITK.InformantType.NotSpecified : NHS111.Models.Models.Web.ITK.InformantType.Self
            };           
            
            return patientDetails;
        }

        private Address MapAddress(PersonalDetailsAddressViewModel addressViewModel)
        {
            return new Address()
            {
                PostalCode = addressViewModel.Postcode,
                StreetAddressLine1 =
                    !string.IsNullOrEmpty(addressViewModel.HouseNumber)
                        ? string.Format("{0} {1}", addressViewModel.HouseNumber, addressViewModel.AddressLine1)
                        : addressViewModel.AddressLine1,
                StreetAddressLine2 = addressViewModel.AddressLine2,
                StreetAddressLine3 = addressViewModel.City,
                StreetAddressLine4 = addressViewModel.County,
                StreetAddressLine5 = addressViewModel.Postcode
            };
        }
    }

    public class FromOutcomeViewModelToServiceDetailsConverter : ITypeConverter<OutcomeViewModel, ServiceDetails>
    {
        public ServiceDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var serviceDetails = (ServiceDetails)context.DestinationValue ?? new ServiceDetails();

            serviceDetails.Id = outcome.SelectedService.Id.ToString();
            serviceDetails.Name = outcome.SelectedService.Name;
            serviceDetails.OdsCode = outcome.SelectedService.OdsCode;
            serviceDetails.PostCode = outcome.SelectedService.PostCode;

            return serviceDetails;
        }
    }
}
