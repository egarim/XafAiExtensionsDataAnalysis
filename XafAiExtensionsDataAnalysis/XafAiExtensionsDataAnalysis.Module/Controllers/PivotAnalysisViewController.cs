using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XafAiExtensionsDataAnalysis.Module.Ai.Reports;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;

namespace XafAiExtensionsDataAnalysis.Module.Controllers
{
    public class PivotAnalysisViewController : ObjectViewController<DetailView, AiAnalysis> 
    {
        SimpleAction GenerateAnalysisFromPrompt;
        public PivotAnalysisViewController() : base()
        {
            // Target required Views (use the TargetXXX properties) and create their Actions.
            GenerateAnalysisFromPrompt = new SimpleAction(this, "Generate Analysis From Prompt", "View");
            GenerateAnalysisFromPrompt.Execute += GenerateAnalysisFromPrompt_Execute;
            
        }
         IChatClient CurrentClient;
         string OpenAiModelId = "gpt-4o";
        private async void GenerateAnalysisFromPrompt_Execute(object sender, SimpleActionExecuteEventArgs e)
        {


            try
            {
                var CurrentAiAnalysis = this.View.CurrentObject as AiAnalysis;
                var businessSchema = this.ObjectSpace.FindObject<BusinessSchema>(null);
                CurrentClient = GetChatClientOpenAiImp(Environment.GetEnvironmentVariable("OpenAiTestKey"), OpenAiModelId);
                List<ChatMessage> Messages = new List<ChatMessage>();


                List<ChatMessage> chatMessages = new List<ChatMessage>();


                //chatMessages.Add(new ChatMessage(ChatRole.System, reportGeneratorAI.SystemPrompt));
                //chatMessages.Add(new ChatMessage(ChatRole.System, $"The pivot MUST be generated using this Schema {businessSchema.Schema}, the datasource must be one of the entities on this schema, use the TypeFullName property to set the value of the datasource"));
                //chatMessages.Add(new ChatMessage(ChatRole.User, CurrentAiGeneratedReport.Prompt));



                //var AiAnswer = await CurrentClient.CompleteAsync<ReportRequest>(chatMessages);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private IChatClient GetChatClientOpenAiImp(string ApiKey, string ModelId)
        {
            OpenAIClient openAIClient = new OpenAIClient(ApiKey);

            return new OpenAIChatClient(openAIClient, ModelId)
                .AsBuilder()
                .UseFunctionInvocation()
                .Build();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
    }
}
