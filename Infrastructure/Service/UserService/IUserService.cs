using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Dtos.UserDto;
using Domain.Response;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Service.UserService;

public interface IUserService
{
    Task<Response<List<GetAllUserDto>>> GetAllUserAsync();
    Task<Response<string>> UnBlockedUserAsync(int id);
    Task<Response<string>> BlockedUserAsync(int id);
    Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto model);
    Task<Response<RegisterRequestDto>> RegisterAsync(RegisterRequestDto model);
}