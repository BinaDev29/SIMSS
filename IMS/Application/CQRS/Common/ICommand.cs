using Application.Responses;
using MediatR;

namespace Application.CQRS.Common
{
    public interface ICommand : IRequest<BaseCommandResponse>
    {
    }

    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}