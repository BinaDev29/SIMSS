using Application.DTOs.Godown;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Godown.Commands.UpdateGodown
{
    public class UpdateGodownCommand : IRequest<BaseCommandResponse>
    {
        public required UpdateGodownDto GodownDto { get; set; }
    }
}