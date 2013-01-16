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
        public void ConstructorWithEnumerableParameterIsNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<string> collection = null;

            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesCollection(collection));
        }

        [Fact]
        public void ConstructorWithNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            string collection = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesCollection(collection));
        }

        [Fact]
        public void ConstructorWithEnumerableParameterWillSetComparerToOrdinalIgnoreCaseStringComparer()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection(new string[0]);

            // Act

            // Assert
            Assert.Same(StringComparer.OrdinalIgnoreCase, propertiesCollection.Comparer);
        }

        [Fact]
        public void ConstructorWithEmptyStringWillNotAddAnyProperties()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection(string.Empty);

            // Act

            // Assert
            Assert.Equal(0, propertiesCollection.Count);
        }

        [Fact]
        public void ConstructorWithStringParameterIsWhitespaceStringWillNotAddAnyProperties()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection(" \t\n ");

            // Act

            // Assert
            Assert.Equal(0, propertiesCollection.Count);
        }

        [Fact]
        public void ConstructorWithStringParameterIsCommaSeparatedStringWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id,Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithStringParameterIsCommaSeparatedStringWithSpacesWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id, Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithStringParameterIsSpaceSeparatedStringWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithStringParameterIsSinglePropertyWillAddThatProperty()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection("Movie.Id");

            // Act

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
        }

        [Fact]
        public void AddWithNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            string collection = null;
            var propertiesCollection = new PropertiesCollection();

            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => propertiesCollection.Add(collection));
        }

        [Fact]
        public void AddWithEmptyStringWillNotAddAnyProperties()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            propertiesCollection.Add(string.Empty);

            // Assert
            Assert.Equal(0, propertiesCollection.Count);
        }

        [Fact]
        public void AddWithWhitespaceStringWillNotAddAnyProperties()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            propertiesCollection.Add(" \t\n ");

            // Assert
            Assert.Equal(0, propertiesCollection.Count);
        }

        [Fact]
        public void AddWithSinglePropertyAddsThatProperty()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            propertiesCollection.Add("Movie.Id");

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
        }

        [Fact]
        public void AddWithSinglePropertyThatDoesNotAlreadyExistReturnsTrue()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            var added = propertiesCollection.Add("Movie.Id");

            // Assert
            Assert.True(added);
        }

        [Fact]
        public void AddWithSinglePropertyThatAlreadyExistsReturnsFalse()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();
            propertiesCollection.Add("Movie.Id");

            // Act
            var added = propertiesCollection.Add("Movie.Id");

            // Assert
            Assert.False(added);
        }

        [Fact]
        public void AddWithCommaSeparatedStringWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            propertiesCollection.Add("Movie.Id, Movie.Title");

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void AddWithCommaSeparatedStringWithSpacesWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            propertiesCollection.Add("Movie.Id, Movie.Title");

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void AddWithSpaceSeparatedStringWillExtractAllPropertiesFromTheStringAndAddThem()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            propertiesCollection.Add("Movie.Id Movie.Title");

            // Assert
            Assert.True(propertiesCollection.Contains("Movie.Id"));
            Assert.True(propertiesCollection.Contains("Movie.Title"));
        }

        [Fact]
        public void AddWithAllPropertiesDidNotExistsReturnsTrue()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            var added = propertiesCollection.Add("Movie.Id Movie.Title");

            // Assert
            Assert.True(added);
        }

        [Fact]
        public void AddWithOnePropertyDidNotExistsReturnsTrue()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();
            propertiesCollection.Add("Movie.Id");

            // Act
            var added = propertiesCollection.Add("Movie.Id Movie.Title");

            // Assert
            Assert.True(added);
        }

        [Fact]
        public void AddWithAllPropertiesAlreadyExistedReturnsFalse()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();
            propertiesCollection.Add("Movie.Id");
            propertiesCollection.Add("Movie.Title");

            // Act
            var added = propertiesCollection.Add("Movie.Id Movie.Title");

            // Assert
            Assert.False(added);
        }

        [Fact]
        public void ToStringWithNoPropertiesAddedReturnsEmptyString()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();

            // Act
            var propertiesCollectionAsString = propertiesCollection.ToString();

            // Assert
            Assert.Equal(string.Empty, propertiesCollectionAsString);
        }

        [Fact]
        public void ToStringWithSinglePropertyAddedReturnsThatProperty()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();
            propertiesCollection.Add("Movie.Id");

            // Act
            var propertiesCollectionAsString = propertiesCollection.ToString();

            // Assert
            Assert.Equal("Movie.Id", propertiesCollectionAsString);
        }

        [Fact]
        public void ToStringWithSinglePropertyAddedReturnsPropertiesSeparatedByCommas()
        {
            // Arrange
            var propertiesCollection = new PropertiesCollection();
            propertiesCollection.Add("Movie.Id");
            propertiesCollection.Add("Movie.Title");

            // Act
            var propertiesCollectionAsString = propertiesCollection.ToString();

            // Assert
            Assert.Equal("Movie.Id,Movie.Title", propertiesCollectionAsString);
        }
    }
}