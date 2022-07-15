using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomerDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomerDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, Models.Customer product, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var product = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);

                if (product != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(product);

                    return (true, result, "");
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> products, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var products = await dbContext.Customers.ToListAsync();

                if (products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(products);

                    return (true, result, "");
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
            }
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Thomas", Address = "Uebelbach" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Markus", Address = "Seiersberg" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Lisa", Address = "Seiersberg" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Gerald", Address = "Graz" });

                dbContext.SaveChanges();
            }
        }
    }
}
