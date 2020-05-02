using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;
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

        public ScheduledPaymentsController(
            Service service, 
            ICategoryRepository categoryRepository,
            IUserAccountRepository userAccountRepository)
        {
            this.service = service;
            this.categoryRepository = categoryRepository;
            this.userAccountRepository = userAccountRepository;
        }

        public async Task<IActionResult> List(long accountId)
        {
            var categories = (await categoryRepository.GetCategories(accountId)).Select(c => new CategoryViewModel(c.Id, c.Name));
            var users = (await userAccountRepository.GetUsers(accountId)).Select(u => new UserViewModel(u.UserId, u.User.Nickname));
            var payments = await service.List(accountId);
            return View(new PaymentsListViewModel(payments, categories, users));
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            throw new NotImplementedException(); // TODO
        }

        [HttpPost]
        public IActionResult Edit()
        {
            throw new NotImplementedException();// TODO
        }

        public IActionResult New()
        {
            throw new NotImplementedException();// TODO
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