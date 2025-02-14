using System;
using System.Linq;
using System.Reflection;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    public static class PromptResourceHelper
    {
        public static IEnumerable<PromptResourceData> GetPromptResources(Assembly assembly)
        {
            var resources = assembly.GetManifestResourceNames()
                .Where(x => x.Contains(".Prompts."))
                .ToList();

            foreach (var resource in resources)
            {
                var fileName = resource.Split('.')
                    .SkipWhile(p => !p.Equals("Prompts", StringComparison.OrdinalIgnoreCase))
                    .Skip(1)
                    .First();

                var data = new PromptResourceData { FileName = fileName };

                using (var stream = assembly.GetManifestResourceStream(resource))
                using (var reader = new StreamReader(stream))
                {
                    data.TextContent = reader.ReadToEnd();
                }

                yield return data;
            }
        }

        // Helper method to get a single prompt resource by filename
        public static PromptResourceData GetPromptResource(Assembly assembly, string fileName)
        {
            return GetPromptResources(assembly).FirstOrDefault(r =>
                r.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
