namespace Domain.Dtos.UserDto;

public class GetAllUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string HashPassword { get; set; } = null!;
    public bool IsBlocked { get; set; }  = false;
}