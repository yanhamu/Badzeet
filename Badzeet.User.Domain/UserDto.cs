using System;

namespace Badzeet.User.Domain;

internal class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}