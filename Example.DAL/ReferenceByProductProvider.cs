using Example.Common.Database.Enums;
using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Example.DAL;

public class ReferenceByProductProvider : IReferenceByProductProvider
{
    private readonly SampleDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public ReferenceByProductProvider(SampleDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public List<ReferenceByProductCommon> GetRefrences(string? productShortName = null, string? entity = null, string? reference = null)
    {
        return _memoryCache.GetOrCreate("ReferenceByProductProvider", entry =>
        {
            entry.SetAbsoluteExpiration(DateTimeOffset.MaxValue);

            return GetReferencesFromDb(productShortName, entity, reference);
        });
    }

    private List<ReferenceByProductCommon> GetReferencesFromDb(string? productShortName, string? entity, string? reference)
    {
        IQueryable<ReferenceByProduct> query = _context.ReferenceByProduct.Include(x => x.Product);

        if (productShortName != null)
            query = query.Where(x => x.Product.ShortName == productShortName);

        if (entity != null)
            query = query.Where(x => x.Entity == entity);

        if (reference != null)
            query = query.Where(x => x.Reference == reference);

        return query.Select(x => new ReferenceByProductCommon
        {
            Id = x.ReferenceByProductId,
            Code = x.Code,
            Entity = x.Entity,
            Reference = x.Reference,
            ProductShortName = x.Product.ShortName
        }).ToList();
    }
}
