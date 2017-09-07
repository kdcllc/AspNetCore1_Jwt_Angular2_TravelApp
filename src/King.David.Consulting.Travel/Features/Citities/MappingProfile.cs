using AutoMapper;

namespace King.David.Consulting.Travel.Web.Features.Citities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.City, CityModel>(MemberList.None);
        }
    }
}
