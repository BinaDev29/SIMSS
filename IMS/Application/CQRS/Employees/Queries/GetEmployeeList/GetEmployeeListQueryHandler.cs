using MediatR;
using Application.Contracts;
using Application.DTOs.Employee;
using AutoMapper;

namespace Application.CQRS.Employee.Queries.GetEmployeeList
{
    public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeListQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<EmployeeDto>>(employees);
        }
    }
}