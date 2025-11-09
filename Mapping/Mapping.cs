
using AutoMapper;
using CrimeManagementApi.DTOs;
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
            CreateMap<CrimeReportDto, CrimeReports>();

            CreateMap<Cases, CaseListDTO>();
            CreateMap<Cases, CaseDetailsDTO>();
            CreateMap<UsersCreateDTO, Users>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.SecondName} {src.LastName}".Trim()));

            CreateMap<UpdateUserDto, Users>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Users, UsersCreateDTO>();
            CreateMap<Users, UserDTO>();

            // Mapping for CaseParticipants
            CreateMap<CaseParticipants, CaseParticipantDto>();
            CreateMap<CreateCaseParticipantDto, CaseParticipants>();
            CreateMap<UpdateCaseParticipantDto, CaseParticipants>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CaseAssigneesDTOs.AssignUserDTO, CaseAssignees>()
     .ForMember(dest => dest.AssignedByUserId, opt => opt.MapFrom(src => src.AssignerId))
     .ForMember(dest => dest.AssignedToUserId, opt => opt.MapFrom(src => src.AssigneeId))
     .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))  // بدل AssigneeRole
     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProgreessStatus.Pending)) // بدل ProgreessStatus
     .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => DateTime.UtcNow));





        }
    }
}

