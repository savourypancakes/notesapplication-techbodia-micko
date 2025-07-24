using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using Dapper;
using Microsoft.IdentityModel.Tokens;
public class AuthService : IAuthService
{
    private readonly IConfiguration config;
    private readonly IDbConnection connect;

    public AuthService(IConfiguration cg, IDbConnection ct)
    {
        config = cg;
        connect = ct;
    }

    public async Task<bool> Register(string username, string password)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var sql = "INSERT INTO [User] (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
        try
        {
            await connect.ExecuteAsync(sql, new { Username = username, PasswordHash = passwordHash });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> Login(string username, string password)
    {
        var sql = "SELECT * FROM [User] WHERE Username = @Username";
        var user = await connect.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: new[] { new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()) },
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    internal bool Register(object username, object password)
    {
        throw new NotImplementedException();
    }
}
