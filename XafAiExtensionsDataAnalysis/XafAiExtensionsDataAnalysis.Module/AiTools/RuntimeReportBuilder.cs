using DevExpress.XtraReports.UI;
using System.Drawing;
using System.Collections;
using System.Linq.Expressions;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.ExpressApp;

namespace XafAiExtensionsDataAnalysis.Module.AiTools
{
  
    public class RuntimeReportBuilder
    {


        public RuntimeReportBuilder()
        {
            
        }
        public CollectionDataSource GetDataSourceFromType(string TypeFullName)
        {
            CollectionDataSource dataSource = new CollectionDataSource();
            dataSource.ObjectTypeName = TypeFullName;   

            return dataSource;
        }
        public XtraReport CreateReport(ReportRequest request)
        {
            // Call the original CreateReport method with mapped parameters
            IEnumerable<ReportColumn> columns = request.Columns?.Select(c => new ReportColumn
            {
                FieldName = c.FieldName,
                HeaderText = c.HeaderText,
                Width = c.Width,
                Format = c.Format
            });
            GroupingOptions grouping = request.Grouping == null ? null : new GroupingOptions
            {
                FieldName = request.Grouping.FieldName,
                ShowGroupSummary = request.Grouping.ShowGroupSummary,
                SubGroups = request.Grouping.SubGroups?.Select(g => new GroupingOptions
                {
                    FieldName = g.FieldName,
                    ShowGroupSummary = g.ShowGroupSummary,
                    DateGroupInterval = g.DateGroupInterval,
                    DateFormat = g.DateFormat
                }).ToList(),
                DateGroupInterval = request.Grouping.DateGroupInterval,
                DateFormat = request.Grouping.DateFormat
            };
            IEnumerable<CalculatedFieldOptions> calculatedFields = request.CalculatedFields?.Select(f => new CalculatedFieldOptions
            {
                Name = f.Name,
                Expression = f.Expression,
                DataMember = f.DataMember,
                FormatString = f.FormatString,
                SummaryFunction = f.SummaryFunction
            });
            IEnumerable<SummaryOptions> summaryOptions = request.SummaryOptions?.Select(s => new SummaryOptions
            {
                FieldName = s.FieldName,
                Function = s.Function,
                FormatString = s.FormatString,
                ShowInGroupFooter = s.ShowInGroupFooter,
                ShowInReportFooter = s.ShowInReportFooter
            });



            var DataSource=this.GetDataSourceFromType(request.DataSource);


            return CreateReport(
                reportTitle: request.ReportTitle,
                dataSource: DataSource,
                columns: columns,
                grouping: grouping,
                calculatedFields: calculatedFields,
                summaryOptions: summaryOptions);
        }
        public XtraReport CreateReport(
       string reportTitle,
       object dataSource,
       IEnumerable<ReportColumn> columns,
       GroupingOptions grouping = null,
       IEnumerable<CalculatedFieldOptions> calculatedFields = null,
       IEnumerable<SummaryOptions> summaryOptions = null)  // 

        {
            var report = new XtraReport
            {
                DataSource = dataSource
            };

            // Add calculated fields if provided
            if (calculatedFields != null)
            {
                foreach (var fieldOptions in calculatedFields)
                {
                    var calculatedField = new CalculatedField
                    {
                        Name = fieldOptions.Name,
                        Expression = fieldOptions.Expression,
                        DataSource = report.DataSource,
                        DataMember = fieldOptions.DataMember
                    };
                    report.CalculatedFields.Add(calculatedField);
                }
            }

            // Initialize bands with specific heights
            var detail = new DetailBand { HeightF = 25 };
            var pageHeader = new PageHeaderBand { HeightF = 25 };
            var reportHeader = new ReportHeaderBand { HeightF = 40 };

            // Create styles for alternating rows
            var evenStyle = new XRControlStyle
            {
                Name = "EvenStyle",
                BackColor = Color.FromArgb(242, 242, 242),
                Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0)
            };

