using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customerService;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customerService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customerService = customerService;
        }



        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var customerResult = await customerService.GetCustomerAsync(customerId);
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productResult = await productsService.GetProductsAsync();

            if (ordersResult.IsSuccess && customerResult.IsSuccess)
            {
                foreach(var order in ordersResult.Orders)
                {
                    foreach(var item in order.Items)
                    {
                        if (productResult.IsSuccess)
                        {
                            item.ProductName = productResult.Products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
                        }
                        else 
                        {
                            item.ProductName = "Product information is not available";
                        }
                    }
                }

                var result = new
                {
                    Customer = customerResult.Customer,
                    Orders = ordersResult.Orders
                };

                return (true, result);
            }

            return (false, null);
        }
    }
}
