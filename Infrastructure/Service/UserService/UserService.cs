using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Dtos.UserDto;
using Domain.Entity;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service.UserService;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private string secretKey;
    public UserService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        secretKey = configuration.GetConnectionString("JwtKey") ?? Guid.NewGuid().ToString();
    }

    public async Task<Response<List<GetAllUserDto>>> GetAllUserAsync()
    {
        var users = await _context.Users.Select(u => new GetAllUserDto()
        {
            Id = u.Id,
            FullName = string.Concat(u.FirstName + " " + u.LastName),
            UserName = u.UserName,
            Email = u.Email,
            Phone = u.Phone,
            IsBlocked = u.IsBlocked
        }).ToListAsync();
        if (users.Count == 0) return new Response<List<GetAllUserDto>>(HttpStatusCode.NoContent, "No content");
        return new Response<List<GetAllUserDto>>(HttpStatusCode.OK, "All data", users);
    }

    public async Task<Response<string>> UnBlockedUserAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return new Response<string>(HttpStatusCode.NotFound, "User not found!");
        user.IsBlocked = false;
        await _context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "User successfully unblocked");
    }

    public async Task<Response<string>> BlockedUserAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return new Response<string>(HttpStatusCode.NotFound, "User not found!");
        user.IsBlocked = false;
        await _context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK, "User successfully blocked");
    }

    public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.UserName.ToLower().Trim() == model.Username.ToLower().Trim());
        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.HashPassword) || user == null)
            return new Response<LoginResponseDto>(HttpStatusCode.BadRequest, "Username or password is incorrect");
        return await GenerateJWT_Token(user);
    }

    public async Task<Response<RegisterRequestDto>> RegisterAsync(RegisterRequestDto model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.UserName.ToLower().Trim() == model.UserName.ToLower().Trim());
        if (user != null) return new Response<RegisterRequestDto>(HttpStatusCode.NotFound, "User already exist!");
        if (model.Password.Length < 4 || model.Password.Length > 45)
            return new Response<RegisterRequestDto>(HttpStatusCode.BadRequest,
                "The password length should be in the range from 4 to 45!");
        var newUser = new User()
        {
            IsBlocked = false,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Phone = model.Phone
        };
        await _context.Users.AddAsync(newUser);
        var resultOfRegister = await _context.SaveChangesAsync();
        if (resultOfRegister == 0)
            return new Response<RegisterRequestDto>(HttpStatusCode.Unauthorized, "User not authorised!");
        return new Response<RegisterRequestDto>(HttpStatusCode.OK, "User successfully authorised");
    }

    private async Task<Response<LoginResponseDto>> GenerateJWT_Token(User user)
    {

        return await Task.Run(() =>
        {
            var claims = new List<Claim>
            {
                new Claim("UserId",user.Id.ToString()),
                new Claim("UserName",user.UserName),
                new Claim("Email",user.Email),
                new Claim(ClaimTypes.MobilePhone,user.Phone)
            };


            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var createToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var writeToken = jwtTokenHandler.WriteToken(createToken);

            var response = new LoginResponseDto
            {
                JwtToken = writeToken
            };
            return new Response<LoginResponseDto>(System.Net.HttpStatusCode.OK, "Success", response);
        });
    }
}