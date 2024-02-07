using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Photos;
using Tap.Contracts.Files;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Photos.UpdatePhoto;

public record UpdatePhotoCommand(int PhotoId, FileRequest File)
    : ICommand<Result<UpdatePhotoResponse>>;
