using System;
using System.Linq;

namespace XafAiExtensionsDataAnalysis.Module.Ai.PivotChart
{
    public class LayoutSettings
    {
        public string Width { get; set; }
        public bool AutoFitEnabled { get; set; }
        public string NumberFormat { get; set; }
        public bool WordWrap { get; set; }
        public string HorizontalAlignment { get; set; }
        public bool AllowDrag { get; set; } = true;
    }
}
