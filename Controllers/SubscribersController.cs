

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SotovayaSvyas.Models;
using SotovayaSvyas.Services;
using SotovayaSvyas.ViewModels;
using SotovayaSvyas.ViewModels.FilterViewModels;
using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.Controllers
{
    [Authorize]
    public class SubscribersController : Controller
    {
        private readonly SubscriberService _cache;
        private readonly int _pageSize = 10;
        public SubscribersController(SubscriberService cache)
        {
            _cache = cache;
        }

        // GET: Subscribers
        public async Task<IActionResult> Index(SubscriberFilterViewModel filter, int page = 1, SubscriberSortState sortOrder = SubscriberSortState.AddressAsc)
        {
            var subscribers = await _cache.GetAll();//.Subscribers.Include(s => s.ServicesProvideds).Include(s => s.TariffPlans).Include(s => s.Treatys);
            
            //Фильтрация
            if (!String.IsNullOrEmpty(filter.AddressFind))
                subscribers = subscribers.Where(e => e.Address.Contains(filter.AddressFind)).ToList();
            if (!String.IsNullOrEmpty(filter.PassportDetailsFind))
                subscribers = subscribers.Where(e => e.PassportDetails.Contains(filter.PassportDetailsFind)).ToList();
            if (!String.IsNullOrEmpty(filter.SurnameFind))
                subscribers = subscribers.Where(e => e.Surname.Contains(filter.SurnameFind)).ToList();
            if (!String.IsNullOrEmpty(filter.NameFind))
                subscribers = subscribers.Where(e => e.Name.Contains(filter.NameFind)).ToList();
            if (!String.IsNullOrEmpty(filter.LastnameFind))
                subscribers = subscribers.Where(e => e.Lastname.Contains(filter.LastnameFind)).ToList();

            //сортировка
            switch (sortOrder)
            {
                case SubscriberSortState.SurnameAsc:
                    subscribers = subscribers.OrderBy(e => e.Surname).ToList();/**/
                    break;
                case SubscriberSortState.SurnameDesc:
                    subscribers = subscribers.OrderByDescending(e => e.Surname).ToList(); /**/
                    break;
                case SubscriberSortState.NameAsc:
                    subscribers = subscribers.OrderBy(e => e.Name).ToList();/**/
                    break;
                case SubscriberSortState.NameDesc:
                    subscribers = subscribers.OrderByDescending(e => e.Name).ToList(); /**/
                    break;
                case SubscriberSortState.LastnameAsc:
                    subscribers = subscribers.OrderBy(e => e.Lastname).ToList();/*we*/
                    break;
                case SubscriberSortState.LastnameDesc:
                    subscribers = subscribers.OrderByDescending(e => e.Lastname).ToList(); /**/
                    break;
                case SubscriberSortState.PassportDetailsAsc:
                    subscribers = subscribers.OrderBy(e => e.PassportDetails).ToList();
                    break;
                case SubscriberSortState.PassportDetailsDesc:
                    subscribers = subscribers.OrderByDescending(e => e.PassportDetails).ToList();
                    break;
                case SubscriberSortState.AddressAsc:
                    subscribers = subscribers.OrderBy(e => e.Address).ToList();
                    break;
                case SubscriberSortState.AddressDesc:
                    subscribers = subscribers.OrderByDescending(e => e.Address).ToList();
                    break;
            }

            //Пагинация
            int count = subscribers.Count();
            subscribers = subscribers.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель представления
            SubscribersViewModel viewModel = new()
            {
                Subscribers = subscribers,
                PageViewModel = new(page, count, _pageSize),
                SortOrder = new(sortOrder),
                Filter = filter
            };

            return View(viewModel);
        }

        // GET: Subscribers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _cache.Get((int)id);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        // GET: Subscribers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubscriberId,Surname,Name,Lastname,Address,PassportDetails")] Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                await _cache.Add(subscriber);
                return RedirectToAction(nameof(Index));
            }
            return View(subscriber);
        }

        // GET: Subscribers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _cache.Get((int)id);
            if (subscriber == null)
            {
                return NotFound();
            }
            return View(subscriber);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubscriberId,Surname,Name,Lastname,Address,PassportDetails")] Subscriber subscriber)
        {
            if (id != subscriber.SubscriberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _cache.Update(subscriber);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriberExists(subscriber.SubscriberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subscriber);
        }

        // GET: Subscribers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _cache.Get((int)id);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscriber = await _cache.Get((int)id);
            if (subscriber != null)
            {
               await _cache.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SubscriberExists(int id)
        {
            return _cache.GetAll().Result.Any(e => e.SubscriberId == id);
        }
    }
}
