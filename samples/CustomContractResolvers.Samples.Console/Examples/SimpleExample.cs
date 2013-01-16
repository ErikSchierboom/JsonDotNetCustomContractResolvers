namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class SimpleExample : Example
    {
        public override void ShowUsage()
        {
            var director = CreateDirector();
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Name");

            ShowUsageHeader("Example 1: simple usage");
            ShowDefaultSerializationResult(director);
            ShowInstruction("To only serialize the \"Name\" property do:");
            ShowCreatePropertiesContractResolver("\"Name\"");
            ShowCustomSerializationResult(director, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}