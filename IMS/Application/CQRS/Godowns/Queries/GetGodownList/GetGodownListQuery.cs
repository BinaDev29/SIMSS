using MediatR;
using Application.DTOs.Godown;
using Application.DTOs.Common;

namespace Application.CQRS.Godowns.Queries.GetGodownList
{
    public class GetGodownListQuery : IRequest<List<GodownDto>>
    {
        public GodownQueryParameters? Parameters { get; set; }
    }
}