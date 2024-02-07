using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Photos.DeletePhoto;

public record DeletePhotoCommand(int PhotoId) : ICommand<Result>;
