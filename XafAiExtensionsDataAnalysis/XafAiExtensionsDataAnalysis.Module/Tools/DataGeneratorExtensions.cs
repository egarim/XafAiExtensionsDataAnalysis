using DevExpress.ExpressApp;
using System.Linq;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    /// <summary>
    /// Extension method to help with data initialization.
    /// </summary>
    public static class DataGeneratorExtensions
    {
        /// <summary>
        /// Generates sample data if the database is empty.
        /// </summary>
        /// <param name="objectSpace">The object space to use for data generation.</param>
        public static void GenerateDataIfEmpty(this IObjectSpace objectSpace)
        {
            if (!objectSpace.GetObjects<Customer>().Any())
            {
                var generator = new DataGenerator(objectSpace);
                generator.GenerateData();
            }
        }
    }
}
