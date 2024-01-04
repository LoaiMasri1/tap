using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Hotels.GetHotelById;
using Tap.Application.Features.Hotels.SearchHotel;
using Tap.Application.Features.Hotels.UpdateHotel;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Application.Features.Rooms.CreateRoom;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Hotels;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Represents a controller for managing hotels.
/// </summary>
[Authorize]
public class HotelController : ApiController
{
    /// <summary>
    /// Uploads photos for a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The photos were uploaded successfully.</response>
    /// <response code="400">The photos were not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
    [HttpPost(ApiRoutes.Hotel.UploadPhotos)]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.Hotel, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Updates a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="request">The update request.</param>
    /// <response code="200">The hotel was updated successfully.</response>
    /// <response code="400">The hotel was not updated successfully.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Hotel.Update)]
    public async Task<IActionResult> Update(int id, UpdateHotelRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new UpdateHotelCommand(
                        x.id,
                        x.request.Name,
                        x.request.Description,
                        x.request.Latitude,
                        x.request.Longitude
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Creates amenities for a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="request">The create amenity request.</param>
    /// <response code="200">The amenities were created successfully.</response>
    /// <response code="400">The amenities were not created successfully.</response>
    /// <returns>The result of the create amenity operation.</returns>
    [HttpPost(ApiRoutes.Hotel.CreateAmenities)]
    public async Task<IActionResult> CreateAmenities(int id, CreateAmenityRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.TypeId, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateAmenityCommand(
                        x.request.Name,
                        x.request.Description,
                        AmenityType.Hotel,
                        x.request.TypeId
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Creates a room for a hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="request">The create room request.</param>
    /// <response code="200">The room was created successfully.</response>
    /// <response code="400">The room was not created successfully.</response>
    /// <returns>The result of the create room operation.</returns>
    [HttpPost(ApiRoutes.Hotel.CreateRoom)]
    [Authorize]
    public async Task<IActionResult> CreateRoom(int id, CreateRoomRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.HotelId, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateRoomCommand(
                        x.request.HotelId,
                        x.request.Number,
                        x.request.Price,
                        x.request.Currency,
                        x.request.Type,
                        x.request.CapacityOfAdults,
                        x.request.CapacityOfChildren
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Gets a list of hotels based on the specified criteria.
    /// </summary>
    /// <param name="city">The city to search for hotels.</param>
    /// <param name="rating">The minimum rating of the hotels.</param>
    /// <param name="numberOfAvailableRooms">The number of available rooms in the hotels.</param>
    /// <param name="filterBy">The field to filter the hotels by.</param>
    /// <param name="filterQuery">The query to filter the hotels.</param>
    /// <param name="sortBy">The field to sort the hotels by.</param>
    /// <param name="sortOrder">The sort order of the hotels.</param>
    /// <param name="pageNumber">The page number of the hotels.</param>
    /// <param name="pageSize">The page size of the hotels.</param>
    /// <response code="200">The list of hotels was retrieved successfully.</response>
    /// <response code="400">The list of hotels was not retrieved successfully.</response>
    /// <returns>The list of hotels based on the specified criteria.</returns>
    [HttpGet(ApiRoutes.Hotel.Get)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        string? city,
        int? numberOfAvailableRooms,
        string? filterBy,
        string? filterQuery,
        string sortBy = "name",
        string sortOrder = "asc",
        int pageNumber = 1,
        int pageSize = 10
    ) =>
        await Maybe<SearchHotelsQuery>
            .From(
                new SearchHotelsQuery(
                    city,
                    numberOfAvailableRooms,
                    pageSize,
                    pageNumber,
                    sortBy,
                    sortOrder,
                    filterBy,
                    filterQuery
                )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Gets a hotel by ID.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="includeRooms">Indicates whether to include the rooms of the hotel.</param>
    /// <response code="200">The hotel with the specified ID was retrieved successfully.</response>
    /// <response code="400">The hotel with the specified ID was not retrieved successfully.</response>
    /// <returns>The hotel with the specified ID.</returns>
    [HttpGet(ApiRoutes.Hotel.GetById)]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, bool includeRooms = false) =>
        await Maybe<GetHotelByIdQuery>
            .From(new GetHotelByIdQuery(id, includeRooms))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
