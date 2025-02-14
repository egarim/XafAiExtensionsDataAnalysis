using System;
using System.Linq;
using System.Reflection;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    public static class ReportResourceHelper
    {
        public static IEnumerable<ReportResourceData> GetReportResources(Assembly assembly)
        {
            var resources = assembly.GetManifestResourceNames()
                .Where(x => x.Contains(".Reports."))
                .ToList();

            // Group resources by folder name
            var folders = resources
                .Select(r => r.Split('.'))
                .Select(parts => parts.SkipWhile(p => !p.Equals("Reports", StringComparison.OrdinalIgnoreCase))
                               .Skip(1)
                               .First())
                .Distinct();

            foreach (var folder in folders)
            {
                var data = new ReportResourceData { FolderName = folder };

                // Get JSON content
                var jsonResource = resources.FirstOrDefault(r => r.Contains($".Reports.{folder}.") && r.EndsWith(".json"));
                if (jsonResource != null)
                {
                    using (var stream = assembly.GetManifestResourceStream(jsonResource))
                    using (var reader = new StreamReader(stream))
                    {
                        data.JsonContent = reader.ReadToEnd();
                    }
                }

                // Get Image content
                var imageResource = resources.FirstOrDefault(r => r.Contains($".Reports.{folder}.") &&
                    (r.EndsWith(".png") || r.EndsWith(".jpg") || r.EndsWith(".jpeg")));
                if (imageResource != null)
                {
                    using (var stream = assembly.GetManifestResourceStream(imageResource))
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        data.ImageContent = memoryStream.ToArray();
                    }
                }

                // Get Prompt content
                var promptResource = resources.FirstOrDefault(r => r.Contains($".Reports.{folder}.") && r.EndsWith(".prompt"));
                if (promptResource != null)
                {
                    using (var stream = assembly.GetManifestResourceStream(promptResource))
                    using (var reader = new StreamReader(stream))
                    {
                        data.PromptContent = reader.ReadToEnd();
                    }
                }

                yield return data;
            }
        }

        // Helper method to get a single report resource by folder name
        public static ReportResourceData GetReportResource(Assembly assembly, string folderName)
        {
            return GetReportResources(assembly).FirstOrDefault(r => r.FolderName.Equals(folderName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
