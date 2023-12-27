﻿using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Domain.Features.Hotels;

public class Hotel : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Location Location { get; private set; }
    public int CityId { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    private Hotel() { }

    public Hotel(string name, string description, Location location, User user)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.NotDefault(location, "The location is required.", nameof(location));
        Ensure.NotNull(user, "The user is required.", nameof(user));

        Name = name;
        Description = description;
        Location = location;
        UserId = user.Id;
    }

    public Result Update(string name, string description, Location location)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.NotDefault(location, "The location is required.", nameof(location));

        if (Name == name && Description == description && Location == location)
        {
            return DomainErrors.Hotel.NothingToUpdate;
        }

        Name = name;
        Description = description;
        Location = location;

        return Result.Success();
    }

    public void AddRoom(Room room)
    {
        Ensure.NotNull(room, "The room is required.", nameof(room));

        Rooms.Add(room);
    }
}
