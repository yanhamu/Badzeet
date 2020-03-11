using System;

namespace Badzeet.Domain.Book.Model
{
    public class UserBook
    {
        public Guid UserId { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
        public string Nickname { get; set; }
    }
}
