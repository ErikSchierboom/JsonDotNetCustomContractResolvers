namespace JsonDotNet.CustomContractResolvers.Samples.Console
{
    using System;
    using System.Collections.Generic;

    using Examples;

    internal class Program
    {
        private static void Main()
        {
            ShowExplanation();

            foreach (var example in GetExamples())
            {
                example.ShowUsage();
            }
        }

        private static void ShowExplanation()
        {
            Console.WriteLine("This console application will show how the [PropertiesContractResolver] class can be used.");
            Console.WriteLine("");
            Console.WriteLine("Press any key to start showing the first example.");
            Console.ReadKey();
        }

        private static IEnumerable<Example> GetExamples()
        {
            return new List<Example>
                       {
                           new SimpleExample(),
                           new SimpleExcludeExample(),
                           new ExplicitTypeExample(),
                           new NesterPropertiesExample(),
                           new PropertyWildcardExample(),
                           new TypeWildcardExample(),
                           new GeneralWildcardExample()
                       };
        }
    }
}