using System.Linq;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    /// <summary>
    /// Represents a property in an ORM entity
    /// </summary>
    public class OrmPropertyDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string TypeFullName { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
    }
}
