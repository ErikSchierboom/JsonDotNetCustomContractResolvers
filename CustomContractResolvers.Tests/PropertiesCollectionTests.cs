namespace JsonDotNet.CustomContractResolvers.Tests
{
    using System;
    using System.Collections.Generic;

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
        public void ConstructorWithEnumerableIsNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesCollection(collection));
        }

        [Fact]
        public void ConstructorWithStringIsNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            string collection = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesCollection(collection));
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

        [Fact]
        public void ConstructorWithPropertiesAsEmptyStringWillNotAddAnyProperties()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection(string.Empty);

            // Act

            // Assert
            Assert.Equal(0, propertiesCollection.Count);
        }

        [Fact]
        public void ConstructorWithPropertiesAsWhitespaceStringWillNotAddAnyProperties()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection(" \t\n ");

            // Act

            // Assert
            Assert.Equal(0, propertiesCollection.Count);
        }

        [Fact]
        public void ConstructorWithPropertiesAsCommaSeparatedStringWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id,Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithPropertiesAsCommaSeparatedStringWithSpacesWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id, Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithPropertiesAsSpaceSeparatedStringWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }
    }
}