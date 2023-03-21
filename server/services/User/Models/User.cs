using System.ComponentModel.DataAnnotations;

namespace User.Models;

public class User {
    [Key]
    public required string Id {get; set;} = string.Empty;

    public string Name {get; set;} = string.Empty;
}