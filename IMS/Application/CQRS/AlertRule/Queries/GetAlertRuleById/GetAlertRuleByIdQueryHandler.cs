using MediatR;
using Application.Contracts;
using Application.DTOs.AlertRule;
using AutoMapper;

namespace Application.CQRS.AlertRule.Queries.GetAlertRuleById
{
    public class GetAlertRuleByIdQueryHandler : IRequestHandler<GetAlertRuleByIdQuery, AlertRuleDto?>
    {
        private readonly IAlertRuleRepository _alertRuleRepository;
        private readonly IMapper _mapper;

        public GetAlertRuleByIdQueryHandler(IAlertRuleRepository alertRuleRepository, IMapper mapper)
        {
            _alertRuleRepository = alertRuleRepository;
            _mapper = mapper;
        }

        public async Task<AlertRuleDto?> Handle(GetAlertRuleByIdQuery request, CancellationToken cancellationToken)
        {
            var alertRule = await _alertRuleRepository.GetByIdAsync(request.Id, cancellationToken);
            return alertRule == null ? null : _mapper.Map<AlertRuleDto>(alertRule);
        }
    }
}