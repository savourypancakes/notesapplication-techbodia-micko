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

    public AuthService(IConfiguration configuration, IDbConnection dbConnection)
    {
        config = configuration;
        connect = dbConnection;
    }

    public async Task<bool> Register(string username, string password)
    {
        try
        {
            // Your existing check for username...
            var existsSql = "SELECT 1 FROM \"User\" WHERE Username = @Username";
            var exists = await connect.QueryFirstOrDefaultAsync<int?>(existsSql, new { Username = username });

            if (exists.HasValue)
            {
                return false;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var insertSql = "INSERT INTO \"User\" (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            await connect.ExecuteAsync(insertSql, new { Username = username, PasswordHash = passwordHash });
            return true;
        }
        catch (Exception ex)
        {
            // Log exception to console or a file for debugging
            Console.Error.WriteLine($"Register error: {ex.Message}\n{ex.StackTrace}");
            // Optionally: throw; // to let it bubble up if you want to see it in logs
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
}

