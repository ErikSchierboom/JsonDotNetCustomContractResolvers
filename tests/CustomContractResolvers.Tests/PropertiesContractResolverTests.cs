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
        public void ConstructorWithNullPropertiesCollectionThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<string> properties = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(properties, new string[0]));
        }

        [Fact]
        public void ConstructorWithNullExcludePropertiesCollectionThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<string> excludeProperties = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(new string[0], excludeProperties));
        }

        [Fact]
        public void ConstructorWithNullPropertiesStringThrowsArgumentNullException()
        {
            // Arrange
            string properties = null;
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(properties: properties));
        }

        [Fact]
        public void ConstructorWithNullExcludePropertiesStringThrowsArgumentNullException()
        {
            // Arrange
            string excludeProperties = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new PropertiesContractResolver(excludeProperties: excludeProperties));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void ConstructorWithWhiteSpacePropertiesStringSetsPropertiesToEmptySet(string whiteSpaceProperties)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: whiteSpaceProperties);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.Properties.Any());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void ConstructorWithWhiteSpaceExcludePropertiesStringSetsExcludePropertiesToEmptySet(string whiteSpaceExcludeProperties)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: whiteSpaceExcludeProperties);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.ExcludeProperties.Any());
        }

        [Fact]
        public void ConstructorWithNoPropertiesStringSetsPropertiesToEmptySet()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: string.Empty);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.Properties.Any());
        }

        [Fact]
        public void ConstructorWithNoExcludePropertiesStringSetsExcludePropertiesToEmptySet()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: string.Empty);

            // Act

            // Assert
            Assert.False(propertiesContractResolver.ExcludeProperties.Any());
        }

        [Fact]
        public void ConstructorWithPropertiesStringWithOnePropertyAddsThatProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Id");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Id"));
        }

        [Fact]
        public void ConstructorWithExcludePropertiesStringWithOnePropertyAddsThatProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Movie.Id");

            // Act

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Id"));
        }

        [Theory]
        [InlineData("Movie.Id,Movie.Title")]
        [InlineData("Movie.Id Movie.Title")]
        [InlineData("Movie.Id, Movie.Title")]
        [InlineData("Movie.Id,\tMovie.Title")]
        [InlineData(" Movie.Id,Movie.Title ")]
        public void ConstructorWithPropertiesSetToStringWithSeveralPropertiesAddsThoseProperties(string properties)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(properties: properties);

            // Act

            // Assert
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.Properties.Contains("Movie.Title"));
        }

        [Theory]
        [InlineData("Movie.Id,Movie.Title")]
        [InlineData("Movie.Id Movie.Title")]
        [InlineData("Movie.Id, Movie.Title")]
        [InlineData("Movie.Id,\tMovie.Title")]
        [InlineData(" Movie.Id,Movie.Title ")]
        public void ConstructorWithExcludePropertiesSetToStringAddsThoseProperties(string excludeProperties)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: excludeProperties);

            // Act

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Id"));
            Assert.True(propertiesContractResolver.ExcludeProperties.Contains("Movie.Title"));
        }
        
        [Fact]
        public void ConstructorWithStringPropertiesSetsPropertyMatchModeToName()
        {
            // Arrange

            // Act
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Id", excludeProperties: "Movie.Title");

            // Assert
            Assert.Equal(PropertyMatchMode.Name, propertiesContractResolver.PropertyMatchMode);
        }

        [Fact]
        public void ConstructorWithCollectionPropertiesSetsPropertyMatchModeToName()
        {
            // Arrange

            // Act
            var propertiesContractResolver = new PropertiesContractResolver(properties: new[] { "Movie.Id" }, excludeProperties: new[] { "Movie.Title" });

            // Assert
            Assert.Equal(PropertyMatchMode.Name, propertiesContractResolver.PropertyMatchMode);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingUsingWildcardsDoesNotModifyPropertiesCollection(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.Properties.Add("*");
            propertiesContractResolver.Properties.Add("*.Title");
            propertiesContractResolver.Properties.Add("Title");
            propertiesContractResolver.Properties.Add("Title.*");

            // Act
            JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.True(propertiesContractResolver.Properties.SequenceEqual(new[] { "*", "*.Title", "Title", "Title.*"}));
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingUsingWildcardsDoesNotModifyExcludePropertiesCollection(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("*");
            propertiesContractResolver.ExcludeProperties.Add("*.Title");
            propertiesContractResolver.ExcludeProperties.Add("Title");
            propertiesContractResolver.ExcludeProperties.Add("Title.*");

            // Act
            JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.True(propertiesContractResolver.ExcludeProperties.SequenceEqual(new[] { "*", "*.Title", "Title", "Title.*" }));
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithPropertiesAndExcludePropertiesAreEmptySerializesObjectLikeDefault(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var objectToSerialize = CreateObjectToSerialize();
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithPropertiesIsGeneralWildcardSerializesObjectLikeDefault(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var objectToSerialize = CreateObjectToSerialize();

            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.Properties.Add("*");

            // Act
            var jsonUsingDefaultSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateDefaultJsonSerializerSettings());
            var jsonUsingCustomSerializer = JsonConvert.SerializeObject(objectToSerialize, CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal(jsonUsingDefaultSerializer, jsonUsingCustomSerializer);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWithPropertiesContainsOneFieldOnlySerializesSpecifiedField()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWillIgnoreTypeOfProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWillIgnoreUnknownTypes()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Dummy.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWillOnlyMatchWhenTypeAndNameMatch()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWillNotMatchUnknownTypeProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Dummy.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWillIgnorePropertiesWithoutType()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Theory]
        [InlineData("Movie.Id")]
        [InlineData("MOVIE.Id")]
        [InlineData("MOVIE.ID")]
        [InlineData("movie.id")]
        public void ConvertingUsingNamePropertyMatchModeWithPropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add(field);

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
        public void ConvertingUsingNameAndTypePropertyMatchModeWithPropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Theory]
        [InlineData("Inventory")]
        [InlineData("Id2")]
        [InlineData("Id_")]
        public void ConvertingUsingNameMatchModeWithOnlyUnknownPropertiesReturnsEmptyObject(string unknownField)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Theory]
        [InlineData("Movie.Inventory")]
        [InlineData("Movie.Id2")]
        [InlineData("Movie.Id_")]
        [InlineData("_Movie.Id")]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithOnlyUnknownPropertiesReturnsEmptyObjct(string unknownField)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithPropertiesWithOnlyPropertyNamesOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Id");
            propertiesContractResolver.Properties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithPropertiesWithPropertyNamesAndTypeSerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Movie.Id");
            propertiesContractResolver.Properties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithPropertiesWithPropertyNamesAndTypeOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Movie.Id");
            propertiesContractResolver.Properties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\"}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithPropertiesWithPropertyNamesOnlyDoesNotSerializeSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Id");
            propertiesContractResolver.Properties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithPropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Title");
            propertiesContractResolver.Properties.Add("Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithPropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Movie.Title");
            propertiesContractResolver.Properties.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithPropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Title");
            propertiesContractResolver.Properties.Add("Director");
            propertiesContractResolver.Properties.Add("Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithPropertiesContainsPropertyNameUsedInRootAndNestedPropertySerializesForAllTypes()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Id");
            propertiesContractResolver.Properties.Add("Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithPropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Movie.Title");
            propertiesContractResolver.Properties.Add("Movie.Director");
            propertiesContractResolver.Properties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithPropertiesWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.Properties.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithPropertiesWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.Properties.Add("Movie.*");
            propertiesContractResolver.Properties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithTypeWildcardAddedForPropertySerializesAllPropertiesWithSpecifiedPropertyName(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            customPropertiesContractResolver.Properties.Add("*.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\"}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithTypeWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            customPropertiesContractResolver.Properties.Add("*.Id");
            customPropertiesContractResolver.Properties.Add("*.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Theory]
        [InlineData("Id")]
        [InlineData("Id")]
        [InlineData("ID")]
        [InlineData("id")]
        public void ConvertingUsingNamePropertyMatchModeWithExcludePropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Movie.Id")]
        [InlineData("MOVIE.Id")]
        [InlineData("MOVIE.ID")]
        [InlineData("movie.id")]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithExcludePropertiesIsNotCaseSensitive(string field)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add(field);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Inventory")]
        [InlineData("Id2")]
        [InlineData("Id_")]
        public void ConvertingUsingNamePropertyMatchModeIgnoresUnknownExcludeProperties(string unknownField)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData("Movie.Inventory")]
        [InlineData("Movie.Id2")]
        [InlineData("Movie.Id_")]
        [InlineData("_Movie.Id")]
        public void ConvertingUsingNameAndTypePropertyMatchModeIgnoresUnknownExcludeProperties(string unknownField)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add(unknownField);

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithExcludePropertiesIsGeneralWildcardDoesNotSerializeAnyProperties(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.ExcludeProperties.Add("*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWithExcludePropertiesWithPropertyNameOnlyDoesNotSerializeSpecifiedFields()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add("Id");
            propertiesContractResolver.ExcludeProperties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWithExcludePropertiesWithPropertyTypeAndNameDoesNotSerializeSpecifiedFields()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add("Movie.Id");
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithExcludePropertiesWithPropertyTypeAndNameDoesNotSerializeSpecifiedFields()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Movie.Id");
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithExcludePropertiesWithoutDeclaringTypeNameWillSerializeSpecifiedFields()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Id");
            propertiesContractResolver.ExcludeProperties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWithExcludePropertiesContainsOneFieldDoesNotSerializeSpecifiedField()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add("Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithExcludePropertiesContainsOneFieldDoesNotSerializeSpecifiedField()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Movie.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithExcludePropertiesContainsSeveralPropertiesDoesNotSerializeSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add("Id");
            propertiesContractResolver.ExcludeProperties.Add("Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithExcludePropertiesContainsSeveralPropertiesDoesNotSerializeSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Movie.Id");
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNamePropertyMatchModeWithExcludePropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.ExcludeProperties.Add("Title");
            propertiesContractResolver.ExcludeProperties.Add("Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithExcludePropertiesContainsFieldForNestedPropertyOnlySerializesSpecifiedProperties()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");
            propertiesContractResolver.ExcludeProperties.Add("Movie.Director");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithExcludePropertiesContainsFieldOfNestedPropertyOnlySerializesSpecifiedProperties(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.ExcludeProperties.Add("Movie.Title");
            propertiesContractResolver.ExcludeProperties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithExcludePropertiesWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Movie.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithExcludePropertiesWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.ExcludeProperties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Title\":\"Inception\",\"Director\":{}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithExcludePropertiesTypeWildcardAddedForPropertySerializesAllPropertiesForSpecifiedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            customPropertiesContractResolver.ExcludeProperties.Add("*.Title");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Id\":12,\"Director\":{\"Id\":77,\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithExcludePropertiesTypeWildcardAddedForNestedPropertySerializesAllPropertiesForSpecifiedNestedProperty(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var customPropertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            customPropertiesContractResolver.ExcludeProperties.Add("*.Id");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(customPropertiesContractResolver));

            // Assert
            Assert.Equal("{\"Title\":\"Inception\",\"Director\":{\"Name\":\"Christopher Nolan\"}}", json);
        }

        [Fact]
        public void ConvertingUsingNameAndTypePropertyMatchModeWithPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.NameAndType };
            propertiesContractResolver.Properties.Add("Director.Name");
            propertiesContractResolver.ExcludeProperties.Add("Director.Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Fact]
        public void ConvertingUsingNameMatchModeWithPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeProperty()
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = PropertyMatchMode.Name };
            propertiesContractResolver.Properties.Add("Name");
            propertiesContractResolver.ExcludeProperties.Add("Name");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithWildcardPropertyBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeAnything(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
            propertiesContractResolver.Properties.Add("Director.*");
            propertiesContractResolver.ExcludeProperties.Add("Director.*");

            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), CreateCustomJsonSerializerSettings(propertiesContractResolver));

            // Assert
            Assert.Equal("{}", json);
        }

        [Theory]
        [InlineData(PropertyMatchMode.Name)]
        [InlineData(PropertyMatchMode.NameAndType)]
        public void ConvertingWithWildcardBeingSpecifiedAsBothFieldAndExcludeFieldWillNotSerializeAnything(PropertyMatchMode propertyMatchMode)
        {
            // Arrange
            var propertiesContractResolver = new PropertiesContractResolver { PropertyMatchMode = propertyMatchMode };
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