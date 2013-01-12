namespace JsonDotNet.CustomContractResolvers.Tests
{
    using System;

    using Xunit;

    public class PropertiesCollectionTests
    {
        [Fact]
        public void DefaultConstructorWillSetComparerToOrdinalIgnoreCaseStringComparer()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act

            // Assert
            Assert.Same(StringComparer.OrdinalIgnoreCase, propertiesCollection.Comparer);
        }

        [Fact]
        public void ConstructorWithEnumerableWillSetComparerToOrdinalIgnoreCaseStringComparer()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection(new string[0]);

            // Act

            // Assert
            Assert.Same(StringComparer.OrdinalIgnoreCase, propertiesCollection.Comparer);
        }
    }
}