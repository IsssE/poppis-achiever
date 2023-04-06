using System.ComponentModel.DataAnnotations;

namespace User.Models;

public class User {
    [Key]
    public required string UserId {get; set;} = string.Empty;

    public string DisplayName {get; set;} = string.Empty;
}