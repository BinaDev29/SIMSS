using Application.Contracts;
using Application.DTOs.Godown.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Godowns.Commands.CreateGodown
{
    // The handler class implements IRequestHandler interface
    public class CreateGodownCommandHandler(IGodownRepository godownRepository, IMapper mapper)
        : IRequestHandler<CreateGodownCommand, BaseCommandResponse>
    {
        // The Handle method contains the core business logic.
        public async Task<BaseCommandResponse> Handle(CreateGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // 1. Validation: The handler uses the validator to check the incoming data.
            var validator = new CreateGodownValidator();
            var validationResult = await validator.ValidateAsync(request.GodownDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 2. Mapping: The DTO is mapped to the domain model.
            var godown = mapper.Map<Domain.Models.Godown>(request.GodownDto);

            // 3. Repository Call: The domain model is added to the database via the repository.
            var addedGodown = await godownRepository.AddAsync(godown, cancellationToken);

            // 4. Response: A BaseCommandResponse is returned with the result.
            response.Success = true;
            response.Message = "Godown created successfully.";
            response.Id = addedGodown.Id;

            return response;
        }
    }
}