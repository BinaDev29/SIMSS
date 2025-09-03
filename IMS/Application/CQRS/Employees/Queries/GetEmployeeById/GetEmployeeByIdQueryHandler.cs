using MediatR;
using Application.Contracts;
using Application.DTOs.Employee;
using AutoMapper;

namespace Application.CQRS.Employee.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }
    }
}