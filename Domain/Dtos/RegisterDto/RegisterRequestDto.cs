using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Domain.Dtos.RegisterDto;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "FirstName is required!")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "LastName is required!")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Phone is required!")]
    public string Phone { get; set; } = null!;
    [Required(ErrorMessage = "UserName is required!")]
    public string UserName { get; set; } = null!;
    [Required(ErrorMessage = "Password is required!")]
    public string Password { get; set; }
}