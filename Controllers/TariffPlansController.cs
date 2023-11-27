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
    public class TariffPlansController : Controller
    {
        private readonly TariffPlanService _cache;
        private readonly TypeTariffService _typeTariffCache;
        private readonly int _pageSize = 10;

        public TariffPlansController(TariffPlanService cache, TypeTariffService typeTariffCache)
        {
            _cache = cache;
            _typeTariffCache = typeTariffCache;
        }

        // GET: TariffPlans
        public async Task<IActionResult> Index(TariffPlanFilterViewModel filter, int page = 1, TariffPlanSortState sortOrder = TariffPlanSortState.NameAsc)
        {
            var TariffPlans = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(filter.TypeNameFind))
                TariffPlans = TariffPlans.Where(e => e.TariffName.Contains(filter.TypeNameFind)).ToList();
            if (!String.IsNullOrEmpty(filter.TypeTariffFind))
                TariffPlans = TariffPlans.Where(e => e.TypeTariff.TariffName.Contains(filter.TypeTariffFind)).ToList();
            if (filter.SubscriptionLocalFind != null)
                TariffPlans = TariffPlans.Where(e => e.SubscriptionLocal == filter.SubscriptionLocalFind).ToList();
            if (filter.SubscriptionIntercityFind != null)
                TariffPlans = TariffPlans.Where(e => e.SubscriptionIntercity == filter.SubscriptionIntercityFind).ToList();
            if (filter.SubscriptionInternationalFind != null)

                TariffPlans = TariffPlans.Where(e => e.SubscriptionInternational == filter.SubscriptionInternationalFind).ToList();
            if (filter.PriceSmsFind != null)
                TariffPlans = TariffPlans.Where(e => e.PriceSms == filter.PriceSmsFind).ToList();

            //сортировка
            switch (sortOrder)
            {
                case TariffPlanSortState.NameAsc:
                    TariffPlans = TariffPlans.OrderBy(e => e.TariffName).ToList();
                    break;
                case TariffPlanSortState.NameDesc:
                    TariffPlans = TariffPlans.OrderByDescending(e => e.TariffName).ToList();
                    break;
                case TariffPlanSortState.PriceSmsAsc:
                    TariffPlans = TariffPlans.OrderBy(e => e.PriceSms).ToList();
                    break;
                case TariffPlanSortState.PriceSmsDesc:
                    TariffPlans = TariffPlans.OrderByDescending(e => e.PriceSms).ToList();
                    break;
                case TariffPlanSortState.SubscriptionLocalAsc:
                    TariffPlans = TariffPlans.OrderBy(e => e.SubscriptionLocal).ToList();
                    break;
                case TariffPlanSortState.SubscriptionLocalDesc:
                    TariffPlans = TariffPlans.OrderByDescending(e => e.SubscriptionLocal).ToList();
                    break;
                case TariffPlanSortState.SubscriptionInternationalAsc:
                    TariffPlans = TariffPlans.OrderBy(e => e.SubscriptionInternational).ToList();
                    break;
                case TariffPlanSortState.SubscriptionInternationalDesc:
                    TariffPlans = TariffPlans.OrderByDescending(e => e.SubscriptionInternational).ToList();
                    break;
                case TariffPlanSortState.SubscriptionIntercityAsc:
                    TariffPlans = TariffPlans.OrderBy(e => e.SubscriptionIntercity).ToList();
                    break;
                case TariffPlanSortState.SubscriptionIntercityDesc:
                    TariffPlans = TariffPlans.OrderByDescending(e => e.SubscriptionIntercity).ToList();
                    break;
            }

            //Пагинация
            int count = TariffPlans.Count();
            TariffPlans = TariffPlans.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель представления
            TariffPlansViewModel viewModel = new()
            {
                TariffPlans = TariffPlans,
                PageViewModel = new(page, count, _pageSize),
                SortOrder = new(sortOrder),
                Filter = filter
            };

            return View(viewModel);
        }

        // GET: TariffPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariffPlan = await _cache.Get((int)id);
            if (tariffPlan == null)
            {
                return NotFound();
            }

            return View(tariffPlan);
        }

        // GET: TariffPlans/Create
        public IActionResult Create()
        {
            ViewData["TypeTariffId"] = new SelectList(_typeTariffCache.GetAll().Result, "TypeTariffId", "TariffName");
            return View();
        }

        // POST: TariffPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TariffPlanId,TariffName,SubscriptionLocal,SubscriptionIntercity,SubscriptionInternational,TypeTariffId,PriceSms")] TariffPlan tariffPlan)
        {
            if (ModelState.IsValid)
            {
               await _cache.Add(tariffPlan);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeTariffId"] = new SelectList(_typeTariffCache.GetAll().Result, "TypeTariffId", "TariffName", tariffPlan.TypeTariffId);
            return View(tariffPlan);
        }

        // GET: TariffPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariffPlan = await _cache.Get((int)id);
            if (tariffPlan == null)
            {
                return NotFound();
            }
            ViewData["TypeTariffId"] = new SelectList(_typeTariffCache.GetAll().Result, "TypeTariffId", "TariffName", tariffPlan.TypeTariffId);
            return View(tariffPlan);
        }

        // POST: TariffPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TariffPlanId,TariffName,SubscriptionLocal,SubscriptionIntercity,SubscriptionInternational,TypeTariffId,PriceSms")] TariffPlan tariffPlan)
        {
            if (id != tariffPlan.TariffPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _cache.Update(tariffPlan);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TariffPlanExists(tariffPlan.TariffPlanId))
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
            ViewData["TypeTariffId"] = new SelectList(_typeTariffCache.GetAll().Result, "TypeTariffId", "TariffName", tariffPlan.TypeTariffId);
            return View(tariffPlan);
        }

        // GET: TariffPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariffPlan = await _cache.Get((int)id);
            if (tariffPlan == null)
            {
                return NotFound();
            }

            return View(tariffPlan);
        }

        // POST: TariffPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tariffPlan = await _cache.Get((int)id);
            if (tariffPlan != null)
            {
               await _cache.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TariffPlanExists(int id)
        {
            return _cache.GetAll().Result.Any(e => e.TariffPlanId == id);
        }
    }
}
