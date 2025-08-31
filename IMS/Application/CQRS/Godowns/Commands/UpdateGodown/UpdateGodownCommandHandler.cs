using Application.Contracts;
using Application.DTOs.Godown;
using Application.DTOs.Godown.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Godowns.Commands.UpdateGodown
{
    public class UpdateGodownCommandHandler(IGodownRepository godownRepository, IMapper mapper, IValidator<UpdateGodownDto> validator)
        : IRequestHandler<UpdateGodownCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.GodownDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var godown = await godownRepository.GetByIdAsync(request.Id, cancellationToken);
            if (godown == null)
            {
                response.Success = false;
                response.Message = "Godown not found.";
                return response;
            }

            mapper.Map(request.GodownDto, godown);
            await godownRepository.Update(godown, cancellationToken);

            response.Success = true;
            response.Message = "Godown updated successfully.";
            response.Id = godown.Id;
            return response;
        }
    }
}