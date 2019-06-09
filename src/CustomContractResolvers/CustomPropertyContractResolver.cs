namespace JsonDotNet.CustomContractResolvers
{
    using System;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Base class for creating a custom contract resolver that looks at each property to see if
    /// it is allowed to be serialized.
    /// </summary>
    public abstract class CustomPropertyContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>
        /// A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            jsonProperty.ShouldSerialize = ShouldSerialize(jsonProperty);

            return jsonProperty;
        }

        /// <summary>
        /// Returns a predicate that checks if a JSON property should be serialized.
        /// </summary>
        /// <param name="jsonProperty">The JSON property.</param>
        /// <returns>The predicate.</returns>
        protected abstract Predicate<object> ShouldSerialize(JsonProperty jsonProperty);
    }
}