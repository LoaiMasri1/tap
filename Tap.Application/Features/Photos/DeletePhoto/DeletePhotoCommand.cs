using MediatR;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Photos.DeletePhoto;

public record DeletePhotoCommand(int PhotoId) : IRequest<Result>;
