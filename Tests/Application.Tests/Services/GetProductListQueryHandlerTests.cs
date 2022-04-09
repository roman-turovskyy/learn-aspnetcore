using Application.Services;
using DAL.Models;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Services
{
    public class GetProductListQueryHandlerTests
    {
        [Fact]
        public async Task EmptyDatabase_EmptyResult_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var handler = new GetProductListQueryHandler(dbContext);

                var res = await handler.Handle(new GetProductListQuery());

                Assert.Empty(res);
            }
        }

        [Fact]
        public async Task DataBaseWithSingleProduct_ThatProductIsReturned_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var Product = new Product()
                {
                    ProductId = 1,
                    Name = "Name1",
                    FinishedGoodsFlag = false,
                    MakeFlag = false,
                    ProductNumber = "ProductNumber1"
                };
                dbContext.Product.Add(Product);
                await dbContext.SaveChangesAsync();

                var handler = new GetProductListQueryHandler(dbContext);

                var res = await handler.Handle(new GetProductListQuery());

                Assert.Equal(1, res.Count);
                Assert.Equal(Product.ProductId, res[0].ProductId);
            }
        }
    }
}
