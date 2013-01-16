namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class TypeWildcardExample : Example
    {
        public override void ShowUsage()
        {
            var movie = CreateMovie();
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "*.Id");
            
            ShowUsageHeader("Example 6: type wildcard usage");
            ShowDefaultSerializationResult(movie);
            ShowInstruction("To exclude all properties named \"Id\" from serialization:");
            ShowCreateExcludePropertiesContractResolver("\"*.Id\"");
            ShowCustomSerializationResult(movie, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}