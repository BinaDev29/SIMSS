using Application.DTOs.Godown;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Godown.Commands.CreateGodown
{
    public class CreateGodownCommand : IRequest<BaseCommandResponse>
    {
        public required CreateGodownDto GodownDto { get; set; }
    }
}