﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Model;

namespace Badzeet.Budget.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategories(long accountId);
    Task Create(long accountId, string name, int order);
    Task<Category?> Remove(long categoryId);
    Task<Category?> Get(long categoryId);
    Task Save();
}