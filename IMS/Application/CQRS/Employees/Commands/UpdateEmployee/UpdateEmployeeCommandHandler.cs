using MediatR;
using Application.Contracts;
using Application.DTOs.Employee.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Employee.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, BaseCommandResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateEmployeeValidator();
            var validationResult = await validator.ValidateAsync(request.EmployeeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Employee update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeDto.Id, cancellationToken);
            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found.";
                return response;
            }

            _mapper.Map(request.EmployeeDto, employee);
            await _employeeRepository.UpdateAsync(employee, cancellationToken);

            response.Success = true;
            response.Message = "Employee updated successfully.";
            return response;
        }
    }
}