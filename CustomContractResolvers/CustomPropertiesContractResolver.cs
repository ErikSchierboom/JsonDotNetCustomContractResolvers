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
        private const string Wildcard = "*";

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
            if (this.NoFieldsHaveBeenSpecified())
            {
                this.SerializeAllFields();
            }

            var jsonProperty = base.CreateProperty(member, memberSerialization);
            jsonProperty.ShouldSerialize = i => this.PropertyIsIncluded(jsonProperty) && !this.PropertyIsExcluded(jsonProperty);

            return jsonProperty;
        }

        private static bool FieldsContainsProperty(ICollection<string> fields, JsonProperty jsonProperty)
        {
            return fields.Contains(Wildcard) ||
                   fields.Contains(GetWildcardForProperty(jsonProperty)) ||
                   fields.Contains(GetFullName(jsonProperty));
        }

        private static string GetWildcardForProperty(JsonProperty jsonProperty)
        {
            return GetFullNameForTypePropery(jsonProperty.DeclaringType, Wildcard);
        }

        private static string GetFullName(JsonProperty jsonProperty)
        {
            return GetFullNameForTypePropery(jsonProperty.DeclaringType, jsonProperty.PropertyName);
        }

        private static string GetFullNameForTypePropery(MemberInfo declaringType, string propertyName)
        {
            return declaringType.Name + "." + propertyName;
        }

        private void SerializeAllFields()
        {
            this.Fields.Add(Wildcard);
        }

        private bool NoFieldsHaveBeenSpecified()
        {
            return !this.Fields.Any();
        }

        private bool PropertyIsIncluded(JsonProperty jsonProperty)
        {
            return FieldsContainsProperty(this.Fields, jsonProperty);
        }

        private bool PropertyIsExcluded(JsonProperty jsonProperty)
        {
            return FieldsContainsProperty(this.ExcludeFields, jsonProperty);
        }
    }
}