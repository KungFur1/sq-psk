using AuthService.DTOs;
using AuthService.Models;
using AutoMapper;

namespace AuthService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<SignUpDto, User>()
            .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => src.Password));

        CreateMap<Session, SessionKeyResponseDto>();
        
        CreateMap<Session, SessionInfoResponseDto>();

        CreateMap<User, UserPublicResponseDto>();
    }
}
