using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace XafAiExtensionsDataAnalysis.Module.BusinessObjects {
    [DefaultClassOptions]
    [NavigationItem("A.I")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class AiGeneratedReport : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public AiGeneratedReport(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }

        string reportDefJson;
        ReportDataV2 report;
        string reportTitle;
        string log;
        string prompt;

        [Size(SizeAttribute.Unlimited)]
        public string Prompt
        {
            get => prompt;
            set => SetPropertyValue(nameof(Prompt), ref prompt, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Log
        {
            get => log;
            set => SetPropertyValue(nameof(Log), ref log, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ReportTitle
        {
            get => reportTitle;
            set => SetPropertyValue(nameof(ReportTitle), ref reportTitle, value);
        }
        
        [Size(SizeAttribute.Unlimited)]
        public string ReportDefJson
        {
            get => reportDefJson;
            set => SetPropertyValue(nameof(ReportDefJson), ref reportDefJson, value);
        }
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public ReportDataV2 Report
        {
            get => report;
            set => SetPropertyValue(nameof(Report), ref report, value);
        }
    }
}