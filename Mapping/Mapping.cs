
using AutoMapper;
using CrimeManagment.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CaseCreateDTO, Cases>();
            CreateMap<UpdateCaseDTO, Cases>();
            CreateMap<CrimeReportCreateDTO, CrimeReports>();

            CreateMap<Cases, CaseListDTO>();
            CreateMap<Cases, CaseDetailsDTO>();
            CreateMap<UsersCreateDTO, Users>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.SecondName} {src.LastName}".Trim()));

            CreateMap<UpdateUserDto, Users>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Users, UsersCreateDTO>();

            // Mapping for CaseParticipants
            CreateMap<CaseParticipants, CaseParticipantDto>();
            CreateMap<CreateCaseParticipantDto, CaseParticipants>();
            CreateMap<UpdateCaseParticipantDto, CaseParticipants>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}

