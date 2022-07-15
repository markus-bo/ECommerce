using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> products, string ErrorMessage)> GetCustomersAsync();

        Task<(bool IsSuccess, Customer product, string ErrorMessage)> GetCustomerAsync(int id);
    }
}
