using System;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;

namespace YAMLValidator
{
    class Program
    {
        public static void Main()
        {
            // Input yaml file
            string yamlFile = @"C:\Users\Bilal\Desktop\New folder (2)\openapi.yaml";

            // List of input rules
            Dictionary<string, Tuple<string, string>> inputRules = new Dictionary<string, Tuple<string, string>>
            {
                {"tags.externalDocs.url",Tuple.Create("string", @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")},
               // {"openapi", Tuple.Create("string", "")},
               // {"info.title", Tuple.Create("int", "")},
               // {"info.contact.email", Tuple.Create("string", "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")},
               // {"servers.url", Tuple.Create("string", @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")}
            };

            // Parse yaml file
            var yaml = new YamlStream();
            using (var reader = new StreamReader(yamlFile))
            {
                yaml.Load(reader);
            }

            // Get the root node
            var rootNode = yaml.Documents[0].RootNode;

            // Iterate through the input rules and validate against the yaml file
            foreach (var rule in inputRules)
            {
                var tagName = rule.Key;
                var expectedType = rule.Value.Item1;
                var regex = rule.Value.Item2;
                Console.WriteLine("\n tagName:{0}, expectedType :{1}, regex: {2}",tagName, expectedType, regex);
                var results = GenericRuleEngine.ValidateRule(rootNode, tagName, expectedType, regex);
                foreach (var result in results)
                {
                    if (result.Item1)
                    {
                        Console.WriteLine(tagName + " validation succeeded");
                    }
                    else
                    {
                        Console.WriteLine(tagName + " validation failed at lines " + result.Item2 + " - " + result.Item3);
                    }

                }
               
            }
            Console.ReadKey();

        }
    }
}
