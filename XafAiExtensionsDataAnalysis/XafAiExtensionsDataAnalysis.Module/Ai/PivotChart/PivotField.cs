using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace XafAiExtensionsDataAnalysis.Module.Ai.PivotChart
{
    [Description("Represents a field configuration for pivot chart analysis, defining how a specific data field should be displayed and processed")]
    public class PivotField
    {
        [Description("Name of the property in the data source")]
        [JsonPropertyName("propertyName")]
        public string PropertyName { get; set; }

        [Description("Display caption for the field in the pivot view")]
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [Description("Location of the field in the pivot (Data, Row, Column, or Filter)")]
        [JsonPropertyName("area")]
        public string Area { get; set; }

        [Description("Position index of the field within its area")]
        [JsonPropertyName("areaIndex")]
        public int AreaIndex { get; set; }

        [Description("Type of summary calculation to apply to the field")]
        [JsonPropertyName("summaryType")]
        public PivotSummaryTypeInfo SummaryType { get; set; }

        [Description("Display format string for the field values")]
        [JsonPropertyName("format")]
        public string Format { get; set; }

        [Description("Indicates if the field values are expanded in the pivot view")]
        [JsonPropertyName("isExpanded")]
        public bool IsExpanded { get; set; }

        [Description("Sort direction for the field values")]
        [JsonPropertyName("sortOrder")]
        public SortOrder? SortOrder { get; set; }

        [Description("Filter configuration settings for the field")]
        [JsonPropertyName("filterSettings")]
        public FilterSettings FilterSettings { get; set; }

        [Description("Layout and display settings for the field")]
        [JsonPropertyName("layoutSettings")]
        public LayoutSettings LayoutSettings { get; set; }
    }
}
