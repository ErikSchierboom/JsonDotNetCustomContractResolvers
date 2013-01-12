namespace JsonDotNet.CustomContractResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class PropertiesCollection : HashSet<string>
    {
        public PropertiesCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public PropertiesCollection(IEnumerable<string> collection)
            : base(collection, StringComparer.OrdinalIgnoreCase)
        {
        }

        protected PropertiesCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}