﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Api;

[Route("api/accounts/{accountId:long}/budgets")]
public class BudgetsController : ControllerBase
{
    private readonly IAccountRepository accountRepository;
    private readonly IBudgetCategoryRepository budgetCategoryRepository;
    private readonly IBudgetRepository budgetRepository;

    public BudgetsController(
        IBudgetRepository budgetRepository,
        IAccountRepository accountRepository,
        IBudgetCategoryRepository budgetCategoryRepository)
    {
        this.budgetRepository = budgetRepository;
        this.accountRepository = accountRepository;
        this.budgetCategoryRepository = budgetCategoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get(long accountId)
    {
        var account = (await accountRepository.GetAccount(accountId))!;
        var budgets = await budgetRepository.List(accountId, new Filter());

        var dtos = budgets.Select(x => new BudgetDto
        {
            AccountId = accountId,
            BudgetId = x.BudgetId,
            From = GetStartDate(x.BudgetId, account.FirstDayOfTheBudget),
            To = GetLastDate(x.BudgetId, account.FirstDayOfTheBudget)
        });

        return Ok(dtos);
    }

    [HttpGet("{budgetId:int}")]
    public async Task<IActionResult> Get(long accountId, int budgetId)
    {
        var account = (await accountRepository.GetAccount(accountId))!;
        var budget = await budgetRepository.Get(budgetId, accountId);
        if (budget == null)
            return NotFound();

        var startDate = GetStartDate(budgetId, account.FirstDayOfTheBudget);
        var endDate = GetLastDate(budgetId, account.FirstDayOfTheBudget);

        var budgetCategories = await GetBudgetCategories(budgetId, account.Id);

        return Ok(new BudgetDto
        {
            AccountId = account.Id,
            BudgetId = budgetId,
            From = startDate,
            To = endDate,
            BudgetCategories = budgetCategories.ToList()
        });
    }

    private async Task<IEnumerable<BudgetCategoryDto>> GetBudgetCategories(int budgetId, long accountId)
    {
        var budgetCategories = await budgetCategoryRepository.GetBudgetCategories(budgetId, accountId);
        return budgetCategories.Select(x => new BudgetCategoryDto
        {
            CategoryId = x.CategoryId,
            Amount = x.Amount
        });
    }

    private DateTime GetStartDate(int budgetId, int firstDay)
    {
        return DateTime.ParseExact(budgetId + firstDay.ToString("D2"), "yyyyMMdd", CultureInfo.InvariantCulture);
    }

    private DateTime GetLastDate(int budgetId, int firstDay)
    {
        var startDate = GetStartDate(budgetId, firstDay);
        return startDate.AddMonths(1).AddTicks(-1);
    }

    public class BudgetDto
    {
        public long AccountId { get; set; }
        public int BudgetId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<BudgetCategoryDto> BudgetCategories { get; set; } = new();
    }

    public class BudgetCategoryDto
    {
        public long CategoryId { get; internal set; }
        public decimal Amount { get; internal set; }
    }
}