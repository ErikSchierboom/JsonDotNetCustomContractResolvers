namespace CustomContractResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// A contract resolver that allows the caller to specify exactly which properties are to be serialized by 
    /// using simple strings.
    /// </summary>
    public class CustomPropertiesContractResolver : DefaultContractResolver
    {
        private const string WildcardPropertyName = "*";

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPropertiesContractResolver" /> class.
        /// </summary>
        public CustomPropertiesContractResolver()
        {
            this.Fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            this.ExcludeFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the fields to serialize.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        /// <remarks>
        /// If no fields have been specified, all fields will be serialized.
        /// </remarks>
        public ISet<string> Fields { get; private set; }
        
        /// <summary>
        /// Gets the fields that are not to be serialized.
        /// </summary>
        /// <value>
        /// The exclude fields.
        /// </value>
        /// <remarks>
        /// If no exclude fields have been specified, all fields will be serialized.
        /// </remarks>
        public ISet<string> ExcludeFields { get; private set; }

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

            if (this.Fields.Any())
            {
                jsonProperty.ShouldSerialize = i =>
                    this.Fields.Contains(WildcardPropertyName) ||
                    this.Fields.Contains(GetWildcardName(jsonProperty)) || 
                    this.Fields.Contains(GetFullName(jsonProperty));
            }

            if (this.ExcludeFields.Any())
            {
                jsonProperty.ShouldSerialize = i =>
                    !this.ExcludeFields.Contains(WildcardPropertyName) && 
                    !this.ExcludeFields.Contains(GetWildcardName(jsonProperty)) && 
                    !this.ExcludeFields.Contains(GetFullName(jsonProperty));
            }

            return jsonProperty;
        }

        private static string GetWildcardName(JsonProperty jsonProperty)
        {
            return GetFullNameForTypePropery(jsonProperty.DeclaringType, WildcardPropertyName);
        }

        private static string GetFullName(JsonProperty jsonProperty)
        {
            return GetFullNameForTypePropery(jsonProperty.DeclaringType, jsonProperty.PropertyName);
        }

        private static string GetFullNameForTypePropery(MemberInfo declaringType, string propertyName)
        {
            return declaringType.Name + "." + propertyName;
        }
    }
}