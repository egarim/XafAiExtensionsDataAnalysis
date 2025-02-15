
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace XafAiExtensionsDataAnalysis.Module.Ai.PivotChart
{

    [Description("Represents a configuration for a pivot chart analysis, defining data structure and visualization settings")]
    public class PivotConfiguration
    {
        [Description("Unique identifier for the pivot configuration")]
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Description("Display name of the pivot configuration")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Description("Detailed description of what this pivot configuration analyzes")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Description("Full type name of the entity being analyzed")]
        [JsonPropertyName("entityFullName")]
        public string EntityFullName { get; set; }

        [Description("Display caption for the analyzed entity")]
        [JsonPropertyName("entityCaption")]
        public string EntityCaption { get; set; }

        [Description("Title displayed on the pivot chart")]
        [JsonPropertyName("pivotTitle")]
        public string PivotTitle { get; set; }

        [Description("Fields used for data calculations in the pivot")]
        [JsonPropertyName("dataFields")]
        public List<PivotField> DataFields { get; set; } = new();

        [Description("Fields displayed as rows in the pivot")]
        [JsonPropertyName("rowFields")]
        public List<PivotField> RowFields { get; set; } = new();

        [Description("Fields displayed as columns in the pivot")]
        [JsonPropertyName("columnFields")]
        public List<PivotField> ColumnFields { get; set; } = new();

        [Description("Fields used for filtering the pivot data")]
        [JsonPropertyName("filterFields")]
        public List<PivotField> FilterFields { get; set; } = new();
    }
}
