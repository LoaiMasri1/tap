using MediatR;

namespace Tap.Application.Core.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse> { }
