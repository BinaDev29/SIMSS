using MediatR;
using Application.Contracts;
using Application.DTOs.Employee;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            // 💡 ከ Godown ጋር የተያያዘውን ሰራተኛ ያመጣል
            var employee = await employeeRepository.GetEmployeeWithDetailsAsync(request.Id, cancellationToken);

            return mapper.Map<EmployeeDto>(employee);
        }
    }
}