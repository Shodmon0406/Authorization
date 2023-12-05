using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.LoginDto;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Username is required!")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Username is required!")]
    public string Password { get; set; }
}