using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SotovayaSvyas.Data;
using SotovayaSvyas.Models;

namespace SotovayaSvyas.Services
{
    public class SubscriberService
    {
        private readonly MobileOperatorContext _context;
        private IMemoryCache _cache;

        public SubscriberService(MobileOperatorContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<Subscriber>> GetAll()
        {
            if (!_cache.TryGetValue("Subscriber", out List<Subscriber> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs;
        }

        public async Task<Subscriber> Get(int id)
        {
            if (!_cache.TryGetValue("Subscriber", out List<Subscriber> tariffs))
            {
                tariffs = await UpdateCache();
            }
            else
                Console.WriteLine("Из кеша");

            return tariffs.FirstOrDefault(e => e.SubscriberId == id);
        }

        public async Task Add(Subscriber entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Update(Subscriber entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task Delete(int id)
        {
            _context.Remove(await _context.Subscribers.FirstAsync(e => e.SubscriberId == id));
            await _context.SaveChangesAsync();
            await UpdateCache();
        }

        public async Task<List<Subscriber>> UpdateCache()
        {
            var entities = _context.Subscribers.ToList();
            if (entities != null)
            {
                _cache.Set("Subscriber", entities, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return entities;
        }
    }
}
