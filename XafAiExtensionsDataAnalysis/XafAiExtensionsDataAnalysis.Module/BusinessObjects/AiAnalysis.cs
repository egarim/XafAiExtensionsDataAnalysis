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


        string generatedOutput;
        string log;
        SystemPrompt reportGeneratorAI;
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
        
        [Size(SizeAttribute.Unlimited)]
        public string GeneratedOutput
        {
            get => generatedOutput;
            set => SetPropertyValue(nameof(GeneratedOutput), ref generatedOutput, value);
        }
        public SystemPrompt SystemPrompt
        {
            get => reportGeneratorAI;
            set => SetPropertyValue(nameof(SystemPrompt), ref reportGeneratorAI, value);
        }
    }
}
