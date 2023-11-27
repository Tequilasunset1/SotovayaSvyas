using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SotovayaSvyas.Data;
using SotovayaSvyas.Models;

namespace SotovayaSvyas.Services
{
    public class ServicePrividedService
    {
        private readonly MobileOperatorContext _context;
        private IMemoryCache _cache;

        public ServicePrividedService(MobileOperatorContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<ServicesProvided>> GetAll()
        {
            if (!_cache.TryGetValue("ServicesProvided", out List<ServicesProvided> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs;
        }

        public async Task<ServicesProvided> Get(int id)
        {
            if (!_cache.TryGetValue("ServicesProvided", out List<ServicesProvided> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs.Single(e => e.ServicesProvidedId == id);
        }

        public async Task Add(ServicesProvided entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Update(ServicesProvided entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Delete(int id)
        {
            _context.Remove(await _context.ServicesProvideds.FirstAsync(e => e.ServicesProvidedId == id));
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task<List<ServicesProvided>> UpdateCache()
        {
            var entities = _context.ServicesProvideds.Include(e => e.Subscriber).ToList();
            if (entities != null)
            {
                _cache.Set("ServicesProvided", entities, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return entities;
        }
    }
}
