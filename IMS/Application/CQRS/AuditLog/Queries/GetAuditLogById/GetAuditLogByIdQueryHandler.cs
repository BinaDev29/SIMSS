using MediatR;
using Application.Contracts;
using Application.DTOs.AuditLog;
using AutoMapper;

namespace Application.CQRS.AuditLog.Queries.GetAuditLogById
{
    public class GetAuditLogByIdQueryHandler : IRequestHandler<GetAuditLogByIdQuery, AuditLogDto?>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMapper _mapper;

        public GetAuditLogByIdQueryHandler(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<AuditLogDto?> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
        {
            var auditLog = await _auditLogRepository.GetByIdAsync(request.Id, cancellationToken);
            return auditLog == null ? null : _mapper.Map<AuditLogDto>(auditLog);
        }
    }
}