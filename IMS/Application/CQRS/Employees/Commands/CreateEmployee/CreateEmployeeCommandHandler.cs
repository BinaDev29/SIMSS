using MediatR;
using Application.Contracts;
using Application.DTOs.Employee.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, BaseCommandResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateEmployeeValidator();
            var validationResult = await validator.ValidateAsync(request.EmployeeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Employee creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var employee = _mapper.Map<Domain.Models.Employee>(request.EmployeeDto);
            var addedEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);

            response.Success = true;
            response.Message = "Employee created successfully.";
            response.Id = addedEmployee.Id;

            return response;
        }
    }
}