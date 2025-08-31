// Application/Profiles/DemandForecastProfile.cs
using Application.DTOs.DemandForecast;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class DemandForecastProfile : Profile
    {
        public DemandForecastProfile()
        {
            CreateMap<DemandForecast, DemandForecastDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemName : null))
                .ForMember(dest => dest.GodownName, opt => opt.MapFrom(src => src.Godown != null ? src.Godown.GodownName : null));
            
            CreateMap<CreateDemandForecastDto, DemandForecast>();
        }
    }
}