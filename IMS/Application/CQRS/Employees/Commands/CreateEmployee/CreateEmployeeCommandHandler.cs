using MediatR;
using Application.Contracts;
using Application.DTOs.Employee.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.CQRS.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<CreateEmployeeCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateEmployeeValidator();
            var validationResult = await validator.ValidateAsync(request.EmployeeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Employee creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var employee = mapper.Map<Domain.Models.Employee>(request.EmployeeDto);
            var addedEmployee = await employeeRepository.AddAsync(employee, cancellationToken);

            response.Success = true;
            response.Message = "Employee created successfully.";
            response.Id = addedEmployee.Id;

            return response;
        }
    }
}