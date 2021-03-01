using eCommerceShop.Models;
using System;
using Xunit;

namespace eCommerceXUnitTest
{
    public class ProductTest
    {
        public readonly Products _products;

        public ProductTest(Products products)
        {
            _products = products;
        }

        [Fact]
        public void ProductModel()
        {
            var actual = _products.Name="Abul";

            var  expected = "Abul";

            //double expected = 5;
            //double actual = 3 + 2;

            Assert.Equal(expected, actual);
        }

        
    }
}
