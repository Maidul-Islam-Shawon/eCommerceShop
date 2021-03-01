using System;
using Xunit;

namespace eCommerceXUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void SmallAmountTest()
        {
            double expected = 5;
            double actual = 3 + 2;

            Assert.Equal(expected, actual);
        }

        
    }
}
