using System;
using System.Linq;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    public class ReportResourceData
    {
        public string FolderName { get; set; }
        public string JsonContent { get; set; }
        public byte[] ImageContent { get; set; }
        public string PromptContent { get; set; }
    }
}
