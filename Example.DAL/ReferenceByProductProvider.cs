using Example.Common.Database.Enums;
using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.DAL;

public class ReferenceByProductProvider : IReferenceByProductProvider
{
    private SampleDbContext _context;

    public ReferenceByProductProvider(SampleDbContext context)
    {
        _context = context;
    }

    public List<ReferenceByProductCommon> GetRefrences(string? productShortName = null, string? entity = null, string? reference = null)
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
