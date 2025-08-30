using MediatR;
using Application.DTOs.Godown;
using System.Collections.Generic;

namespace Application.CQRS.Godowns.Queries.GetGodownList
{
    public class GetGodownListQuery : IRequest<List<GodownDto>>
    {
    }
}