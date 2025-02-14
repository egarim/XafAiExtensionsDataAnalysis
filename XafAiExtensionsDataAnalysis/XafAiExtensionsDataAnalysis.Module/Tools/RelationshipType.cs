using System.Linq;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    /// <summary>
    /// Defines the type of relationship between entities
    /// </summary>
    public enum RelationshipType
    {
        OneToOne,
        OneToMany,
        ManyToOne,
        ManyToMany
    }
}
