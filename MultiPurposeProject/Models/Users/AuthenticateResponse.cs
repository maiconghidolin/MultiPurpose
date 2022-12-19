namespace MultiPurposeProject.Models.Users;

using System.ComponentModel.DataAnnotations;

public class AuthenticateResponse
{
   
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }

}

