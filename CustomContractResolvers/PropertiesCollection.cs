namespace JsonDotNet.CustomContractResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    public class PropertiesCollection : HashSet<string>
    {
        private const char CommaSeparator = ',';
        private const char SpaceSeparator = ' ';

        public PropertiesCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public PropertiesCollection(IEnumerable<string> collection)
            : base(collection, StringComparer.OrdinalIgnoreCase)
        {
        }

        public PropertiesCollection(string properties)
            : base(ParseProperties(properties), StringComparer.OrdinalIgnoreCase)
        {
            
        }

        protected PropertiesCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public new bool Add(string item)
        {
            return ParseProperties(item).Aggregate(false, (current, property) => current | base.Add(property));
        }

        private static IEnumerable<string> ParseProperties(string properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return properties.Split(CommaSeparator, SpaceSeparator).Where(p => !string.IsNullOrWhiteSpace(properties));
        }
    }
}