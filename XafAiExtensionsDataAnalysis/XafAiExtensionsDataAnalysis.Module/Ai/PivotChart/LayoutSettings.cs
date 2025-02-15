using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace XafAiExtensionsDataAnalysis.Module.Ai.PivotChart
{
    [Description("Defines the visual and behavioral layout settings for pivot field display")]
    public class LayoutSettings
    {
        [Description("Width of the field column (can be pixel value or percentage)")]
        [JsonPropertyName("width")]
        public string Width { get; set; }

        [Description("Determines if the column width should automatically adjust to content")]
        [JsonPropertyName("autoFitEnabled")]
        public bool AutoFitEnabled { get; set; }

        [Description("Format string for displaying numeric values (e.g. '#,##0.00', 'P2')")]
        [JsonPropertyName("numberFormat")]
        public string NumberFormat { get; set; }

        [Description("Indicates if text should wrap to multiple lines when it exceeds column width")]
        [JsonPropertyName("wordWrap")]
        public bool WordWrap { get; set; }

        [Description("Text alignment within the column ('Left', 'Center', 'Right')")]
        [JsonPropertyName("horizontalAlignment")]
        public string HorizontalAlignment { get; set; }

        [Description("Specifies whether the field can be dragged to different areas in the pivot")]
        [JsonPropertyName("allowDrag")]
        public bool AllowDrag { get; set; } = true;
    }
}
