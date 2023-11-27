using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SotovayaSvyas.Data;
using SotovayaSvyas.Models;

namespace SotovayaSvyas.Services
{
    public class TariffPlanService
    {
        private readonly MobileOperatorContext _context;
        private IMemoryCache _cache;

        public TariffPlanService(MobileOperatorContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }
        public async Task<List<TariffPlan>> GetAll()
        {
            if (!_cache.TryGetValue("TariffPlan", out List<TariffPlan> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs;
        }

        public async Task<TariffPlan> Get(int id)
        {
            if (!_cache.TryGetValue("TariffPlan", out List<TariffPlan> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs.FirstOrDefault(e => e.TariffPlanId == id);
        }

        public async Task Add(TariffPlan entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Update(TariffPlan entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Delete(int id)
        {
            _context.Remove(await _context.TariffPlans.FirstAsync(e => e.TariffPlanId == id));
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task<List<TariffPlan>> UpdateCache()
        {
            var entities = _context.TariffPlans.Include(e => e.TypeTariff).ToList();
            if (entities != null)
            {
                _cache.Set("TariffPlan", entities, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return entities;
        }
    }
}
