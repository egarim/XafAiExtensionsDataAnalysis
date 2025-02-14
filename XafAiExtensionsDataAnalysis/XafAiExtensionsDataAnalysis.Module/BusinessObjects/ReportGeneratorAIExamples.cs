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
using System.Drawing;

namespace XafAiExtensionsDataAnalysis.Module.BusinessObjects {
    
    public class ReportGeneratorAIExample : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ReportGeneratorAIExample(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }

        string name;
        ReportGeneratorAI reportGeneratorAi;
        Image reportExample;
        string prompt;
        string json;
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Json
        {
            get => json;
            set => SetPropertyValue(nameof(Json), ref json, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Prompt
        {
            get => prompt;
            set => SetPropertyValue(nameof(Prompt), ref prompt, value);
        }
        [ValueConverter(typeof(DevExpress.Xpo.Metadata.ImageValueConverter))]
        public Image ReportExample
        {
            get => reportExample;
            set => SetPropertyValue(nameof(ReportExample), ref reportExample, value);
        }

        [Association("ReportGeneratorAI-ReportGeneratorExamples")]
        public ReportGeneratorAI ReportGeneratorAi
        {
            get => reportGeneratorAi;
            set => SetPropertyValue(nameof(ReportGeneratorAi), ref reportGeneratorAi, value);
        }
    }
}