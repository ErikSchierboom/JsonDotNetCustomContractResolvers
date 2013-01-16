namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class GeneralWildcardExample : Example
    {
        public override void ShowUsage()
        {
            var director = CreateDirector();
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "*");

            ShowUsageHeader("Example 7: general wildcard usage");
            ShowDefaultSerializationResult(director);
            ShowInstruction("To exclude all properties from serialization:");
            ShowCreateExcludePropertiesContractResolver("\"*\"");
            ShowCustomSerializationResult(director, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}