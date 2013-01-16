namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    internal class NesterPropertiesExample : Example
    {
        public override void ShowUsage()
        {
            var movie = CreateMovie();
            var propertiesContractResolver = new PropertiesContractResolver(properties: "Movie.Title, Movie.Director, Director.Name");

            ShowUsageHeader("Example 4: nested properties usage");
            ShowDefaultSerializationResult(movie);
            ShowInstruction("To serialize the \"Movie.Title\" and \"Director.Name\" properties:");
            ShowCreatePropertiesContractResolver("\"Movie.Title, Movie.Director, Director.Name\"");
            ShowCustomSerializationResult(movie, propertiesContractResolver);
            ShowMoveToNextExample();
        }
    }
}