﻿using Badzeet.Domain.Book.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories(long bookId);
    }
}
