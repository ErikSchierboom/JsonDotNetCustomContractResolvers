namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class ExplicitTypeExample : Example
    {
        public override void ShowUsage()
        {
            var director = CreateDirector();
            var propertiesContractResolver = new PropertiesContractResolver(excludeProperties: "Director.Name");
            
            ShowUsageHeader("Example 3: explicit type usage");
            ShowDefaultSerializationResult(director);
            ShowInstruction("To serialize everything but the \"Name\" property of the \"Director\" type do:");
            ShowCreatePropertiesContractResolver("\"Director.Name\"");
            ShowCustomSerializationResult(director, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}