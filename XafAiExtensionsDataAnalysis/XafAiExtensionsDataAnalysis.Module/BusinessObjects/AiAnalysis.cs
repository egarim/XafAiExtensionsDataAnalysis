using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafAiExtensionsDataAnalysis.Module.BusinessObjects
{

    [NavigationItem("Reports")]
    [DefaultClassOptions]
    public class AiAnalysis : Analysis
    {
        public AiAnalysis(Session session) : base(session)
        { }


        SystemPrompt reportGeneratorAI;
        string prompt;

        [Size(SizeAttribute.Unlimited)]
        public string Prompt
        {
            get => prompt;
            set => SetPropertyValue(nameof(Prompt), ref prompt, value);
        }
        
        public SystemPrompt ReportGeneratorAI
        {
            get => reportGeneratorAI;
            set => SetPropertyValue(nameof(ReportGeneratorAI), ref reportGeneratorAI, value);
        }
    }
}
