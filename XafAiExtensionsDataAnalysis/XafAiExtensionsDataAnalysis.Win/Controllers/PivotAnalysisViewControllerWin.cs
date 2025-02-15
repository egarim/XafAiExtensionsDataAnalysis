using DevExpress.Data.PivotGrid;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.PivotChart;
using DevExpress.ExpressApp.PivotChart.Win;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XafAiExtensionsDataAnalysis.Module.Ai.PivotChart;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;
using XafAiExtensionsDataAnalysis.Module.Controllers;

namespace XafAiExtensionsDataAnalysis.Win.Controllers
{
    public class PivotAnalysisViewControllerWin : PivotAnalysisViewController
    {
    
        private PivotGridControl pivotGrid;


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

        }

        public PivotAnalysisViewControllerWin()
        {
           

        }
        private void GeneratePivot_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SetupPivotGrid();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

        }

        private void SetupPivotGrid()
        {
            // Create sample configuration (in real app, you would load this from somewhere)
            var config = PivotConfigurationHelper.CreateSampleSalesAnalysisConfig();
            var analysis = this.View.CurrentObject as Analysis;

            if (analysis.DataType == null)
            {
                analysis.DataType = typeof(InvoiceHeader);
            }
            AnalysisControlWin control = this.View.GetItems<IAnalysisEditorWin>()[0].Control;
            //IAnalysisControl control= new AnalysisControlWin();
            control.DataSource = new AnalysisDataSource(analysis, this.View.ObjectSpace.GetObjects(typeof(InvoiceHeader)));
            control.FieldBuilder.RebuildFields();



            // Configure the pivot grid using our configuration
            ConfigurePivotGrid(config, this.ObjectSpace, control);
            this.View.ObjectSpace.CommitChanges();
        }
        protected override void ConfigureAnalysis(PivotConfiguration config, AiAnalysis aiAnalysis)
        {
          
            AnalysisControlWin control = this.View.GetItems<IAnalysisEditorWin>()[0].Control;
            //IAnalysisControl control= new AnalysisControlWin();
            control.DataSource = new AnalysisDataSource(aiAnalysis, this.View.ObjectSpace.GetObjects(typeof(InvoiceHeader)));
            control.FieldBuilder.RebuildFields();
            control.
            try
            {

                // Configure Data Fields
                foreach (var fieldConfig in config.DataFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.DataArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }

                // Configure Row Fields
                foreach (var fieldConfig in config.RowFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.RowArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }

                // Configure Column Fields
                foreach (var fieldConfig in config.ColumnFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.ColumnArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }

                // Configure Filter Fields
                foreach (var fieldConfig in config.FilterFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.FilterArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Error configuring pivot grid: {ex.Message}");
            }

        }
        private void ConfigurePivotGrid(PivotConfiguration config, IObjectSpace objectSpace, AnalysisControlWin control)
        {
            try
            {

                // Configure Data Fields
                foreach (var fieldConfig in config.DataFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.DataArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }

                // Configure Row Fields
                foreach (var fieldConfig in config.RowFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.RowArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }

                // Configure Column Fields
                foreach (var fieldConfig in config.ColumnFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.ColumnArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }

                // Configure Filter Fields
                foreach (var fieldConfig in config.FilterFields)
                {
                    var field = CreatePivotField(fieldConfig, control.Fields);
                    field.Area = PivotArea.FilterArea;
                    field.AreaIndex = fieldConfig.AreaIndex;

                }


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Error configuring pivot grid: {ex.Message}");
            }
        }

        private PivotGridFieldBase CreatePivotField(PivotField fieldConfig, PivotGridFieldCollectionBase Fields)
        {
            PivotGridFieldBase field = Fields[fieldConfig.PropertyName];
            if (field == null)
            {
                field = new PivotGridField();
                field.FieldName = fieldConfig.PropertyName;
                Fields.Add(field);

            }
            field.Caption = fieldConfig.Caption;
            field.Name = $"field{fieldConfig.PropertyName.Replace(".", "_")}";
            //var field = new PivotGridField
            //{
            //    FieldName = fieldConfig.PropertyName,
            //    Caption = fieldConfig.Caption,
            //    Name = $"field{fieldConfig.PropertyName.Replace(".", "_")}"
            //};

            // Configure summary type
            field.SummaryType = ConvertToDevExpressSummaryType(fieldConfig.SummaryType);

            // Apply format if specified
            if (!string.IsNullOrEmpty(fieldConfig.Format))
            {
                field.ValueFormat.FormatString = fieldConfig.Format;
                field.ValueFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            }

            // Apply sort order if specified
            if (fieldConfig.SortOrder.HasValue)
            {
                field.SortOrder = (DevExpress.XtraPivotGrid.PivotSortOrder)fieldConfig.SortOrder.Value;
            }

            // Apply layout settings
            if (fieldConfig.LayoutSettings != null)
            {
                if (!string.IsNullOrEmpty(fieldConfig.LayoutSettings.Width))
                {
                    field.Width = int.Parse(fieldConfig.LayoutSettings.Width);
                }
                //TODO fix
                //field.CanDrag = fieldConfig.LayoutSettings.AllowDrag;
            }

            // Apply filter settings
            if (fieldConfig.FilterSettings != null)
            {
                field.Options.ShowGrandTotal = fieldConfig.FilterSettings.ShowGrandTotals;
                field.Options.ShowTotals = fieldConfig.FilterSettings.ShowRowTotals;
            }

            return field;
        }

        private PivotSummaryType ConvertToDevExpressSummaryType(PivotSummaryTypeInfo summaryType)
        {
            return summaryType switch
            {
                PivotSummaryTypeInfo.Sum => PivotSummaryType.Sum,
                PivotSummaryTypeInfo.Min => PivotSummaryType.Min,
                PivotSummaryTypeInfo.Max => PivotSummaryType.Max,
                PivotSummaryTypeInfo.Count => PivotSummaryType.Count,
                PivotSummaryTypeInfo.Average => PivotSummaryType.Average,
                _ => PivotSummaryType.Custom
            };
        }

        protected override void OnDeactivated()
        {
            if (pivotGrid != null)
            {
                pivotGrid.Dispose();
                pivotGrid = null;
            }
            base.OnDeactivated();
        }
    }
}
