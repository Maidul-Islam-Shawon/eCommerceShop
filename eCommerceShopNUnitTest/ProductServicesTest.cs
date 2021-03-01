using eCommerceShop.Models;
using eCommerceShop.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace eCommerceShopNUnitTest
{
    public class ProductServicesTest
    {
        ProductServices productServices;

        [SetUp]
        public void Setup()
        {
            productServices = new ProductServices();
        }

        [Test]
        public void FilterByMinMaxAmount_works()
        {
            //Arange
            var minAmount = 100;
            var maxAmount = 500;

            var products = new List<Products>() 
            {
                new Products
                {
                    Name="Iphone X",
                    Price=200,
                    ProductColor="Red",
                    IsAvailable=true,
                    ProductTypesId=1,
                    SpecialTagId=2
                },
                new Products
                {
                    Name="Logitech Keyboard",
                    Price=500,
                    ProductColor="Black",
                    IsAvailable=true,
                    ProductTypesId=2,
                    SpecialTagId=3
                },
                new Products
                {
                    Name="Skullcandy Headphone",
                    Price=450,
                    ProductColor="Blue",
                    IsAvailable=true,
                    ProductTypesId=4,
                    SpecialTagId=1
                },
                new Products
                {
                    Name="Samsung Tv 32",
                    Price=1450,
                    ProductColor="Black",
                    IsAvailable=true,
                    ProductTypesId=1,
                    SpecialTagId=1
                },
                new Products
                {
                    Name="Samsung D900",
                    Price=120,
                    ProductColor="Silver",
                    IsAvailable=false,
                    ProductTypesId=5,
                    SpecialTagId=1
                }
            };

            //Act
            
            var filterProduct = productServices.FilterByMinMaxAmount(minAmount, maxAmount, products);
            
            
            //Assert

            Assert.That(filterProduct, Is.EqualTo(products.Where(m => m.Price >= 100 && m.Price <= 500)));
        }

        [Test]
        public void FilterByMinMaxAmount_ByEqual()
        {
            //Arange
            var minAmount = 100;
            var maxAmount = 500;

            var products = new List<Products>()
            {
                new Products
                {
                    Name="Iphone X",
                    Price=200,
                    ProductColor="Red",
                    IsAvailable=true,
                    ProductTypesId=1,
                    SpecialTagId=2
                },
                new Products
                {
                    Name="Logitech Keyboard",
                    Price=500,
                    ProductColor="Black",
                    IsAvailable=true,
                    ProductTypesId=2,
                    SpecialTagId=3
                },
                new Products
                {
                    Name="Skullcandy Headphone",
                    Price=450,
                    ProductColor="Blue",
                    IsAvailable=true,
                    ProductTypesId=4,
                    SpecialTagId=1
                },
                new Products
                {
                    Name="Samsung Tv 32",
                    Price=1450,
                    ProductColor="Black",
                    IsAvailable=true,
                    ProductTypesId=1,
                    SpecialTagId=1
                },
                new Products
                {
                    Name="Samsung D900",
                    Price=120,
                    ProductColor="Silver",
                    IsAvailable=false,
                    ProductTypesId=5,
                    SpecialTagId=1
                }
            };

            //Act

            var filterProduct = productServices.FilterByMinMaxAmount(minAmount, maxAmount, products);

            var expectedResult = products.Where(m => m.Price >= minAmount && m.Price <= maxAmount);

            //Assert

            //Assert.That(filterProduct, Is.EqualTo(products.Where(m => m.Price >= 100 && m.Price <= 500)));
            Assert.AreEqual(expectedResult, filterProduct);
            
        }

        [Test]
        public void FilterByMinMaxAmount_Wrong()
        {
            //Arange
            var minAmount = 100;
            var maxAmount = 500;

            var products = new List<Products>()
            {
                new Products
                {
                    Name="Iphone X",
                    Price=200,
                    ProductColor="Red",
                    IsAvailable=true,
                    ProductTypesId=1,
                    SpecialTagId=2
                },
                new Products
                {
                    Name="Logitech Keyboard",
                    Price=500,
                    ProductColor="Black",
                    IsAvailable=true,
                    ProductTypesId=2,
                    SpecialTagId=3
                },
                new Products
                {
                    Name="Skullcandy Headphone",
                    Price=450,
                    ProductColor="Blue",
                    IsAvailable=true,
                    ProductTypesId=4,
                    SpecialTagId=1
                },
                new Products
                {
                    Name="Samsung Tv 32",
                    Price=1450,
                    ProductColor="Black",
                    IsAvailable=true,
                    ProductTypesId=1,
                    SpecialTagId=1
                },
                new Products
                {
                    Name="Samsung D900",
                    Price=120,
                    ProductColor="Silver",
                    IsAvailable=false,
                    ProductTypesId=5,
                    SpecialTagId=1
                }
            };

            //Act

            var filterProduct = productServices.FilterByMinMaxAmount(minAmount, maxAmount, products);

            var expectedResult = products.Where(m => m.Price >= minAmount);

            //Assert

            //Assert.That(filterProduct, Is.EqualTo(products.Where(m => m.Price >= 100 && m.Price <= 500)));
            Assert.AreNotEqual(expectedResult, filterProduct);
        }
    }
}