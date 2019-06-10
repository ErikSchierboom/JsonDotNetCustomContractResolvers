namespace JsonDotNet.CustomContractResolvers.Samples.Console.Examples
{
    using System;

    using Models;

    using Newtonsoft.Json;

    internal abstract class Example
    {
        private const string Separator = "----------------------------------------------";

        public abstract void ShowUsage();

        protected static void ShowUsageHeader(string usageName)
        {
            Console.WriteLine("");
            Console.WriteLine(Separator);
            Console.WriteLine(usageName);
        }

        protected static void ShowInstruction(string instruction)
        {
            Console.WriteLine(instruction);
        }

        protected static void ShowMoveToNextExample()
        {
            Console.WriteLine(Separator);
            Console.WriteLine("");
            Console.WriteLine("Press any key to start showing the next example.");
            Console.ReadKey();
        }

        protected static void ShowCreatePropertiesContractResolver(string properties)
        {
            Console.WriteLine("new PropertiesContractResolver(properties: {0});", properties);
        }

        protected static void ShowCreateExcludePropertiesContractResolver(string excludeProperties)
        {
            Console.WriteLine("new PropertiesContractResolver(excludeProperties: {0});", excludeProperties);
        }

        protected static void ShowDefaultSerializationResult(object obj)
        {
            Console.WriteLine("Default serialization result: {0}.", JsonConvert.SerializeObject(obj));
            Console.WriteLine("");
        }

        protected static void ShowCustomSerializationResult(object obj, PropertiesContractResolver propertiesContractResolver)
        {
            var customJsonSerializerSettings = new JsonSerializerSettings { ContractResolver = propertiesContractResolver };

            Console.WriteLine("");
            Console.WriteLine("Custom serialization result: {0}", JsonConvert.SerializeObject(obj, customJsonSerializerSettings));
        }

        protected static Director CreateDirector() =>
            new Director
            {
                Id = 79,
                Name = "Christopher Nolan"
            };

        protected static Movie CreateMovie() =>
            new Movie
            {
                Id = 12,
                Title = "Inception",
                Director = CreateDirector()
            };
    }
}