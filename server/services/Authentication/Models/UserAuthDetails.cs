using System.ComponentModel.DataAnnotations;

namespace Authentication.Models;

public class UserAuthDetails
{
    [Required]
    [Key]
    public string UserId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
