using AutoMapper;
using Demo.Infrastructure.Entity;
using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.ResultModel;

namespace Demo.Infrastructure.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateUserModel, User>()
                .ForMember(x=>x.UserName,opt=>opt.MapFrom(src=>src.Email));
            CreateMap<User, UserResultItem>();
            CreateMap<UserLog, UserLogResultItem>();
        }
    }
}
