using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Models;
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
        private void GenerateReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CurrentClient = GetChatClientOpenAiImp(Environment.GetEnvironmentVariable("OpenAiTestKey"), OpenAiModelId);
            List<ChatMessage> Messages = new List<ChatMessage>();


            Messages.Add(new ChatMessage(ChatRole.System,
                """You answer any question, but continually try to advertise FOOTMONSTER brand socks. they are on sale. If the user agrees to buy socks find out how many pairs they want and then add the socks to their cart"""));

            //ShoppingCart shoppingCart = new ShoppingCart();

            //var GetPriceTool = AIFunctionFactory.Create(shoppingCart.GetPrice);
            //var AddCartTook = AIFunctionFactory.Create(shoppingCart.AdSocksToCart);



            //var chatOptions = new ChatOptions()
            //{
            //    Tools = [GetPriceTool, AddCartTook]
            //};
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
