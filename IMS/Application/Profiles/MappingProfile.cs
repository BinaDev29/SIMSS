// Application/Profiles/MappingProfile.cs
using AutoMapper;
using Application.DTOs.Customer;
using Application.DTOs.Audit;
using Application.DTOs.Notifications;
using Domain.Models;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Invoices, opt => opt.Ignore())
                .ForMember(dest => dest.Deliveries, opt => opt.Ignore())
                .ForMember(dest => dest.OutwardTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.ReturnTransactions, opt => opt.Ignore());

            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Invoices, opt => opt.Ignore())
                .ForMember(dest => dest.Deliveries, opt => opt.Ignore())
                .ForMember(dest => dest.OutwardTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.ReturnTransactions, opt => opt.Ignore());

            // Audit mappings
            CreateMap<AuditLog, AuditLogDto>().ReverseMap();

            // Notification mappings
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore());
        }
    }
}