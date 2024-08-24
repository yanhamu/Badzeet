using System;

namespace Badzeet.Budget.Domain.Model;

public class User
{
    public Guid Id { get; set; }
    public string Nickname { get; set; } = default!;
}