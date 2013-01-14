namespace JsonDotNet.CustomContractResolvers.Tests
{
    using System.Linq;

    using JsonDotNet.CustomContractResolvers.Tests.Stubs;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Xunit;
    using Xunit.Extensions;

    public class PropertiesContractResolverTests
    {
        [Fact]
        public void ByDefaultNoPropertiesAreSpecified()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();

            // Act

            // Assert
            Assert.False(customPropertiesContractResolver.Properties.Any());
        }

        [Fact]
        public void ByDefaultNoExcludePropertiesAreSpecified()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();

            // Act

            // Assert
            Assert.False(customPropertiesContractResolver.ExcludeProperties.Any());
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

            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("*");

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsOneFieldOnlySerializesSpecifiedField()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

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
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Theory]
        [InlineData("Movie.Inventory")]
        [InlineData("Movie.Id2")]
        [InlineData("Movie.Id ")]
        [InlineData(" Movie.Id")]
        public void ConvertingWithOnlyUnknownPropertiesReturnsEmptyObjct(string unknownField)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesWithoutDeclaringTypeNameWillUseTypeOfRootProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Id");
            customPropertiesContractResolver.Properties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsSeveralPropertiesOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Movie.Id");
            customPropertiesContractResolver.Properties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Movie.Title");
            customPropertiesContractResolver.Properties.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Movie.Title");
            customPropertiesContractResolver.Properties.Add("Movie.Director");
            customPropertiesContractResolver.Properties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithPropertiesWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Movie.*");
            customPropertiesContractResolver.Properties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithTypeWildcardAddedForPropertySerializesAllPropertiesWithSpecifiedPropertyName()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("*.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingWithTypeWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("*.Id");
            customPropertiesContractResolver.Properties.Add("Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Theory]
        [InlineData("Movie.Id")]
        [InlineData("MOVIE.Id")]
        [InlineData("MOVIE.ID")]
        [InlineData("movie.id")]
        public void ConvertingWithExcludePropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Movie.Inventory")]
        [InlineData("Movie.Id2")]
        [InlineData("Movie.Id ")]
        [InlineData(" Movie.Id")]
        public void ConvertingIgnoresUnknownExcludeProperties(string unknownField)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesIsGeneralWildcardDoesNotSerializeAnyProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesWithoutDeclaringTypeNameWillUseTypeOfRootProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Id");
            customPropertiesContractResolver.ExcludeProperties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsOneFieldDoesNotSerializeSpecifiedField()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsSeveralPropertiesDoesNotSerializeSpecifiedProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.Id");
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.Title");
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.Title");
            customPropertiesContractResolver.ExcludeProperties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesTypeWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("*.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludePropertiesTypeWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.ExcludeProperties.Add("*.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Director.Name");
            customPropertiesContractResolver.ExcludeProperties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithWildcardPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("Director.*");
            customPropertiesContractResolver.ExcludeProperties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithWildcardBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver();
            customPropertiesContractResolver.Properties.Add("*");
            customPropertiesContractResolver.ExcludeProperties.Add("*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

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