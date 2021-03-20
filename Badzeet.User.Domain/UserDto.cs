using System;

namespace Badzeet.User.Domain
{
    internal class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}