namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class SimpleExcludeExample : Example
    {
        public override void ShowUsage()
        {
            var director = CreateDirector();
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Name");
            
            ShowUsageHeader("Example 2: simple exclude usage");
            ShowDefaultSerializationResult(director);
            ShowInstruction("To serialize everything but the \"Name\" property do:");
            ShowCreateExcludePropertiesContractResolver("\"Name\"");
            ShowCustomSerializationResult(director, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}