// Application/Profiles/InventoryReportProfile.cs
using Application.DTOs.InventoryReport;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class InventoryReportProfile : Profile
    {
        public InventoryReportProfile()
        {
            CreateMap<InventoryReport, InventoryReportDto>();
            CreateMap<CreateInventoryReportDto, InventoryReport>()
                .ForMember(dest => dest.GeneratedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Generating"));
        }
    }
}