using Application.DTOs.Godown;
using MediatR;

namespace Application.CQRS.Godown.Queries.GetGodownList
{
    public class GetGodownListQuery : IRequest<List<GodownDto>>
    {
        // Add filtering, paging, sorting parameters if needed
    }
}