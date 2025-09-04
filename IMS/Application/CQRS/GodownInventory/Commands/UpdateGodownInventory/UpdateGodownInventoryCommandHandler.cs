using MediatR;
using Application.Contracts;
using Application.DTOs.GodownInventory.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.GodownInventory.Commands.UpdateGodownInventory
{
    public class UpdateGodownInventoryCommandHandler : IRequestHandler<UpdateGodownInventoryCommand, BaseCommandResponse>
    {
        private readonly IGodownInventoryRepository _godownInventoryRepository;
        private readonly IMapper _mapper;

        public UpdateGodownInventoryCommandHandler(IGodownInventoryRepository godownInventoryRepository, IMapper mapper)
        {
            _godownInventoryRepository = godownInventoryRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateGodownInventoryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateGodownInventoryValidator();
            var validationResult = await validator.ValidateAsync(request.GodownInventoryDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Godown inventory update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var existingInventory = await _godownInventoryRepository.GetByIdAsync(request.GodownInventoryDto.Id, cancellationToken);
            if (existingInventory == null)
            {
                response.Success = false;
                response.Message = "Godown inventory not found.";
                return response;
            }

            _mapper.Map(request.GodownInventoryDto, existingInventory);
            await _godownInventoryRepository.UpdateAsync(existingInventory, cancellationToken);

            response.Success = true;
            response.Message = "Godown inventory updated successfully.";
            return response;
        }
    }
}