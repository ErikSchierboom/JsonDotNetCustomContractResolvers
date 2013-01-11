namespace JsonDotNet.CustomContractResolvers
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
    public class PropertiesContractResolver : CustomPropertyContractResolver
    {
        private const string Wildcard = "*";

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesContractResolver" /> class.
        /// </summary>
        public PropertiesContractResolver()
        {
            this.Properties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            this.ExcludeProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the names of the properties to serialize.
        /// </summary>
        /// <value>
        /// The names of the properties to serialize.
        /// </value>
        /// <remarks>
        /// If no properties have been specified, all properties will be serialized.
        /// </remarks>
        public ISet<string> Properties { get; private set; }

        /// <summary>
        /// Gets the name of the properties that are not to be serialized.
        /// </summary>
        /// <value>
        /// The names of the properties to exclude.
        /// </value>
        /// <remarks>
        /// If no exclude properties have been specified, all properties will be serialized.
        /// </remarks>
        public ISet<string> ExcludeProperties { get; private set; }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization);
        }

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

            return base.CreateProperty(member, memberSerialization);
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            return i => this.PropertyIsIncluded(jsonProperty) && !this.PropertyIsExcluded(jsonProperty);
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
            this.Properties.Add(Wildcard);
        }

        private bool NoFieldsHaveBeenSpecified()
        {
            return !this.Properties.Any();
        }

        private bool PropertyIsIncluded(JsonProperty jsonProperty)
        {
            return FieldsContainsProperty(this.Properties, jsonProperty);
        }

        private bool PropertyIsExcluded(JsonProperty jsonProperty)
        {
            return FieldsContainsProperty(this.ExcludeProperties, jsonProperty);
        }
    }
}