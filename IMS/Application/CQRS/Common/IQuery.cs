using MediatR;

namespace Application.CQRS.Common
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}