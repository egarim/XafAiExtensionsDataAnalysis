﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XafAiExtensionsDataAnalysis.Module.Ai.PivotChart;
using XafAiExtensionsDataAnalysis.Module.Ai.Reports;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;

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

                ShowMessage("Generating Analysis", "Please Wait", InformationType.Info);    
                var CurrentAiAnalysis = this.View.CurrentObject as AiAnalysis;
                var businessSchema = this.ObjectSpace.FindObject<BusinessSchema>(null);
                CurrentClient = GetChatClientOpenAiImp(Environment.GetEnvironmentVariable("OpenAiTestKey"), OpenAiModelId);
                List<ChatMessage> Messages = new List<ChatMessage>();


                List<ChatMessage> chatMessages = new List<ChatMessage>();


                chatMessages.Add(new ChatMessage(ChatRole.System, CurrentAiAnalysis.SystemPrompt.Text));
                chatMessages.Add(new ChatMessage(ChatRole.System, $"The pivot MUST be generated using this Schema {businessSchema.Schema}, the datasource must be one of the entities on this schema, use the EntityFullName property to set the value of the OrmEntityTypeFullName"));
                chatMessages.Add(new ChatMessage(ChatRole.User, CurrentAiAnalysis.Prompt));



                var AiAnswer = await CurrentClient.CompleteAsync<PivotConfiguration>(chatMessages);
                Type type = this.Application.TypesInfo.PersistentTypes.FirstOrDefault(t => t.FullName == AiAnswer.Result.EntityFullName).Type;
                CurrentAiAnalysis.DataType = type;
                CurrentAiAnalysis.Name = AiAnswer.Result.Name;
                CurrentAiAnalysis.GeneratedOutput = AiAnswer.Result.ToString();
                CurrentAiAnalysis.GeneratedOutput = JsonSerializer.Serialize(AiAnswer.Result);
                CurrentAiAnalysis.Log = "Analysis Successfully Generated" + System.Environment.NewLine + JsonSerializer.Serialize(AiAnswer.Usage);
                this.ConfigureAnalysis(AiAnswer.Result, CurrentAiAnalysis);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ShowMessage(string Caption,string Message, InformationType informationType)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 2000;
            options.Message = Message;
            options.Type = informationType;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = Caption;
            options.Win.Type = WinMessageType.Toast;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        protected virtual void ConfigureAnalysis(PivotConfiguration config,AiAnalysis aiAnalysis)
        {
            throw new NotImplementedException();
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
