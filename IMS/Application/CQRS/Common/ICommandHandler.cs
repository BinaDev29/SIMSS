using Application.Responses;
using MediatR;

namespace Application.CQRS.Common
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, BaseCommandResponse>
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}