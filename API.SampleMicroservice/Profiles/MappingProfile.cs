using API.SampleMicroservice.DataModels.Request;
using API.SampleMicroservice.DataModels.Response;
using API.SampleMicroservice.Entities;
using AutoMapper;

namespace API.SampleMicroservice.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SampleMicroserviceEntity, SampleEntityCreateUpdateCommand>().ReverseMap()
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => !string.IsNullOrWhiteSpace(dest.Name) ? dest.Name.Trim() : string.Empty));
			CreateMap<SampleMicroserviceEntity, SampleEntityResponseModel>().ReverseMap();
		}
    }
}