using Application.Contracts;
using Application.DTOs.Godown.Validators;
using Application.Responses;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Godowns.Commands.CreateGodown
{
    public class CreateGodownCommandHandler(IGodownRepository godownRepository, IMapper mapper)
        : IRequestHandler<CreateGodownCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateGodownValidator();
            var validationResult = await validator.ValidateAsync(request.GodownDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 ተመሳሳይ ስም ወይም ኮድ ያለው መጋዘን መኖሩን ማረጋገጥ
            var godownExists = await godownRepository.GetGodownByNameOrCodeAsync(request.GodownDto.Name, request.GodownDto.Code, cancellationToken);
            if (godownExists != null)
            {
                response.Success = false;
                response.Message = "A godown with this name or code already exists.";
                return response;
            }

            var godown = mapper.Map<Domain.Models.Godown>(request.GodownDto);
            var addedGodown = await godownRepository.AddAsync(godown, cancellationToken);

            response.Success = true;
            response.Message = "Godown created successfully.";
            response.Id = addedGodown.Id;

            return response;
        }
    }
}