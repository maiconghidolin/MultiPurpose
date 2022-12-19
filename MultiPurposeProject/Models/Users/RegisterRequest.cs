namespace MultiPurposeProject.Models.Users;

using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{

    [Required]
    public string Name { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

}

