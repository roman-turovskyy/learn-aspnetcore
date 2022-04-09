using Application.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class GetProductListQuery : IQuery<IList<Product>>
    {
    }

    public class GetProductListQueryHandler : IQueryHandler<GetProductListQuery, IList<Product>>
    {
        private readonly IAppDbContext _dbContext;

        public GetProductListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Product>> Handle(GetProductListQuery query, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Product.AsNoTracking().Take(100).ToListAsync(cancellationToken);
        }
    }
}
