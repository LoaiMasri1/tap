using MediatR;

namespace Tap.Application.Core.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
