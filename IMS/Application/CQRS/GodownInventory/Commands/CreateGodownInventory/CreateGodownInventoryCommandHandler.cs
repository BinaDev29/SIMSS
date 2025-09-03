using MediatR;
using Application.Contracts;
using Application.DTOs.GodownInventory.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.GodownInventory.Commands.CreateGodownInventory
{
    public class CreateGodownInventoryCommandHandler : IRequestHandler<CreateGodownInventoryCommand, BaseCommandResponse>
    {
        private readonly IGodownInventoryRepository _godownInventoryRepository;
        private readonly IMapper _mapper;

        public CreateGodownInventoryCommandHandler(IGodownInventoryRepository godownInventoryRepository, IMapper mapper)
        {
            _godownInventoryRepository = godownInventoryRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateGodownInventoryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateGodownInventoryValidator();
            var validationResult = await validator.ValidateAsync(request.GodownInventoryDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown inventory creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var godownInventory = _mapper.Map<Domain.Models.GodownInventory>(request.GodownInventoryDto);
            var addedInventory = await _godownInventoryRepository.AddAsync(godownInventory, cancellationToken);

            response.Success = true;
            response.Message = "Godown inventory created successfully.";
            response.Id = addedInventory.Id;

            return response;
        }
    }
}