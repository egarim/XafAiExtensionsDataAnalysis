using System.Linq;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    /// <summary>
    /// Represents a relationship between ORM entities
    /// </summary>
    public class OrmRelationshipDto
    {
        public string Name { get; set; }
        public string SourceEntity { get; set; }
        public string TargetEntity { get; set; }
        public RelationshipType RelationType { get; set; }
        public string RelationShipTypeName { get; set; }
        public string AssociationName { get; set; }
    }
}
