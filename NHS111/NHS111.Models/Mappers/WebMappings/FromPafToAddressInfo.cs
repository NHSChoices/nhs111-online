using System.Collections.Generic;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromPafToAddressInfo : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<List<PAF>, List<AddressInfo>>()
                 .ConvertUsing<FromPafToAddressInfoConverter>();

        }

        public class FromPafToAddressInfoConverter : ITypeConverter<List<PAF>, List<AddressInfo>>
        {
            public List<AddressInfo> Convert(ResolutionContext context)
            {
                var pafList = (List<PAF>)context.SourceValue;
                var listAddressInfo = new List<AddressInfo>();
                foreach (var paf in pafList)
                {

                    listAddressInfo.Add(new AddressInfo
                    {
                        HouseNumber = paf.BuildingName,
                        AddressLine1 = paf.BuildingNumber,
                        AddressLine2 = paf.DepartmentName,
                        City = paf.Town,
                        PostCode = paf.Postcode
                    });
                }

                return listAddressInfo;
            }
        }
    }
   
}