using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;
using Badzeet.Scheduler.Domain.Processors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.ScheduledPayments
{
    [Authorize]
    public class ScheduledPaymentsController : Controller
    {
        private readonly Service service;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly Badzeet.Scheduler.Domain.Interfaces.IPaymentRepository paymentRepository;

        public ScheduledPaymentsController(
            Service service,
            ICategoryRepository categoryRepository,
            IUserAccountRepository userAccountRepository,
            Badzeet.Scheduler.Domain.Interfaces.IPaymentRepository paymentRepository)
        {
            this.service = service;
            this.categoryRepository = categoryRepository;
            this.userAccountRepository = userAccountRepository;
            this.paymentRepository = paymentRepository;
        }

        public async Task<IActionResult> List(long accountId)
        {
            var categories = (await categoryRepository.GetCategories(accountId)).Select(c => new CategoryViewModel(c.Id, c.Name));
            var users = (await userAccountRepository.GetUsers(accountId)).Select(u => new UserViewModel(u.UserId, u.User.Nickname));
            var payments = await service.List(accountId);
            return View(new PaymentsListViewModel(payments, categories, users));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id, long accountId)
        {
            var categories = (await categoryRepository.GetCategories(accountId)).Select(c => new CategoryViewModel(c.Id, c.Name));
            var users = (await userAccountRepository.GetUsers(accountId)).Select(u => new UserViewModel(u.UserId, u.User.Nickname));
            var payment = await paymentRepository.Get(id);
            var metadata = MonthlyPaymentProcessor.MonthlySettings.Parse(payment.Metadata);

            return View(new MonthlyPaymentViewModel() { Users = users, Categories = categories, Payment = payment, Day = metadata.Day, LastDay = metadata.LastDay, When = metadata.When });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long accountId, MonthlyPaymentViewModel model)
        {
            //TODO add edit view
            var metadata = GetSettings(model);
            var scheduledAt = CalculateFirstScheduledDate(metadata);
            var payment = await paymentRepository.Get(model.Payment.Id);
            payment.Amount = model.Payment.Amount;
            payment.CategoryId = model.Payment.CategoryId;
            payment.Description = model.Payment.Description;
            payment.Metadata = metadata.Serialize();
            payment.OwnerId = model.Payment.OwnerId;
            payment.ScheduledAt = scheduledAt;
            payment.SchedulingType = SchedulingType.Monthly;
            payment.UpdatedAt = DateTime.UtcNow;
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> New(long accountId)
        {
            var categories = (await categoryRepository.GetCategories(accountId)).Select(c => new CategoryViewModel(c.Id, c.Name));
            var users = (await userAccountRepository.GetUsers(accountId)).Select(u => new UserViewModel(u.UserId, u.User.Nickname));
            return View(new MonthlyPaymentViewModel() { Users = users, Categories = categories });
        }

        [HttpPost]
        public async Task<IActionResult> New(long accountId, MonthlyPaymentViewModel model)
        {
            var metadata = GetSettings(model);
            var scheduledAt = CalculateFirstScheduledDate(metadata);
            paymentRepository.Add(new Payment()
            {
                AccountId = accountId,
                Amount = model.Payment.Amount,
                CategoryId = model.Payment.CategoryId,
                Description = model.Payment.Description,
                Metadata = metadata.Serialize(),
                OwnerId = model.Payment.OwnerId,
                ScheduledAt = scheduledAt,
                SchedulingType = SchedulingType.Monthly,
                UpdatedAt = DateTime.Now
            });
            await paymentRepository.SaveAll();

            return RedirectToAction(nameof(List));
        }

        private MonthlyPaymentProcessor.MonthlySettings GetSettings(MonthlyPaymentViewModel model)
        {
            return model.LastDay
                ? MonthlyPaymentProcessor.MonthlySettings.CreateLastDayOfTheMonth(model.When)
                : MonthlyPaymentProcessor.MonthlySettings.CreateFixedDay(model.Day.Value, model.When);
        }

        private DateTime CalculateFirstScheduledDate(MonthlyPaymentProcessor.MonthlySettings metadata)
        {
            var scheduledDate = DateTime.UtcNow;

            if (metadata.LastDay)
            {
                while (scheduledDate.Month == scheduledDate.Month)
                    scheduledDate = scheduledDate.AddDays(1);
                scheduledDate = scheduledDate.AddDays(-1);
                return new DateTime(scheduledDate.Year, scheduledDate.Month, scheduledDate.Day).Add(metadata.When);
            }
            else
            {
                while (scheduledDate.Day != metadata.Day.Value)
                    scheduledDate = scheduledDate.AddDays(1);
                return new DateTime(scheduledDate.Year, scheduledDate.Month, scheduledDate.Day).Add(metadata.When);
            }
        }

        public class MonthlyPaymentViewModel
        {
            public bool LastDay { get; set; }
            public int? Day { get; set; }
            public TimeSpan When { get; set; }
            public Payment Payment { get; set; }
            public IEnumerable<UserViewModel> Users { get; set; }
            public IEnumerable<CategoryViewModel> Categories { get; set; }
        }

        public class PaymentsListViewModel
        {
            public PaymentsListViewModel(IEnumerable<Payment> payments, IEnumerable<CategoryViewModel> categories, IEnumerable<UserViewModel> users)
            {
                Payments = payments;
                Categories = categories;
                Users = users;
            }

            public IEnumerable<Payment> Payments { get; set; }
            public IEnumerable<CategoryViewModel> Categories { get; set; }
            public IEnumerable<UserViewModel> Users { get; set; }

        }

        public class CategoryViewModel
        {
            public CategoryViewModel(long id, string name)
            {
                Id = id;
                Name = name;
            }

            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class UserViewModel
        {
            public UserViewModel(Guid id, string nickname)
            {
                Id = id;
                Nickname = nickname;
            }

            public Guid Id { get; set; }
            public string Nickname { get; set; }
        }
    }
}