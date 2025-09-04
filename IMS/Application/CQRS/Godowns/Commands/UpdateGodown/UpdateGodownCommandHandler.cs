using MediatR;
using Application.Contracts;
using Application.DTOs.Godown.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Godown.Commands.UpdateGodown
{
    public class UpdateGodownCommandHandler : IRequestHandler<UpdateGodownCommand, BaseCommandResponse>
    {
        private readonly IGodownRepository _godownRepository;
        private readonly IMapper _mapper;

        public UpdateGodownCommandHandler(IGodownRepository godownRepository, IMapper mapper)
        {
            _godownRepository = godownRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateGodownValidator();
            var validationResult = await validator.ValidateAsync(request.GodownDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var godown = await _godownRepository.GetByIdAsync(request.GodownDto.Id, cancellationToken);
            if (godown == null)
            {
                response.Success = false;
                response.Message = "Godown not found.";
                return response;
            }

            _mapper.Map(request.GodownDto, godown);
            await _godownRepository.UpdateAsync(godown, cancellationToken);

            response.Success = true;
            response.Message = "Godown updated successfully.";
            return response;
        }
    }
}