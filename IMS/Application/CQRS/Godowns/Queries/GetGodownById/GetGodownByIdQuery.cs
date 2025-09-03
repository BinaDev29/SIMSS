using Application.DTOs.Godown;
using MediatR;

namespace Application.CQRS.Godown.Queries.GetGodownById
{
    public class GetGodownByIdQuery : IRequest<GodownDto?>
    {
        public required int Id { get; set; }
    }
}