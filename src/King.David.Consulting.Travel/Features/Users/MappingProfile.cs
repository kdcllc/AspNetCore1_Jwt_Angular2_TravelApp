using AutoMapper;

namespace King.David.Consulting.Travel.Web.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.User, User>(MemberList.None);
        }
    }
}
