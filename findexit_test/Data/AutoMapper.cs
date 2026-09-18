using AutoMapper;
using findexit_test.Models;
using findexit_test.Models.Dto;

namespace findexit_test.Data;

public class AutoMapper : Profile
{
    public AutoMapper() {
        CreateMap<User, UserInfoDto>();
    }
}