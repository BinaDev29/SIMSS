using MediatR;
using Application.Contracts;
using Application.DTOs.AlertRule;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.AlertRule.Queries.GetAlertRules
{
    public class GetAlertRulesQueryHandler : IRequestHandler<GetAlertRulesQuery, IReadOnlyList<AlertRuleDto>>
    {
        private readonly IAlertRuleRepository _alertRuleRepository;
        private readonly IMapper _mapper;

        public GetAlertRulesQueryHandler(IAlertRuleRepository alertRuleRepository, IMapper mapper)
        {
            _alertRuleRepository = alertRuleRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AlertRuleDto>> Handle(GetAlertRulesQuery request, CancellationToken cancellationToken)
        {
            var query = _alertRuleRepository.Query().AsNoTracking();

            // Add filtering, sorting, paging here if needed

            var alertRules = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<AlertRuleDto>>(alertRules);
        }
    }
}