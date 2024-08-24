using System;

namespace Badzeet.Web.Features.Dashboard;

public class UserViewModel
{
    public UserViewModel(Guid id, string nickname, decimal total)
    {
        Id = id;
        Nickname = nickname;
        Total = total;
    }

    public Guid Id { get; }
    public string Nickname { get; }
    public decimal Total { get; set; }
}