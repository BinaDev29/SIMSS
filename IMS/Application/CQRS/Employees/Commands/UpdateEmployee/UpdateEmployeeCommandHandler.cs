using Application.Contracts;
using Application.DTOs.Employee;
using Application.DTOs.Employee.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, IValidator<UpdateEmployeeDto> validator)
        : IRequestHandler<UpdateEmployeeCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.EmployeeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Employee update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found.";
                return response;
            }

            mapper.Map(request.EmployeeDto, employee);
            await employeeRepository.UpdateAsync(employee, cancellationToken);

            response.Success = true;
            response.Message = "Employee updated successfully.";
            response.Id = employee.Id;
            return response;
        }
    }
}