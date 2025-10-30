
using AutoMapper;
using Crime.DTOs;
using Crime.Models;

namespace Crime.Mapping
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
        }
    }
}
