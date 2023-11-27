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
    public class TypeTariffsController : Controller
    {
        private readonly TypeTariffService _cache;
        private readonly int _pageSize = 10;

        public TypeTariffsController(TypeTariffService cache)
        {
            _cache = cache;
        }

        // GET: TypeTariffs
        public async Task<IActionResult> Index(string nameFind = "", int page = 1, TypeTariffSortState sortOrder = TypeTariffSortState.TariffNameAsc)
        {
            Console.WriteLine(nameFind);
            var typeTariffs = await _cache.GetAll();

            //Фильтрация
            if (!String.IsNullOrEmpty(nameFind))
                typeTariffs = typeTariffs.Where(e => e.TariffName.Contains(nameFind)).ToList();

            //сортировка
            switch (sortOrder)
            {
                case TypeTariffSortState.TariffNameAsc:
                    typeTariffs = typeTariffs.OrderBy(e => e.TariffName).ToList();
                    break;
                case TypeTariffSortState.TariffNameDesc:
                    typeTariffs = typeTariffs.OrderByDescending(e => e.TariffName).ToList();
                    break;
            }

            //Пагинация
            int count = typeTariffs.Count();
            typeTariffs = typeTariffs.Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

            //Модель представления
            TypeTariffsViewModel viewModel = new()
            {
                TypeTariffs = typeTariffs,
                PageViewModel = new(page, count, _pageSize),
                SortOrder = new(sortOrder),
                NameFind = nameFind
            };

            return View(viewModel);
        }

        // GET: TypeTariffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeTariff = await _cache.Get((int)id);
            if (typeTariff == null)
            {
                return NotFound();
            }

            return View(typeTariff);
        }

        // GET: TypeTariffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeTariffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeTariffId,TariffName")] TypeTariff typeTariff)
        {
            if (ModelState.IsValid)
            {
                await _cache.Add(typeTariff);
                return RedirectToAction(nameof(Index));
            }
            return View(typeTariff);
        }

        // GET: TypeTariffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeTariff = await _cache.Get((int)id);
            if (typeTariff == null)
            {
                return NotFound();
            }
            return View(typeTariff);
        }

        // POST: TypeTariffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeTariffId,TariffName")] TypeTariff typeTariff)
        {
            if (id != typeTariff.TypeTariffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _cache.Update(typeTariff);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeTariffExists(typeTariff.TypeTariffId))
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
            return View(typeTariff);
        }

        // GET: TypeTariffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeTariff = await _cache.Get((int)id);
            if (typeTariff == null)
            {
                return NotFound();
            }

            return View(typeTariff);
        }

        // POST: TypeTariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeTariff = await _cache.GetAll();
            if (typeTariff != null)
            {
                await _cache.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TypeTariffExists(int id)
        {
            return _cache.GetAll().Result.Any(e => e.TypeTariffId == id);
        }
    }
}