            var oddStyle = new XRControlStyle
            {
                Name = "OddStyle",
                BackColor = Color.White,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0)
            };

            var totalStyle = new XRControlStyle
            {
                Name = "TotalStyle",
                BackColor = Color.FromArgb(230, 235, 247),
                Font = new Font("Arial", 9.75f, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 66, 110),
                Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0)
            };

            report.StyleSheet.AddRange(new XRControlStyle[] { evenStyle, oddStyle, totalStyle });

            // Apply alternating styles to detail band
            detail.EvenStyleName = "EvenStyle";
            detail.OddStyleName = "OddStyle";

            ConfigureReportHeader(reportHeader, reportTitle);

            float currentX = 0;
            foreach (var column in columns)
            {
                AddColumnToReport(report, detail, pageHeader, column, ref currentX);
            }

            if (grouping != null)
            {
                ConfigureGrouping(report, grouping);

                // Add group summary if enabled
                if (grouping.ShowGroupSummary)
                {
                    var groupFooter = new GroupFooterBand { HeightF = 25 };
                    groupFooter.StyleName = "TotalStyle";

                    // Add summary fields for numeric columns
                    float summaryX = 0;
                    foreach (var column in columns)
                    {
                        var label = new XRLabel
                        {
                            Width = column.Width,
                            LocationF = new PointF(summaryX, 0),
                            HeightF = 25,
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
                        };

                        // Check if column is numeric based on format
                        if (column.Format?.Contains("N") == true || column.Format?.Contains("C") == true)
                        {
                            label.ExpressionBindings.Add(
                                new ExpressionBinding("Text", $"sumSum([{column.FieldName}])")
                            );
                            label.TextFormatString = column.Format;
                        }
                        else if (summaryX == 0) // First column shows "Total" text
                        {
                            label.Text = "Total:";
                            label.Font = new Font("Arial", 9.75f, FontStyle.Bold);
                        }

                        groupFooter.Controls.Add(label);
                        summaryX += column.Width;
                    }

                    report.Bands.Add(groupFooter);
                }
            }

            report.Bands.AddRange(new Band[] { reportHeader, pageHeader, detail });

            // Add report totals at the bottom
            var reportFooter = new ReportFooterBand { HeightF = 30 };
            reportFooter.StyleName = "TotalStyle";

            float footerX = 0;
            foreach (var column in columns)
            {
                var label = new XRLabel
                {
                    Width = column.Width,
                    LocationF = new PointF(footerX, 0),
                    HeightF = 30,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
                };

                if (column.Format?.Contains("N") == true || column.Format?.Contains("C") == true)
                {
                    label.ExpressionBindings.Add(
                        new ExpressionBinding("Text", $"sumSum([{column.FieldName}])")
                    );
                    label.TextFormatString = column.Format;
                }
                else if (footerX == 0) // First column shows "Grand Total" text
                {
                    label.Text = "Grand Total:";
                    label.Font = new Font("Arial", 9.75f, FontStyle.Bold);
                }

                reportFooter.Controls.Add(label);
                footerX += column.Width;
            }

            report.Bands.Add(reportFooter);

            return report;
        }


        private void ConfigureReportHeader(ReportHeaderBand header, string title)
        {
            var titleLabel = new XRLabel
            {
                Text = title,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Width = 500,
                Height = 30,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                LocationF = new PointF(0, 5)  // Add small top padding
            };
            header.Controls.Add(titleLabel);
            header.HeightF = 40;  // Reduced from 50
        }

        private void AddColumnToReport(XtraReport report, DetailBand detail, PageHeaderBand pageHeader,
            ReportColumn column, ref float currentX)
        {
            // Header
            var headerLabel = new XRLabel
            {
                Text = column.HeaderText,
                Width = column.Width,
                LocationF = new PointF(currentX, 2),  // Small top padding
                HeightF = 20,  // Fixed height for header
                Font = new Font("Arial", 10, FontStyle.Bold),
                Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
            };
            pageHeader.Controls.Add(headerLabel);

            // Detail
            var detailLabel = new XRLabel
            {
                Width = column.Width,
                LocationF = new PointF(currentX, 0),
                HeightF = 25,  // Fixed height for detail rows
                ExpressionBindings = {
                    new ExpressionBinding("Text", column.FieldName)
                },
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
            };

            if (column.Format != null)
            {
                detailLabel.TextFormatString = column.Format;
            }

            detail.Controls.Add(detailLabel);
            currentX += column.Width;
        }

        private void ConfigureGrouping(XtraReport report, GroupingOptions grouping)
        {
            // Configure main group
            var groupHeader = CreateGroupHeader(grouping);
            report.Bands.Add(groupHeader);

            // Handle nested groups if they exist
            if (grouping.SubGroups != null && grouping.SubGroups.Any())
            {
                foreach (var subGroup in grouping.SubGroups)
                {
                    var subGroupHeader = CreateGroupHeader(subGroup);
                    // Set a smaller font for sub-groups to create visual hierarchy
                    var label = (XRLabel)subGroupHeader.Controls[0];
                    label.Font = new Font("Arial", 10, FontStyle.Bold);
                    report.Bands.Add(subGroupHeader);
                }
            }
        }

        private GroupHeaderBand CreateGroupHeader(GroupingOptions grouping)
        {
            var groupHeader = new GroupHeaderBand { HeightF = 25 };

            // Handle date-based grouping if specified
            if (!string.IsNullOrEmpty(grouping.DateGroupInterval))
            {
                string groupExpression = grouping.DateGroupInterval.ToLower() switch
                {
                    "month" => $"GetMonth([{grouping.FieldName}])",
                    "quarter" => $"GetQuarter([{grouping.FieldName}])",
                    "year" => $"GetYear([{grouping.FieldName}])",
                    _ => grouping.FieldName
                };
                groupHeader.GroupFields.Add(new GroupField(groupExpression));
            }
            else
            {
                groupHeader.GroupFields.Add(new GroupField(grouping.FieldName));
            }

            var groupLabel = new XRLabel
            {
                Width = 500,
                HeightF = 25,
                LocationF = new PointF(0, 0),
                ExpressionBindings = {
            new ExpressionBinding("Text", grouping.FieldName)
        },
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
            };

            // Apply date formatting if specified
            if (!string.IsNullOrEmpty(grouping.DateFormat))
            {
                groupLabel.TextFormatString = grouping.DateFormat;
            }

            groupHeader.Controls.Add(groupLabel);
            return groupHeader;
        }
    }

    public class SummaryOptions
    {
        public string FieldName { get; set; }
        public string Function { get; set; } // Sum, Avg, Count, etc.
        public string FormatString { get; set; }
        public bool ShowInGroupFooter { get; set; }
        public bool ShowInReportFooter { get; set; }
    }
    public class ReportColumn
    {
        public string FieldName { get; set; }
        public string HeaderText { get; set; }
        public int Width { get; set; }
        public string Format { get; set; }
    }
    public class CalculatedFieldOptions
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public string DataMember { get; set; }
        public string FormatString { get; set; }
        public string SummaryFunction { get; set; } 
    }
    public class GroupingOptions
    {
        public string FieldName { get; set; }
        public bool ShowGroupSummary { get; set; }
        public List<GroupingOptions> SubGroups { get; set; }
        public string DateGroupInterval { get; set; } // Month, Quarter, Year
        public string DateFormat { get; set; }
    }
    public class TopNFieldOptions
    {
        public string Name { get; set; }
        public string SourceField { get; set; }
        public string GroupByField { get; set; }
        public int TopCount { get; set; }
        public string AggregateFunction { get; set; } // Sum, Count, etc.
    }
   
}
