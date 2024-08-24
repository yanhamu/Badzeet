using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Badzeet.User.Domain;

public class UserRepository
{
    private readonly IDbConnection connection;

    public UserRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    internal async Task<UserDto?> GetUser(string username)
    {
        var sql = "select id, username, password from u_users where username = @username";
        return await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { username });
    }

    internal async Task CreateUser(UserDto userDto)
    {
        var sql = @"insert into u_users values (@id, @username, @password)";
        await connection.ExecuteAsync(sql, new { id = userDto.Id, username = userDto.Username, password = userDto.Password });
    }
}