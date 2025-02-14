using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.ComponentModel;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    /// <summary>
    /// Represents the structure of an ORM entity
    /// </summary>
    public class OrmEntityDto
    {
        public string EntityName { get; set; }
        public string Description { get; set; }
        public List<OrmPropertyDto> Properties { get; set; } = new();
        public List<OrmRelationshipDto> Relationships { get; set; } = new();
    }
}
