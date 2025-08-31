// Application/Profiles/SmartReorderProfile.cs
using Application.DTOs.SmartReorder;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class SmartReorderProfile : Profile
    {
        public SmartReorderProfile()
        {
            CreateMap<SmartReorder, SmartReorderDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemName : null))
                .ForMember(dest => dest.GodownName, opt => opt.MapFrom(src => src.Godown != null ? src.Godown.GodownName : null))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.SupplierName : null));
            
            CreateMap<CreateSmartReorderDto, SmartReorder>();
        }
    }
}