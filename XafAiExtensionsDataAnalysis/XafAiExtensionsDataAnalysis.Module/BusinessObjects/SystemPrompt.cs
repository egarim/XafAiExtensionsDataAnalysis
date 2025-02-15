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

    public enum AiModule
    {
        None=0,
        Reports=1,
        Analysis=2,

    }

    [DefaultClassOptions]
    [NavigationItem("A.I")]
    //[ImageName("BO_Contact")]
    [DefaultProperty(nameof(BusinessObjects.SystemPrompt.Name))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class SystemPrompt : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public SystemPrompt(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }

        AiModule module;
        string text;
        string name;
        string systemPrompt;



        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Text
        {
            get => text;
            set => SetPropertyValue(nameof(Text), ref text, value);
        }
        
        public AiModule Module
        {
            get => module;
            set => SetPropertyValue(nameof(Module), ref module, value);
        }
        [Association("SystemPrompt-PromptExamples")]
        public XPCollection<PromptExample> PromptExamples
        {
            get
            {
                return GetCollection<PromptExample>(nameof(PromptExamples));
            }
        }
    }
}