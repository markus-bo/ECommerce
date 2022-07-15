using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsProviderTest
    {
        [Fact]
        public async Task GetProductsAsync_ShouldReturnAllProducts_WhenCalled()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsAsync_ShouldReturnAllProducts_WhenCalled)).Options;
            var dbContext = new ProductsDbContext(dbContextOptions);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            // Act
            var result = await productsProvider.GetProductsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.products.Any());
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnAllProducts_WhenCalledWithValidId()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductAsync_ShouldReturnAllProducts_WhenCalledWithValidId)).Options;
            var dbContext = new ProductsDbContext(dbContextOptions);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            // Act
            var result = await productsProvider.GetProductAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.product.Id);
            Assert.Equal(11, result.product.Inventory);
            Assert.Equal<decimal>((decimal)3.14, result.product.Price);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnAllProducts_WhenCalledWithInvalidId()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductAsync_ShouldReturnAllProducts_WhenCalledWithInvalidId)).Options;
            var dbContext = new ProductsDbContext(dbContextOptions);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            // Act
            var result = await productsProvider.GetProductAsync(-1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.product);
            Assert.NotNull(result.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i=1; i<=10; i++)
            {
                dbContext.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price =(decimal)(i*3.14)
                }) ;
            }

            dbContext.SaveChanges();
        }
    }
}
