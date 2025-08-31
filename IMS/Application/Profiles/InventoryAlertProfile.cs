// Application/Profiles/InventoryAlertProfile.cs
using Application.DTOs.InventoryAlert;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class InventoryAlertProfile : Profile
    {
        public InventoryAlertProfile()
        {
            CreateMap<InventoryAlert, InventoryAlertDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemName : null))
                .ForMember(dest => dest.GodownName, opt => opt.MapFrom(src => src.Godown != null ? src.Godown.GodownName : null));
            
            CreateMap<CreateInventoryAlertDto, InventoryAlert>();
            CreateMap<UpdateInventoryAlertDto, InventoryAlert>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}