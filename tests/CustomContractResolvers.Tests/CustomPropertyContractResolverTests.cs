namespace JsonDotNet.CustomContractResolvers.Tests
{
    using System;

    using JsonDotNet.CustomContractResolvers.Tests.Stubs;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Xunit;

    public class CustomPropertyContractResolverTests
    {
        [Fact]
        public void ShouldSerializeMethodUsedWhenObjectIsSerialized()
        {
            // Arrange
            var customPropertiesContractResolver = new TestableCustomPropertyContractResolver();
            
            // Act
            var json = JsonConvert.SerializeObject(CreateObjectToSerialize(), new JsonSerializerSettings { ContractResolver = customPropertiesContractResolver });

            // Assert
            Assert.Equal("{}", json);
        }

        private static Movie CreateObjectToSerialize() => new Movie { Id = 12, Title = "Inception" };

        private class TestableCustomPropertyContractResolver : CustomPropertyContractResolver
        {
            protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty) => x => false;
        }
    }
}