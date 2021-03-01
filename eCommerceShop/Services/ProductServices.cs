using eCommerceShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceShop.Services
{
    public class ProductServices
    {
        public IList<Products> FilterByMinMaxAmount(decimal? minAmount, decimal? maxAmount, IList<Products> products)
        {
            var filteredResult = products.Where(m => m.Price >= minAmount && m.Price <= maxAmount)
                                                    .ToList();

            return filteredResult;
        }
    }
}
