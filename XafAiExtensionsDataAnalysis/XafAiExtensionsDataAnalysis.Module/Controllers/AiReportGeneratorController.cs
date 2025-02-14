using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Microsoft.Extensions.AI;
using OpenAI;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;


namespace XafAiExtensionsDataAnalysis.Module.Controllers {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class AiReportGeneratorController : ObjectViewController<DetailView,AiGeneratedReport> {
        SimpleAction GenerateReport;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public AiReportGeneratorController()
        {
            InitializeComponent();
            GenerateReport = new SimpleAction(this, "Generate Report", "View");
            GenerateReport.Execute += GenerateReport_Execute;
            
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
            

                chatMessages.Add(new ChatMessage(ChatRole.User, "Which countries have we mention on this conversation"));

                foreach (ChatMessage chatMessage in chatMessages)
                {
                    Console.WriteLine($"{chatMessage.Role}:{chatMessage.ToString()}");
                }



                var Result = await CurrentClient.CompleteAsync(chatMessages);
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
