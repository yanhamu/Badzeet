using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public class UserRepository
    {
        private readonly IDbConnection connection;

        public UserRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        internal async Task<UserDto> GetUser(string username)
        {
            var sql = "select id, username, password from users.users where username = @username";
            var userDto = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { username });
            return userDto;
        }

        internal async Task CreateUser(UserDto userDto)
        {
            var sql = @"insert into users.users values (@id, @username, @password)";
            await connection.ExecuteAsync(sql, new { id = userDto.Id, username = userDto.Username, password = userDto.Password });
        }
    }
}
