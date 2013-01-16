namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class PropertyWildcardExample : Example
    {
        public override void ShowUsage()
        {
            var movie = CreateMovie();
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.*");
            
            ShowUsageHeader("Example 5: property wildcard usage");
            ShowDefaultSerializationResult(movie);
            ShowInstruction("To output all properties of the \"Movie\" type from serialization:");
            ShowCreatePropertiesContractResolver("\"Movie.*\"");
            ShowCustomSerializationResult(movie, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}