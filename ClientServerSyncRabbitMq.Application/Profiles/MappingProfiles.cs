using AutoMapper;
using ClientServerSyncRabbitMq.Application.DTOs.User;
using ClientServerSyncRabbitMq.Domain.Entities;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>(); // تعریف نقشه‌برداری بین مدل User و UserDto
        CreateMap<UserDto, User>(); // تعریف نقشه‌برداری برگشتی

        CreateMap<CreateUserDto, User>().ReverseMap(); // تعریف نقشه‌برداری برگشتی

    }
}
