using Application.DTOs.Godown;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Godowns.Commands.UpdateGodown
{
    public class UpdateGodownCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateGodownDto GodownDto { get; set; }
    }
}