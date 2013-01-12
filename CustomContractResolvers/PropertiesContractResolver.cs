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
        /// <param name="properties">The properties.</param>
        /// <param name="excludeProperties">The exclude properties.</param>
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
        public PropertiesCollection Properties { get; private set; }

        /// <summary>
        /// Gets the name of the properties that are not to be serialized.
        /// </summary>
        /// <value>
        /// The names of the properties to exclude.
        /// </value>
        /// <remarks>
        /// If no exclude properties have been specified, all properties will be serialized.
        /// </remarks>
        public PropertiesCollection ExcludeProperties { get; private set; }

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
            NormalizeRootProperties(type, this.Properties);
            NormalizeRootProperties(type, this.ExcludeProperties);

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
            if (this.NoPropertiesHaveBeenSpecified())
            {
                this.SerializeAllProperties();
            }

            return base.CreateProperty(member, memberSerialization);
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            return i => this.PropertyIsIncluded(jsonProperty) && !this.PropertyIsExcluded(jsonProperty);
        }

        private static void NormalizeRootProperties(Type type, ISet<string> properties)
        {
            var rootProperties = properties.Where(IsRootProperty).ToList();

            foreach (var rootProperty in rootProperties)
            {
                var fullNameForRootProperty = GetFullNameForTypeProperty(type, rootProperty);
                properties.Add(fullNameForRootProperty);
                properties.Remove(rootProperty);
            }
        }

        private static bool IsRootProperty(string p)
        {
            return p != Wildcard && !p.Contains(PropertyTypeAndNameSeparator);
        }

        private static bool PropertiesContainsProperty(ICollection<string> properties, JsonProperty jsonProperty)
        {
            return properties.Contains(Wildcard) ||
                   properties.Contains(GetWildcardForProperty(jsonProperty)) ||
                   properties.Contains(GetFullName(jsonProperty));
        }

        private static string GetWildcardForProperty(JsonProperty jsonProperty)
        {
            return GetFullNameForTypeProperty(jsonProperty.DeclaringType, Wildcard);
        }

        private static string GetFullName(JsonProperty jsonProperty)
        {
            return GetFullNameForTypeProperty(jsonProperty.DeclaringType, jsonProperty.PropertyName);
        }

        private static string GetFullNameForTypeProperty(MemberInfo declaringType, string propertyName)
        {
            return declaringType.Name + PropertyTypeAndNameSeparator + propertyName;
        }

        private void SerializeAllProperties()
        {
            this.Properties.Add(Wildcard);
        }

        private bool NoPropertiesHaveBeenSpecified()
        {
            return !this.Properties.Any();
        }

        private bool PropertyIsIncluded(JsonProperty jsonProperty)
        {
            return PropertiesContainsProperty(this.Properties, jsonProperty);
        }

        private bool PropertyIsExcluded(JsonProperty jsonProperty)
        {
            return PropertiesContainsProperty(this.ExcludeProperties, jsonProperty);
        }
    }
}