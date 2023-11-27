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
using SotovayaSvyas.ViewModels.SortViewModels;

namespace SotovayaSvyas.Controllers
{
    [Authorize]
    public class ServicesProvidedController : Controller
    {
        private readonly ServicePrividedService _cache;
        private readonly SubscriberService _subscriberCache;

        private readonly int _pageSize = 10;
        public ServicesProvidedController(ServicePrividedService cache, SubscriberService subscriberCache)
        {
            _cache = cache;
            _subscriberCache = subscriberCache;
        }

        // GET: ServicesProvided
        public async Task<IActionResult> Index(ServicesProvidedFilterViewModel filter, int page = 1, ServicesProvidedSortState sortOrder = ServicesProvidedSortState.TimeAsc)
        {
            var servicesProvided = await _cache.GetAll();

            //Фильтрация
            if(filter.DataVolumeFind != null)
                servicesProvided = servicesProvided.Where(e => e.DataVolume == filter.DataVolumeFind).ToList();
            if(filter.QuantitySmsFind != null)
                servicesProvided = servicesProvided.Where(e => e.QuantitySms == filter.QuantitySmsFind).ToList();
            if (filter.TimeFind != null)
                servicesProvided = servicesProvided.Where(e => e.Time == filter.TimeFind).ToList();
            if (!String.IsNullOrEmpty(filter.SubscriberFind))
                servicesProvided = servicesProvided.Where(e => $"{e.Subscriber.Surname} {e.Subscriber.Name} {e.Subscriber.Lastname}".Contains(filter.SubscriberFind)).ToList();

            //сортировка
            switch(sortOrder)
            {
                case ServicesProvidedSortState.TimeAsc:
                    servicesProvided = servicesProvided.OrderBy(e => e.Time).ToList();
                    break;
                case ServicesProvidedSortState.TimeDesc:
                    servicesProvided = servicesProvided.OrderByDescending(e => e.Time).ToList();
                    break;
                case ServicesProvidedSortState.QuantitySmsAsc:
                    servicesProvided = servicesProvided.OrderBy(e => e.QuantitySms).ToList();
                    break;
                case ServicesProvidedSortState.QuantitySmsDesc:
                    servicesProvided = servicesProvided.OrderByDescending(e => e.QuantitySms).ToList();
                    break;
                case ServicesProvidedSortState.DataVolumeAsc:
                    servicesProvided = servicesProvided.OrderBy(e => e.DataVolume).ToList();
                    break;
                case ServicesProvidedSortState.DataVolumeDesc:
                    servicesProvided = servicesProvided.OrderByDescending(e => e.DataVolume).ToList();
                    break;
                case ServicesProvidedSortState.SubscriberAsc:
                    servicesProvided = servicesProvided.OrderBy(e => e.Subscriber.Surname).ToList();
                    break;
                case ServicesProvidedSortState.SubscriberDesc:
                    servicesProvided = servicesProvided.OrderByDescending(e => e.Subscriber.Surname).ToList();
                    break;
            }

            //Пагинация
            int count = servicesProvided.Count();
            servicesProvided = servicesProvided.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель представления
            ServicesProvidedViewModel viewModel = new()
            {
                ServicesProvideds = servicesProvided,
                PageViewModel = new(page, count, _pageSize),
                SortOrder = new(sortOrder),
                Filter = filter
            };

            return View(viewModel);
        }

        // GET: ServicesProvided/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicesProvided = await _cache.Get((int)id);
            if (servicesProvided == null)
            {
                return NotFound();
            }

            return View(servicesProvided);
        }

        // GET: ServicesProvided/Create
        public IActionResult Create()
        {
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio");
            return View();
        }

        // POST: ServicesProvided/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServicesProvidedId,Time,QuantitySms,DataVolume,SubscriberId")] ServicesProvided servicesProvided)
        {
            if (ModelState.IsValid)
            {
              await _cache.Add(servicesProvided);
                return RedirectToAction(nameof(Index));
            }
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio");
            return View(servicesProvided);
        }

        // GET: ServicesProvided/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicesProvided = await _cache.Get((int)id);
            if (servicesProvided == null)
            {
                return NotFound();
            }
            var subscribers = _subscriberCache.GetAll().Result.Select(e => new { Fio = $"{e.Surname} {e.Name} {e.Lastname}", SubscriberId = e.SubscriberId });
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio");
            return View(servicesProvided);
        }

        // POST: ServicesProvided/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicesProvidedId,Time,QuantitySms,DataVolume,SubscriberId")] ServicesProvided servicesProvided)
        {
            if (id != servicesProvided.ServicesProvidedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _cache.Update(servicesProvided);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesProvidedExists(servicesProvided.ServicesProvidedId))
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
            ViewData["SubscriberId"] = new SelectList(subscribers, "SubscriberId", "Fio");
            return View(servicesProvided);
        }

        // GET: ServicesProvided/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicesProvided = await _cache.Get((int)id);
            if (servicesProvided == null)
            {
                return NotFound();
            }

            return View(servicesProvided);
        }

        // POST: ServicesProvided/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicesProvided = await _cache.Get((int)id);
            if (servicesProvided != null)
            {
               await _cache.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServicesProvidedExists(int id)
        {
            return _cache.GetAll().Result.Any(e => e.ServicesProvidedId == id);
        }
    }
}
