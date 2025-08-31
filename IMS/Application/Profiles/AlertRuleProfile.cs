// Application/Profiles/AlertRuleProfile.cs
using Application.DTOs.AlertRule;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class AlertRuleProfile : Profile
    {
        public AlertRuleProfile()
        {
            CreateMap<AlertRule, AlertRuleDto>();
            CreateMap<CreateAlertRuleDto, AlertRule>();
        }
    }
}