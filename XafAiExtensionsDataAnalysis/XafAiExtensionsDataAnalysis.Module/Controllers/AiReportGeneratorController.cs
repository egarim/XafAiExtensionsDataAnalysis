using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.PivotGrid.OLAP.AdoWrappers;
using DevExpress.Xpo;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using System.Text.Json;
using XafAiExtensionsDataAnalysis.Module.AiTools;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;


namespace XafAiExtensionsDataAnalysis.Module.Controllers {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class AiReportGeneratorController : ObjectViewController<DetailView,AiGeneratedReport> {
        SimpleAction PreviewReport;
        SimpleAction GenerateReport;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public AiReportGeneratorController()
        {
            InitializeComponent();
            GenerateReport = new SimpleAction(this, "Generate Report", "View");
            GenerateReport.Execute += GenerateReport_Execute;

            PreviewReport = new SimpleAction(this, "MyAction", "View");
            PreviewReport.Execute += PreviewReport_Execute;
            

        }
        private void PreviewReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ReportServiceController controller = Frame.GetController<ReportServiceController>();
            if (controller != null)
            {
                var reportStorage = Application.ServiceProvider.GetRequiredService<IReportStorage>();
                using IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportDataV2));
                IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Contacts Report");
                string handle = reportStorage.GetReportContainerHandle(reportData);

                
                controller.ShowPreview(handle);
            }
            ;
        }
       
            static IChatClient CurrentClient;
        static string OpenAiModelId = "gpt-4o";



        private async void GenerateReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                var CurrentAiGeneratedReport = this.View.CurrentObject as AiGeneratedReport;
                var reportGeneratorAI=this.ObjectSpace.FindObject<ReportGeneratorAI>(null);
                var businessSchema = this.ObjectSpace.FindObject<BusinessSchema>(null);
                CurrentClient = GetChatClientOpenAiImp(Environment.GetEnvironmentVariable("OpenAiTestKey"), OpenAiModelId);
                List<ChatMessage> Messages = new List<ChatMessage>();


                List<ChatMessage> chatMessages = new List<ChatMessage>();
            

                chatMessages.Add(new ChatMessage(ChatRole.System, reportGeneratorAI.SystemPrompt));
                chatMessages.Add(new ChatMessage(ChatRole.User, $"The report MUST be generated using this Schema {businessSchema.Schema}, the datasource must be one of the entities on this schema, use the TypeFullName property to set the value of the datasource"));
                chatMessages.Add(new ChatMessage(ChatRole.User, CurrentAiGeneratedReport.Prompt));



                var AiAnswer = await CurrentClient.CompleteAsync<ReportRequest>(chatMessages);
                RuntimeReportBuilder runtimeReportBuilder = new RuntimeReportBuilder();
                var Report=runtimeReportBuilder.CreateReport(AiAnswer.Result);
                MemoryStream stream = new();
                Report.SaveLayoutToXml(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var XpOs=this.ObjectSpace as XPObjectSpace;
                Type type = XpOs.TypesInfo.PersistentTypes.FirstOrDefault(t => t.FullName == AiAnswer.Result.DataSource).Type;
                ReportDataV2 reportData=new ReportDataV2(XpOs.Session, type);
                reportData.Content = stream.ToArray();
                reportData.DisplayName = AiAnswer.Result.ReportTitle;
                CurrentAiGeneratedReport.ReportTitle = AiAnswer.Result.ReportTitle;
                CurrentAiGeneratedReport.Report = reportData;
            
                CurrentAiGeneratedReport.Log ="Report Successfully Generated"+System.Environment.NewLine+JsonSerializer.Serialize(AiAnswer.Usage);
                this.ObjectSpace.CommitChanges();
            }
            catch (Exception)
            {

                throw;
            }
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
        private IChatClient GetChatClientOpenAiImp(string ApiKey, string ModelId)
        {
            OpenAIClient openAIClient = new OpenAIClient(ApiKey);

            return new OpenAIChatClient(openAIClient, ModelId)
                .AsBuilder()
                .UseFunctionInvocation()
                .Build();
        }
    }
}
