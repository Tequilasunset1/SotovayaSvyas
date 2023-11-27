using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SotovayaSvyas.Data;
using SotovayaSvyas.Models;

namespace SotovayaSvyas.Services
{
    public class TypeTariffService
    {
        private readonly MobileOperatorContext _context;
        private IMemoryCache _cache;

        public TypeTariffService(MobileOperatorContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<TypeTariff>> GetAll()
        {
            if (!_cache.TryGetValue("TypeTariff", out List<TypeTariff> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs;
        }

        public async Task<TypeTariff> Get(int id)
        {
            if (!_cache.TryGetValue("TypeTariff", out List<TypeTariff> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs.FirstOrDefault(e => e.TypeTariffId == id);
        }

        public async Task Add(TypeTariff entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Update(TypeTariff entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Delete(int id)
        {
            _context.Remove(await _context.TypeTariffs.FirstAsync(e => e.TypeTariffId == id));
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task<List<TypeTariff>> UpdateCache()
        {
            _cache.Remove("TypeTariff");
            var entities = await _context.TypeTariffs.ToListAsync();
            if (entities != null)
            {
                _cache.Set("TypeTariff", entities, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return entities;
        } 
    }
}
