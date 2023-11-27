using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SotovayaSvyas.Data;
using SotovayaSvyas.Models;

namespace SotovayaSvyas.Services
{
    public class TreatyService
    {
        private readonly MobileOperatorContext _context;
        private IMemoryCache _cache;

        public TreatyService(MobileOperatorContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<Treaty>> GetAll()
        {
            if (!_cache.TryGetValue("Treaty", out List<Treaty> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs;
        }

        public async Task<Treaty> Get(int id)
        {
            if (!_cache.TryGetValue("Treaty", out List<Treaty> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs.FirstOrDefault(e => e.TreatyId == id);
        }

        public async Task Add(Treaty entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Update(Treaty entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Delete(int id)
        {
            _context.Remove(await _context.Treatys.FirstAsync(e => e.TreatyId == id));
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task<List<Treaty>> UpdateCache()
        {
            var entities = _context.Treatys.Include(e => e.Subscriber).Include(e => e.TariffPlan).ToList();
            if (entities != null)
            {
                _cache.Set("Treaty", entities, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return entities;
        }
    }
}
