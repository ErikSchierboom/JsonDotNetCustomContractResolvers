namespace JsonDotNet.CustomContractResolvers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JsonDotNet.CustomContractResolvers.Tests.Stubs;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Xunit;
    using Xunit.Extensions;

    public class PropertiesContractResolverTests
    {
        [Fact]
        public void ConstructorWithPropertiesCollectionSetToNullThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<string> properties = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(properties, new string[0]));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesCollectionSetToNullThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<string> excludeProperties = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(new string[0], excludeProperties));
        }

        [Fact]
        public void ConstructorWithPropertiesSetToNullStringThrowsArgumentNullException()
        {
            // Arrange
            string properties = null;
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(properties: properties));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesSetToNullStringThrowsArgumentNullException()
        {
            // Arrange
            string excludeProperties = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(excludeProperties: excludeProperties));
        }

        [Fact]
        public void ConstructorWithNoPropertiesSetsPropertiesToEmptySet()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: string.Empty);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.Properties.Any());
        }

        [Fact]
        public void ConstructorWithNoExcludePropertiesSetsExcludePropertiesToEmptySet()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: string.Empty);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.ExcludeProperties.Any());
        }

        [Fact]
        public void ConstructorWithPropertiesSetToEmptyStringSetsPropertiesToEmptySet()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: string.Empty);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.Properties.Any());
        }

        [Fact]
        public void ConstructorWithPropertiesSetToStringWithOnePropertyAddsThatProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Id");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Id"));
        }

        [Fact]
        public void ConstructorWithPropertiesSetToStringWithCommaSeparatedPropertiesAddsThoseProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Id,Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithPropertiesSetToStringWithCommaSeparatedWithSpacesPropertiesAddsThoseProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Id, Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithPropertiesSetToStringWithSpacesPropertiesAddsThoseProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Id Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesSetToEmptyStringSetsExcludePropertiesToEmptySet()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: string.Empty);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.ExcludeProperties.Any());
        }

        [Fact]
        public void ConstructorWithExcludePropertiesSetToStringWithOnePropertyAddsThatProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Movie.Id");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Id"));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesSetToStringWithCommaSeparatedPropertiesAddsThoseProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Movie.Id,Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesSetToStringWithCommaSeparatedWithSpacesPropertiesAddsThoseProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Movie.Id, Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Title"));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesSetToStringWithSpacesPropertiesAddsThoseProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Movie.Id Movie.Title");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Title"));
        }

        [Fact]
        public void ConvertingWithPropertiesAndExcludePropertiesAreEmptySerializesObjectLikeDefault()
        {
            // Arrange
            var objectToSerialize = CreateObjectToSerialize();

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(new PropertiesContractResolver()));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Fact]
        public void ConvertingWithPropertiesIsGeneralWildcardSerializesObjectLikeDefault()
        {
            // Arrange
            var objectToSerialize = CreateObjectToSerialize();

            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("*");

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsOneFieldOnlySerializesSpecifiedField()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Theory]
        [InlineData("Movie.Id")]
        [InlineData("MOVIE.Id")]
        [InlineData("MOVIE.ID")]
        [InlineData("movie.id")]
        public void ConvertingWithPropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Theory]
        [InlineData("Movie.Inventory")]
        [InlineData("Movie.Id2")]
        [InlineData("Movie.Id_")]
        [InlineData("_Movie.Id")]
        public void ConvertingWithOnlyUnknownPropertiesReturnsEmptyObjct(string unknownField)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesWithoutDeclaringTypeNameWillUseTypeOfRootProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Id");
            propertiesContractResolver.Properties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsSeveralPropertiesOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Movie.Id");
            propertiesContractResolver.Properties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Movie.Title");
            propertiesContractResolver.Properties.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Movie.Title");
            propertiesContractResolver.Properties.Add("Movie.Director");
            propertiesContractResolver.Properties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Movie.*");
            propertiesContractResolver.Properties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Movie.Id")]
        [InlineData("MOVIE.Id")]
        [InlineData("MOVIE.ID")]
        [InlineData("movie.id")]
        public void ConvertingWithExcludePropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Movie.Inventory")]
        [InlineData("Movie.Id2")]
        [InlineData("Movie.Id_")]
        [InlineData("_Movie.Id")]
        public void ConvertingIgnoresUnknownExcludeProperties(string unknownField)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesIsGeneralWildcardDoesNotSerializeAnyProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesWithoutDeclaringTypeNameWillUseTypeOfRootProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Id");
            propertiesContractResolver.ExcludeProperties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsOneFieldDoesNotSerializeSpecifiedField()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsSeveralPropertiesDoesNotSerializeSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Movie.Id");
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");
            propertiesContractResolver.ExcludeProperties.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");
            propertiesContractResolver.ExcludeProperties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.ExcludeProperties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Director.Name");
            propertiesContractResolver.ExcludeProperties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithWildcardPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("Director.*");
            propertiesContractResolver.ExcludeProperties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithWildcardBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver();
            propertiesContractResolver.Properties.Add("*");
            propertiesContractResolver.ExcludeProperties.Add("*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        private static JsonSerializerSettings CreateCustomJsonSerializerSettings(IContractResolver contractResolver)
        {
            return new JsonSerializerSettings { ContractResolver = contractResolver };
        }

        private static JsonSerializerSettings CreateDefaultJsonSerializerSettings()
        {
            return new JsonSerializerSettings();
        }

        private static Movie CreateObjectToSerialize()
        {
            return new Movie
                       {
                           Id = 12,
                           Title = "Inception",
                           Director = new Director
                                          {
                                              Id = 77,
                                              Name = "Christopher Nolan",
                                          }
                       };
        }
    }
}