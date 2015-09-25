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
        private const string PropertyTypeAndNameSeparator = ".";

        private PropertiesCollection normalizedProperties;
        private PropertiesCollection normalizedExcludeProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesContractResolver" /> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="excludeProperties">The exclude properties.</param>
        public PropertiesContractResolver(string properties = "", string excludeProperties = "")
        {
            this.Properties = new PropertiesCollection(properties);
            this.ExcludeProperties = new PropertiesCollection(excludeProperties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesContractResolver" /> class.
        /// </summary>
        /// <param name="properties">The properties collection.</param>
        /// <param name="excludeProperties">The exclude properties collection.</param>
        public PropertiesContractResolver(IEnumerable<string> properties, IEnumerable<string> excludeProperties)
        {
            this.Properties = new PropertiesCollection(properties);
            this.ExcludeProperties = new PropertiesCollection(excludeProperties);
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
        public PropertiesCollection Properties { get; }

        /// <summary>
        /// Gets the name of the properties that are not to be serialized.
        /// </summary>
        /// <value>
        /// The names of the properties to exclude.
        /// </value>
        /// <remarks>
        /// If no exclude properties have been specified, all properties will be serialized.
        /// </remarks>
        public PropertiesCollection ExcludeProperties { get; }

        /// <summary>
        /// Gets or sets the property match mode.
        /// </summary>
        /// <value>
        /// The property match mode.
        /// </value>
        public PropertyMatchMode PropertyMatchMode { get; set; }

        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            this.normalizedProperties = this.NormalizeProperties(this.Properties);
            this.normalizedExcludeProperties = this.NormalizeProperties(this.ExcludeProperties);

            if (this.NoPropertiesHaveBeenSpecified())
            {
                this.MarkAllPropertiesForSerialization();
            }

            return base.CreateProperties(type, memberSerialization);
        }

        /// <summary>
        /// Returns a predicate that checks if a JSON property should be serialized.
        /// </summary>
        /// <param name="jsonProperty">The JSON property.</param>
        /// <returns>
        /// The predicate.
        /// </returns>
        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            return i => this.PropertyIsIncluded(jsonProperty) && !this.PropertyIsExcluded(jsonProperty);
        }

        private static bool PropertiesContainsProperty(ICollection<string> properties, JsonProperty jsonProperty)
        {
            return properties.Contains(Wildcard) ||
                   properties.Contains(GetWildcardForType(jsonProperty)) ||
                   properties.Contains(GetWildcardForProperty(jsonProperty)) ||
                   properties.Contains(GetFullName(jsonProperty));
        }

        private static string GetWildcardForType(JsonProperty jsonProperty) => GetFullName(Wildcard, jsonProperty.PropertyName);

        private static string GetWildcardForProperty(JsonProperty jsonProperty) => GetFullName(jsonProperty.DeclaringType, Wildcard);

        private static string GetFullName(JsonProperty jsonProperty) => GetFullName(jsonProperty.DeclaringType, jsonProperty.PropertyName);

        private static string GetFullName(MemberInfo declaringType, string propertyName) => GetFullName(declaringType.Name, propertyName);

        private static string GetFullName(string declaringTypeName, string propertyName) => declaringTypeName + PropertyTypeAndNameSeparator + propertyName;

        private static IEnumerable<string> AddTypeWildcardToNameOnlyProperties(PropertiesCollection properties)
        {
            var propertiesWithNameOnly = properties.Where(IsNameOnlyProperty);
            var propertiesWithWilcardAsType = propertiesWithNameOnly.Select(p => GetFullName(Wildcard, p));

            return properties.Except(propertiesWithNameOnly).Union(propertiesWithWilcardAsType);
        }

        private static bool IsNameOnlyProperty(string p) => p != Wildcard && !p.Contains(PropertyTypeAndNameSeparator);

        private PropertiesCollection NormalizeProperties(PropertiesCollection properties)
        {
            if (this.PropertyMatchMode == PropertyMatchMode.Name)
            {
                return new PropertiesCollection(AddTypeWildcardToNameOnlyProperties(properties));
            }

            return properties;
        }

        private void MarkAllPropertiesForSerialization() => this.normalizedProperties.Add(Wildcard);

        private bool NoPropertiesHaveBeenSpecified() => !this.normalizedProperties.Any();

        private bool PropertyIsIncluded(JsonProperty jsonProperty) => PropertiesContainsProperty(this.normalizedProperties, jsonProperty);

        private bool PropertyIsExcluded(JsonProperty jsonProperty) => PropertiesContainsProperty(this.normalizedExcludeProperties, jsonProperty);
    }
}