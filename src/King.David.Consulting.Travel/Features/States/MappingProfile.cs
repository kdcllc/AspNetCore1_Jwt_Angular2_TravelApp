using AutoMapper;

namespace King.David.Consulting.Travel.Web.Features.States
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.State, StateModel>(MemberList.None);
        }
    }
}
