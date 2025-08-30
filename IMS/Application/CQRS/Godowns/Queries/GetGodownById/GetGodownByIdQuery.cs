using MediatR;
using Application.DTOs.Godown;

namespace Application.CQRS.Godowns.Queries.GetGodownById
{
    public class GetGodownByIdQuery : IRequest<GodownDto>
    {
        public required int Id { get; set; }
    }
}