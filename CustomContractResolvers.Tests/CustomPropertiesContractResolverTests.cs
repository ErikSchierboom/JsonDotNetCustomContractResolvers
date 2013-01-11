namespace CustomContractResolvers.Tests
{
    using System.Linq;

    using CustomContractResolvers.Tests.Stubs;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Xunit;
    using Xunit.Extensions;

    public class CustomPropertiesContractResolverTests
    {
        [Fact]
        public void ByDefaultNoFieldsAreSpecified()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();

            // Act

            // Assert
            Assert.False(customPropertiesContractResolver.Fields.Any());
        }

        [Fact]
        public void ByDefaultNoExcludeFieldsAreSpecified()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();

            // Act

            // Assert
            Assert.False(customPropertiesContractResolver.ExcludeFields.Any());
        }

        [Fact]
        public void ConvertingWithFieldsAndExcludeFieldsAreEmptySerializesObjectLikeDefault()
        {
            // Arrange
            var objectToSerialize = CreateObjectToSerialize();

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(new CustomPropertiesContractResolver()));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Fact]
        public void ConvertingWithFieldsIsGeneralWildcardSerializesObjectLikeDefault()
        {
            // Arrange
            var objectToSerialize = CreateObjectToSerialize();

            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("*");

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Fact]
        public void ConvertingWithFieldsContainsOneFieldOnlySerializesSpecifiedField()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Movie.Id");

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
        public void ConvertingWithFieldsIsNotCaseSensitive(string field)
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add(field);

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
        public void ConvertingWithOnlyUnknownFieldsReturnsEmptyObjct(string unknownField)
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithFieldsContainsSeveralFieldsOnlySerializesSpecifiedFields()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Movie.Id");
            customPropertiesContractResolver.Fields.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingWithFieldsContainsFieldForNestedPropertyOnlySerializesSpecifiedFields()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Movie.Title");
            customPropertiesContractResolver.Fields.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithFieldsContainsFieldOfNestedPropertyOnlySerializesSpecifiedFields()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Movie.Title");
            customPropertiesContractResolver.Fields.Add("Movie.Director");
            customPropertiesContractResolver.Fields.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithFieldsWildcardAddedForPropertySerializesAllFieldsForSpecifiedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithFieldsWildcardAddedForNestedPropertySerializesAllFieldsForSpecifiedNestedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Movie.*");
            customPropertiesContractResolver.Fields.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Movie.Id")]
        [InlineData("MOVIE.Id")]
        [InlineData("MOVIE.ID")]
        [InlineData("movie.id")]
        public void ConvertingWithExcludeFieldsIsNotCaseSensitive(string field)
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add(field);

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
        public void ConvertingIgnoresUnknownExcludeFields(string unknownField)
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsIsGeneralWildcardDoesNotSerializeAnyProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsContainsOneFieldDoesNotSerializeSpecifiedField()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsContainsSeveralFieldsDoesNotSerializeSpecifiedFields()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("Movie.Id");
            customPropertiesContractResolver.ExcludeFields.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsContainsFieldForNestedPropertyOnlySerializesSpecifiedFields()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("Movie.Title");
            customPropertiesContractResolver.ExcludeFields.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsContainsFieldOfNestedPropertyOnlySerializesSpecifiedFields()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("Movie.Title");
            customPropertiesContractResolver.ExcludeFields.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsWildcardAddedForPropertySerializesAllFieldsForSpecifiedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithExcludeFieldsWildcardAddedForNestedPropertySerializesAllFieldsForSpecifiedNestedProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.ExcludeFields.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingWithPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperty()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Director.Name");
            customPropertiesContractResolver.ExcludeFields.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithWildcardPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("Director.*");
            customPropertiesContractResolver.ExcludeFields.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingWithWildcardBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperties()
        {
            // Arrange
            var customPropertiesContractResolver = new CustomPropertiesContractResolver();
            customPropertiesContractResolver.Fields.Add("*");
            customPropertiesContractResolver.ExcludeFields.Add("*");

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