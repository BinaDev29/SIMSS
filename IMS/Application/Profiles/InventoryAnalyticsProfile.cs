// Application/Profiles/InventoryAnalyticsProfile.cs
using Application.DTOs.InventoryAnalytics;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class InventoryAnalyticsProfile : Profile
    {
        public InventoryAnalyticsProfile()
        {
            CreateMap<InventoryAnalytics, InventoryAnalyticsDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemName : null))
                .ForMember(dest => dest.GodownName, opt => opt.MapFrom(src => src.Godown != null ? src.Godown.GodownName : null));
        }
    }
}