using System.Collections.Generic;
using System.ComponentModel;

using System.Text.Json.Serialization;
namespace XafAiExtensionsDataAnalysis.Module.Ai.PivotChart
{
   

    [Description("Defines filtering and totals display configuration for a pivot field")]
    public class FilterSettings
    {
        [Description("Type of filter to apply (Value, Set, TopN)")]
        [JsonPropertyName("filterType")]
        public string FilterType { get; set; }

        [Description("Value to use for filtering when FilterType is Value or TopN")]
        [JsonPropertyName("filterValue")]
        public object FilterValue { get; set; }

        [Description("Determines if grand totals should be displayed in the pivot view")]
        [JsonPropertyName("showGrandTotals")]
        public bool ShowGrandTotals { get; set; } = true;

        [Description("Determines if column totals should be displayed in the pivot view")]
        [JsonPropertyName("showColumnTotals")]
        public bool ShowColumnTotals { get; set; } = true;

        [Description("Determines if row totals should be displayed in the pivot view")]
        [JsonPropertyName("showRowTotals")]
        public bool ShowRowTotals { get; set; } = true;

        [Description("Collection of specific values to include/exclude when FilterType is Set")]
        [JsonPropertyName("filteredValues")]
        public List<object> FilteredValues { get; set; }
    }
}
