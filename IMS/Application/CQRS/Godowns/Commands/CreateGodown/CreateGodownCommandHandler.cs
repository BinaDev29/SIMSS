using MediatR;
using Application.Contracts;
using Application.DTOs.Godown.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Godown.Commands.CreateGodown
{
    public class CreateGodownCommandHandler : IRequestHandler<CreateGodownCommand, BaseCommandResponse>
    {
        private readonly IGodownRepository _godownRepository;
        private readonly IMapper _mapper;

        public CreateGodownCommandHandler(IGodownRepository godownRepository, IMapper mapper)
        {
            _godownRepository = godownRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateGodownValidator();
            var validationResult = await validator.ValidateAsync(request.GodownDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var godown = _mapper.Map<Domain.Models.Godown>(request.GodownDto);
            var addedGodown = await _godownRepository.AddAsync(godown, cancellationToken);

            response.Success = true;
            response.Message = "Godown created successfully.";
            response.Id = addedGodown.Id;

            return response;
        }
    }
}