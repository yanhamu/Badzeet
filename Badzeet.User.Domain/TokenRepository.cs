using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public class TokenRepository
    {
        private readonly IDbConnection connection;

        public TokenRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        internal Task Save(Guid userId, byte[] token, DateTime expiration)
        {
            var tokenId = Guid.NewGuid();
            return connection.ExecuteAsync("insert into users.tokens values(@tokenId, @userId, @token, @expiration)", new { tokenId, userId, token, expiration });
        }

        internal async Task<IEnumerable<TokenDto>> Get(Guid userId)
        {
            return await connection.QueryAsync<TokenDto>("select id, user_id, token from users.tokens where user_id = @userId and expires > @expiration",
                new
                {
                    userId,
                    expiration = DateTime.UtcNow
                });
        }

        internal async Task Remove(Guid id)
        {
            await connection.ExecuteAsync("delete from users.tokens where id = @id", new { id = id });
        }
    }
    internal class TokenDto
    {
        public Guid Id { get; set; }
        public byte[] Token { get; set; }
        public Guid UserId { get; set; }

        public bool IsSame(byte[] other)
        {
            if (other.Length != this.Token.Length)
                return false;

            for (int i = 0; i < Token.Length; i++)
            {
                if (this.Token[i] != other[i])
                    return false;
            }

            return true;
        }
    }
}