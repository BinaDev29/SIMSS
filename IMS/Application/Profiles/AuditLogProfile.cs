// Application/Profiles/AuditLogProfile.cs
using Application.DTOs.AuditLog;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class AuditLogProfile : Profile
    {
        public AuditLogProfile()
        {
            CreateMap<AuditLog, AuditLogDto>();
        }
    }
}