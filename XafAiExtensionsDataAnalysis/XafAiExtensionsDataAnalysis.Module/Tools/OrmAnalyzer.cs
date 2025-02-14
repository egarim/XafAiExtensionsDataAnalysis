using DevExpress.ExpressApp;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{

    /// <summary>
    /// Helper class to analyze ORM structures from assemblies
    /// </summary>
    public static class OrmAnalyzer
    {
        public static List<OrmEntityDto> AnalyzeOrm(Assembly assembly, Type baseType)
        {
            var entities = new List<OrmEntityDto>();

            // Get all types that inherit from the base type
            var types = assembly.GetTypes()
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType);

            foreach (var type in types)
            {
                var entity = new OrmEntityDto
                {
                    EntityName = type.Name,
                    Description = type.GetCustomAttribute<DescriptionAttribute>()?.Description
                };

                // Analyze properties
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // Skip XPCollection properties as they're handled in relationships
                    if (prop.PropertyType.IsGenericType &&
                        prop.PropertyType.GetGenericTypeDefinition() == typeof(XPCollection<>))
                        continue;

                    var propertyDto = new OrmPropertyDto
                    {
                        Name = prop.Name,
                        Type = GetFriendlyTypeName(prop.PropertyType),
                        Description = prop.GetCustomAttribute<DescriptionAttribute>()?.Description,
                        IsRequired = !IsNullableType(prop.PropertyType)
                    };

                    // Collect additional attributes
                    foreach (var attr in prop.GetCustomAttributes())
                    {
                        if (attr is SizeAttribute sizeAttr)
                            propertyDto.Attributes["Size"] = sizeAttr.Size.ToString();
                    }

                    entity.Properties.Add(propertyDto);
                }

                // Analyze relationships
                foreach (var prop in type.GetProperties())
                {
                    var associationAttr = prop.GetCustomAttribute<AssociationAttribute>();
                    if (associationAttr != null)
                    {
                        var relationship = new OrmRelationshipDto
                        {
                            Name = prop.Name,
                            SourceEntity = type.Name,
                            AssociationName = associationAttr.Name
                        };

                        if (prop.PropertyType.IsGenericType &&
                            prop.PropertyType.GetGenericTypeDefinition() == typeof(XPCollection<>))
                        {
                            relationship.TargetEntity = prop.PropertyType.GetGenericArguments()[0].Name;
                            relationship.RelationType = RelationshipType.OneToMany;
                        }
                        else
                        {
                            relationship.TargetEntity = prop.PropertyType.Name;
                            relationship.RelationType = RelationshipType.ManyToOne;
                        }

                        entity.Relationships.Add(relationship);
                    }
                }

                entities.Add(entity);
            }

            return entities;
        }

        private static string GetFriendlyTypeName(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"{GetFriendlyTypeName(type.GetGenericArguments()[0])}?";
            }

            var typeMap = new Dictionary<Type, string>
            {
                { typeof(string), "string" },
                { typeof(int), "int" },
                { typeof(decimal), "decimal" },
                { typeof(DateTime), "DateTime" },
                { typeof(bool), "bool" }
            };

            return typeMap.TryGetValue(type, out var friendlyName) ? friendlyName : type.Name;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ||
                   !type.IsValueType;
        }
    }
}
