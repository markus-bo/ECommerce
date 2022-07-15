using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrderAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders
                    .Where(x => x.CustomerId == customerId)
                    .Include(x => x.Items)
                    .ToListAsync();

                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);

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
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2022, 07, 14, 16, 00, 00),
                    Total = 100,
                    Items = new List<Db.OrderItem>() {
                  new Db.OrderItem() { OrderId = 1, ProductId = 3, Quantity = 4, UnitPrice = 10 },
                  new Db.OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 5 },
                  new Db.OrderItem() { OrderId = 1, ProductId = 5, Quantity = 0, UnitPrice = 150 },
                }
                });

                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 3,
                    OrderDate = new DateTime(2022, 01, 14, 12, 11, 00),
                    Total = 200,
                    Items = new List<Db.OrderItem>() {
                  new Db.OrderItem() { OrderId = 2, ProductId = 2, Quantity = 2, UnitPrice = 6 },
                  new Db.OrderItem() { OrderId = 2, ProductId = 4, Quantity = 2, UnitPrice = 10 },
                  new Db.OrderItem() { OrderId = 2, ProductId = 1, Quantity = 2, UnitPrice = 20 },
                }
                });

                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 3,
                    CustomerId = 1,
                    OrderDate = new DateTime(2019, 01, 14, 12, 11, 00),
                    Total = 200,
                    Items = new List<Db.OrderItem>() {
                  new Db.OrderItem() { OrderId = 2, ProductId = 2, Quantity = 2, UnitPrice = 6 }
                }
                });

                dbContext.SaveChanges();
            }
        }
    }
}
