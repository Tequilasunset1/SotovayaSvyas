using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SotovayaSvyas.Data;
using SotovayaSvyas.Models;
using SotovayaSvyas.Services;
using SotovayaSvyas.ViewModels;
using SotovayaSvyas.ViewModels.FilterViewModels;
using SotovayaSvyas.ViewModels.SortStates;

namespace SotovayaSvyas.Controllers
{
    [Authorize]
    public class TreatiesController : Controller
    {
        private readonly TreatyService _cache;
        private readonly SubscriberService _subscriberCache;
        private readonly TariffPlanService _tariffPlanCache;
        private readonly int _pageSize = 10;

        public TreatiesController(TreatyService cache, SubscriberService subscriberCache, TariffPlanService tariffPlanCache)
        {
            _cache = cache;
            _subscriberCache = subscriberCache;
            _tariffPlanCache = tariffPlanCache;
        }

        // GET: Treaties
        public async Task<IActionResult> Index(TreatyFilterViewModel filter, int page = 1, TreatySortState sortOrder = TreatySortState.SubscriberAsc)
        {
            var treaties = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(filter.SubscriberFind))
                treaties = treaties.Where(e => $"{e.Subscriber.Surname} {e.Subscriber.Name} {e.Subscriber.Lastname}".Contains(filter.SubscriberFind)).ToList();
            if (!String.IsNullOrEmpty(filter.PhoneNumberFind))
                treaties = treaties.Where(e => e.PhoneNumber.Contains(filter.PhoneNumberFind)).ToList();
            if (!String.IsNullOrEmpty(filter.TariffFind))
                treaties = treaties.Where(e => e.TariffPlan.TariffName.Contains(filter.TariffFind)).ToList();
            if (!String.IsNullOrEmpty(filter.SurnameFind))
                treaties = treaties.Where(e => e.Surname.Contains(filter.SurnameFind)).ToList();
            if (!String.IsNullOrEmpty(filter.NameFind))
                treaties = treaties.Where(e => e.Name.Contains(filter.NameFind)).ToList();
            if (!String.IsNullOrEmpty(filter.LastnameFind))
                treaties = treaties.Where(e => e.Lastname.Contains(filter.LastnameFind)).ToList();
            if (filter.DateConclusionFind != null)
                treaties = treaties.Where(e => e.DateConclusion == filter.DateConclusionFind).ToList();

            //сортировка
            switch (sortOrder)
            {
                case TreatySortState.SubscriberAsc:
                    treaties = treaties.OrderBy(e => e.Subscriber.Surname).ToList();
                    break;
                case TreatySortState.SubscriberDesc:
                    treaties = treaties.OrderByDescending(e => e.Subscriber.Surname).ToList();
                    break;
                case TreatySortState.DateConclusionAsc:
                    treaties = treaties.OrderBy(e => e.DateConclusion).ToList();
                    break;
                case TreatySortState.DateConclusionDesc:
                    treaties = treaties.OrderByDescending(e => e.DateConclusion).ToList();
                    break;
                case TreatySortState.SurnameAsc:
                    treaties = treaties.OrderBy(e => e.Surname).ToList();
                    break;
                case TreatySortState.SurnameDesc:
                    treaties = treaties.OrderByDescending(e => e.Surname).ToList();
                    break;
                case TreatySortState.NameAsc:
                    treaties = treaties.OrderBy(e => e.Name).ToList();
                    break;
                case TreatySortState.NameDesc:
                    treaties = treaties.OrderByDescending(e => e.Name).ToList();
                    break;
                case TreatySortState.LastnameAsc:
                    treaties = treaties.OrderBy(e => e.Lastname).ToList();
                    break;
                case TreatySortState.LastnameDesc:
                    treaties = treaties.OrderByDescending(e => e.Lastname).ToList();
                    break;
                case TreatySortState.TariffAsc:
                    treaties = treaties.OrderBy(e => e.TariffPlan.TariffName).ToList();
                    break;
                case TreatySortState.TariffDesc:
                    treaties = treaties.OrderByDescending(e => e.TariffPlan.TariffName).ToList();
                    break;
            }

            //Пагинация
            int count = treaties.Count();
            treaties = treaties.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель представления
            TreatiesViewModel viewModel = new()
            {
                Treaties = treaties,
                PageViewModel = new(page, count, _pageSize),
                Sort = new(sortOrder),
                Filter = filter
            };

            return View(viewModel);
        }

        // GET: Treaties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treaty = await _cache.Get((int)id);
            if (treaty == null)
            {
                return NotFound();
            }

            return View(treaty);
        }

        // GET: Treaties/Create
        public IActionResult Create()
        {
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio");
            ViewData["TariffPlanId"] = new SelectList(_tariffPlanCache.GetAll().Result, "TariffPlanId", "TariffName");
            return View();
        }

        // POST: Treaties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TreatyId,SubscriberId,DateConclusion,TariffPlanId,PhoneNumber,Surname,Name,Lastname")] Treaty treaty)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(treaty.ToString());
                await _cache.Add(treaty);
                return RedirectToAction(nameof(Index));
            }
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio", treaty.SubscriberId);
            ViewData["TariffPlanId"] = new SelectList(_tariffPlanCache.GetAll().Result, "TariffPlanId", "TariffName", treaty.TariffPlanId);

            return View(treaty);
        }

        // GET: Treaties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treaty = await _cache.Get((int)id);
            if (treaty == null)
            {
                return NotFound();
            }
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio");
            ViewData["TariffPlanId"] = new SelectList(_tariffPlanCache.GetAll().Result, "TariffPlanId", "TariffName");

            return View(treaty);
        }

        // POST: Treaties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatyId,SubscriberId,DateConclusion,TariffPlanId,PhoneNumber,Surname,Name,Lastname")] Treaty treaty)
        {
            if (id != treaty.TreatyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _cache.Update(treaty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatyExists(treaty.TreatyId))
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
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio", treaty.SubscriberId);
            ViewData["TariffPlanId"] = new SelectList(_tariffPlanCache.GetAll().Result, "TariffPlanId", "TariffName", treaty.TariffPlanId);

            return View(treaty);
        }

        // GET: Treaties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treaty = await _cache.Get((int)id);
            if (treaty == null)
            {
                return NotFound();
            }

            return View(treaty);
        }

        // POST: Treaties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treaty = await _cache.Get(id);
            if (treaty != null)
            {
               await _cache.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TreatyExists(int id)
        {
            return _cache.GetAll().Result.Any(e => e.TreatyId == id);
        }
    }
}
