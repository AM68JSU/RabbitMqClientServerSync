using AutoMapper;
using ClientServerSyncRabbitMq.Application.DTOs;
using ClientServerSyncRabbitMq.Domain.Entities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>(); // تعریف نقشه‌برداری بین مدل User و UserDto
        CreateMap<UserDto, User>(); // تعریف نقشه‌برداری برگشتی
    }
}
