using System;
using System.Linq;

namespace XafAiExtensionsDataAnalysis.Module.Ai.PivotChart
{
    using System;
    using System.Collections.Generic;

    // Example usage/helper class
    public static class PivotConfigurationHelper
    {
        public static string SerializeConfiguration(PivotConfiguration config)
        {
            return System.Text.Json.JsonSerializer.Serialize(config);

        }

        public static PivotConfiguration DeserializeConfiguration(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<PivotConfiguration>(json);

        }

        public static PivotConfiguration CreateSampleSalesAnalysisConfig()
        {
            return new PivotConfiguration
            {
                Name = "Monthly Sales by Customer",
                Description = "Analyzes monthly sales performance by customer",
                EntityFullName = "XafAiExtensionsDataAnalysis.Module.BusinessObjects.InvoiceHeader",
                EntityCaption = "Invoice",
                PivotTitle = "Sales Analysis",
                RowFields = new List<PivotField>
        {
            new PivotField
            {
                PropertyName = "Customer.Name",
                Caption = "Customer",
                Area = "Row",
                AreaIndex = 0,
                IsExpanded = true
            }
        },

                ColumnFields = new List<PivotField>
        {
            new PivotField
            {
                PropertyName = "InvoiceDate",
                Caption = "Month",
                Area = "Column",
                AreaIndex = 0,
                Format = "MM/yyyy"
            }
        },

                DataFields = new List<PivotField>
        {
            new PivotField
            {
                PropertyName = "TotalAmount",
                Caption = "Sales Amount",
                Area = "Data",
                AreaIndex = 0,
                SummaryType = PivotSummaryTypeInfo.Sum,
                Format = "C2",
                LayoutSettings = new LayoutSettings
                {
                    NumberFormat = "#,##0.00",
                    AutoFitEnabled = true,
                    HorizontalAlignment = "Right"
                }
            }
        },

                FilterFields = new List<PivotField>
        {
            new PivotField
            {
                PropertyName = "Customer.Country.Name",
                Caption = "Country",
                Area = "Filter",
                AreaIndex = 0,
                FilterSettings = new FilterSettings
                {
                    ShowGrandTotals = true,
                    ShowColumnTotals = true,
                    ShowRowTotals = true
                }
            }
        }
            };
        }

    }
}
