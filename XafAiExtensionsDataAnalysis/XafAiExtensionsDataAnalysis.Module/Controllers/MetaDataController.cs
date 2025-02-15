using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;
using XafAiExtensionsDataAnalysis.Module.Tools;

namespace XafAiExtensionsDataAnalysis.Module.Controllers {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class MetaDataController : ObjectViewController<DetailView,BusinessSchema> {
        SimpleAction GenerateMetadata;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public MetaDataController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

            GenerateMetadata = new SimpleAction(this, "Generate Metadata", "View");
            GenerateMetadata.Execute += GenerateMetadata_Execute;
            
        }
        private void GenerateMetadata_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var assembly = typeof(Customer).Assembly;
            var ormStructure = OrmAnalyzer.AnalyzeOrm(assembly, typeof(BaseObject));

           
            //serialize ormStructure to json formatted string
            var json = System.Text.Json.JsonSerializer.Serialize(ormStructure, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });


            var CurrentSchema = this.ObjectSpace.GetObjectsQuery<BusinessSchema>().FirstOrDefault();
            CurrentSchema.Schema = json;
            this.View.ObjectSpace.CommitChanges();
        }
        protected override void OnActivated() {
            base.OnActivated(); 
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated(); 
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
