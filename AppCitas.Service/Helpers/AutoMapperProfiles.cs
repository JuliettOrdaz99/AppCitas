using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AutoMapper;

namespace AppCitas.Service.Helpers;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<AppUser, MemberDto>();
		CreateMap<Photo, PhotoDto>();
	}
}
