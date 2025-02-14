using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Drawing;

namespace XafAiExtensionsDataAnalysis.Module.AiTools
{


    public class ReportRequest
    {
        [JsonPropertyName("reportTitle")]
        [Description("The title of the report.")]
        public string ReportTitle { get; set; }

        [JsonPropertyName("dataSource")]
        [Description("The name of the type that will be used as the datasource of the report")]
        public string DataSource { get; set; }

        [JsonPropertyName("columns")]
        [Description("The columns to be included in the report.")]
        public IEnumerable<ReportColumnDTO> Columns { get; set; }

        [JsonPropertyName("grouping")]
        [Description("The grouping options for the report.")]
        public GroupingOptionsDTO Grouping { get; set; }

        [JsonPropertyName("calculatedFields")]
        [Description("The calculated fields to be included in the report.")]
        public IEnumerable<CalculatedFieldOptionsDTO> CalculatedFields { get; set; }

        [JsonPropertyName("summaryOptions")]
        [Description("The summary options for the report.")]
        public IEnumerable<SummaryOptionsDTO> SummaryOptions { get; set; }

        [JsonPropertyName("evenStyle")]
        [Description("The style for even rows.")]
        public RowStyleDTO EvenStyle { get; set; }

        [JsonPropertyName("oddStyle")]
        [Description("The style for odd rows.")]
        public RowStyleDTO OddStyle { get; set; }
    }

    public class ReportColumnDTO
    {
        [JsonPropertyName("fieldName")]
        [Description("The field name of the column.")]
        public string FieldName { get; set; }

        [JsonPropertyName("headerText")]
        [Description("The header text of the column.")]
        public string HeaderText { get; set; }

        [JsonPropertyName("width")]
        [Description("The width of the column.")]
        public int Width { get; set; }

        [JsonPropertyName("format")]
        [Description("The format string for the column.")]
        public string Format { get; set; }
    }

    public class CalculatedFieldOptionsDTO
    {
        [JsonPropertyName("name")]
        [Description("The name of the calculated field.")]
        public string Name { get; set; }

        [JsonPropertyName("expression")]
        [Description("The expression for the calculated field.")]
        public string Expression { get; set; }

        [JsonPropertyName("dataMember")]
        [Description("The data member for the calculated field.")]
        public string DataMember { get; set; }

        [JsonPropertyName("formatString")]
        [Description("The format string for the calculated field.")]
        public string FormatString { get; set; }

        [JsonPropertyName("summaryFunction")]
        [Description("The summary function for the calculated field.")]
        public string SummaryFunction { get; set; }
    }

    public class GroupingOptionsDTO
    {
        [JsonPropertyName("fieldName")]
        [Description("The field name for grouping.")]
        public string FieldName { get; set; }

        [JsonPropertyName("showGroupSummary")]
        [Description("Indicates whether to show group summary.")]
        public bool ShowGroupSummary { get; set; }

        [JsonPropertyName("subGroups")]
        [Description("The sub-groups for nested grouping.")]
        public List<GroupingOptionsDTO> SubGroups { get; set; }

        [JsonPropertyName("dateGroupInterval")]
        [Description("The date group interval (e.g., Month, Quarter, Year).")]
        public string DateGroupInterval { get; set; }

        [JsonPropertyName("dateFormat")]
        [Description("The date format for the group header.")]
        public string DateFormat { get; set; }
    }

    public class SummaryOptionsDTO
    {
        [JsonPropertyName("fieldName")]
        [Description("The field name for the summary.")]
        public string FieldName { get; set; }

        [JsonPropertyName("function")]
        [Description("The summary function (e.g., Sum, Avg, Count).")]
        public string Function { get; set; }

        [JsonPropertyName("formatString")]
        [Description("The format string for the summary.")]
        public string FormatString { get; set; }

        [JsonPropertyName("showInGroupFooter")]
        [Description("Indicates whether to show the summary in the group footer.")]
        public bool ShowInGroupFooter { get; set; }

        [JsonPropertyName("showInReportFooter")]
        [Description("Indicates whether to show the summary in the report footer.")]
        public bool ShowInReportFooter { get; set; }
    }

    public class RowStyleDTO
    {
        [JsonPropertyName("backColor")]
        [Description("The background color of the row.")]
        public string BackColor { get; set; }

        [JsonPropertyName("padding")]
        [Description("The padding of the row.")]
        public PaddingDTO Padding { get; set; }
    }

    public class PaddingDTO
    {
        [JsonPropertyName("left")]
        [Description("The left padding.")]
        public int Left { get; set; }

        [JsonPropertyName("right")]
        [Description("The right padding.")]
        public int Right { get; set; }

        [JsonPropertyName("top")]
        [Description("The top padding.")]
        public int Top { get; set; }

        [JsonPropertyName("bottom")]
        [Description("The bottom padding.")]
        public int Bottom { get; set; }
    }

}
