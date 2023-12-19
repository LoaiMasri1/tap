using MediatR;
using Tap.Contracts.Features.Photos;
using Tap.Contracts.Files;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Photos.UploadPhoto;

public record UploadPhotosCommand(int ItemId, ItemType ItemType, FileRequest[] Files)
    : IRequest<Result<UploadPhotosResponse>>;
