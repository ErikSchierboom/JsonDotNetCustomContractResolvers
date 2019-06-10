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

        private PropertiesCollection _normalizedProperties;
        private PropertiesCollection _normalizedExcludeProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesContractResolver" /> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="excludeProperties">The exclude properties.</param>
        public PropertiesContractResolver(string properties = "", string excludeProperties = "")
        {
            Properties = new PropertiesCollection(properties);
            ExcludeProperties = new PropertiesCollection(excludeProperties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesContractResolver" /> class.
        /// </summary>
        /// <param name="properties">The properties collection.</param>
        /// <param name="excludeProperties">The exclude properties collection.</param>
        public PropertiesContractResolver(IEnumerable<string> properties, IEnumerable<string> excludeProperties)
        {
            Properties = new PropertiesCollection(properties);
            ExcludeProperties = new PropertiesCollection(excludeProperties);
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
            _normalizedProperties = NormalizeProperties(Properties);
            _normalizedExcludeProperties = NormalizeProperties(ExcludeProperties);

            if (NoPropertiesHaveBeenSpecified())
            {
                MarkAllPropertiesForSerialization();
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
        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty) =>
            i => PropertyIsIncluded(jsonProperty) && !PropertyIsExcluded(jsonProperty);

        private static bool PropertiesContainsProperty(ICollection<string> properties, JsonProperty jsonProperty) =>
            properties.Contains(Wildcard) ||
            properties.Contains(GetWildcardForType(jsonProperty)) ||
            properties.Contains(GetWildcardForProperty(jsonProperty)) ||
            properties.Contains(GetFullName(jsonProperty));

        private static string GetWildcardForType(JsonProperty jsonProperty) =>
            GetFullName(Wildcard, jsonProperty.PropertyName);

        private static string GetWildcardForProperty(JsonProperty jsonProperty) =>
            GetFullName(jsonProperty.DeclaringType, Wildcard);

        private static string GetFullName(JsonProperty jsonProperty) =>
            GetFullName(jsonProperty.DeclaringType, jsonProperty.PropertyName);

        private static string GetFullName(MemberInfo declaringType, string propertyName) =>
            GetFullName(declaringType.Name, propertyName);

        private static string GetFullName(string declaringTypeName, string propertyName) =>
            declaringTypeName + PropertyTypeAndNameSeparator + propertyName;

        private static IEnumerable<string> AddTypeWildcardToNameOnlyProperties(PropertiesCollection properties)
        {
            var propertiesWithNameOnly = properties.Where(IsNameOnlyProperty).ToList();
            var propertiesWithWilcardAsType = propertiesWithNameOnly.Select(p => GetFullName(Wildcard, p));

            return properties.Except(propertiesWithNameOnly).Union(propertiesWithWilcardAsType);
        }

        private static bool IsNameOnlyProperty(string p) =>
            p != Wildcard && !p.Contains(PropertyTypeAndNameSeparator);

        private PropertiesCollection NormalizeProperties(PropertiesCollection properties)
        {
            if (PropertyMatchMode == PropertyMatchMode.Name)
            {
                return new PropertiesCollection(AddTypeWildcardToNameOnlyProperties(properties));
            }

            return properties;
        }

        private void MarkAllPropertiesForSerialization() => _normalizedProperties.Add(Wildcard);

        private bool NoPropertiesHaveBeenSpecified() => !_normalizedProperties.Any();

        private bool PropertyIsIncluded(JsonProperty jsonProperty) =>
            PropertiesContainsProperty(_normalizedProperties, jsonProperty);

        private bool PropertyIsExcluded(JsonProperty jsonProperty) =>
            PropertiesContainsProperty(_normalizedExcludeProperties, jsonProperty);
    }
}