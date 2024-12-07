using System.ComponentModel.DataAnnotations;

namespace Badzeet.Web.Features.Account;

public class UserCredentialsModel
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Password { get; set; }
}