using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Drawing;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using XafAiExtensionsDataAnalysis.Module.Ai.PivotChart;
using XafAiExtensionsDataAnalysis.Module.Ai.Reports;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;
using XafAiExtensionsDataAnalysis.Module.Tools;

namespace XafAiExtensionsDataAnalysis.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        //string name = "MyName";
        //DomainObject1 theObject = ObjectSpace.FirstOrDefault<DomainObject1>(u => u.Name == name);
        //if(theObject == null) {
        //    theObject = ObjectSpace.CreateObject<DomainObject1>();
        //    theObject.Name = name;
        //}

        // The code below creates users and roles for testing purposes only.
        // In production code, you can create users and assign roles to them automatically, as described in the following help topic:
        // https://docs.devexpress.com/eXpressAppFramework/119064/data-security-and-safety/security-system/authentication
#if !RELEASE
        // If a role doesn't exist in the database, create this role
        var defaultRole = CreateDefaultRole();
        var adminRole = CreateAdminRole();

        ObjectSpace.CommitChanges(); //This line persists created object(s).

        UserManager userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

        // If a user named 'User' doesn't exist in the database, create this user
        if(userManager.FindUserByName<ApplicationUser>(ObjectSpace, "User") == null) {
            // Set a password if the standard authentication type is used
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "User", EmptyPassword, (user) => {
                // Add the Users role to the user
                user.Roles.Add(defaultRole);
            });
        }

        // If a user named 'Admin' doesn't exist in the database, create this user
        if(userManager.FindUserByName<ApplicationUser>(ObjectSpace, "Admin") == null) {
            // Set a password if the standard authentication type is used
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "Admin", EmptyPassword, (user) => {
                // Add the Administrators role to the user
                user.Roles.Add(adminRole);
            });
        }

        ObjectSpace.CommitChanges(); //This line persists created object(s).
#endif

        if (ObjectSpace.GetObjectsCount(typeof(BusinessSchema), null) == 0)
        {
            BusinessSchema singleton = ObjectSpace.CreateObject<BusinessSchema>();
            var assembly = typeof(Customer).Assembly;
            var ormStructure = OrmAnalyzer.AnalyzeOrm(assembly, typeof(BaseObject));


            //serialize ormStructure to json formatted string
            var json = JsonSerializer.Serialize(ormStructure, new JsonSerializerOptions { WriteIndented = true });

            singleton.Schema = json;

        }

        // Get all report resources
        var allReports = ReportResourceHelper.GetReportResources(Assembly.GetExecutingAssembly()).ToList();
        // Get all prompt resources
        var allPrompts = PromptResourceHelper.GetPromptResources(Assembly.GetExecutingAssembly()).ToList();

        if (ObjectSpace.GetObjectsCount(typeof(SystemPrompt), null) == 0)
        {
            JsonSerializerOptions options = JsonSerializerOptions.Default;
            JsonNode OrmSchema = options.GetJsonSchemaAsNode(typeof(List<OrmEntityDto>));


            string AnalysisGeneratorAi = allPrompts.FirstOrDefault(p => p.FileName == "AnalysisGeneratorAISystemPrompt")?.TextContent;

            SystemPrompt AnalysisGeneratorAI = GetReportGeneratorAI(AnalysisGeneratorAi, "Default Analysis Generator AI",AiModule.Analysis);

            string ReportGeneratorAISystemPrompt = allPrompts.FirstOrDefault(p => p.FileName == "ReportGeneratorAISystemPrompt")?.TextContent;

            SystemPrompt reportGeneratorAI = GetReportGeneratorAI(ReportGeneratorAISystemPrompt, "Default Report Generator AI",AiModule.Reports);


            JsonNode PivotConfigurationSchema = options.GetJsonSchemaAsNode(typeof(PivotConfiguration));
            AnalysisGeneratorAI.Text = AnalysisGeneratorAI.Text.Replace("{{OutputSchema}}", PivotConfigurationSchema.ToString());
            AnalysisGeneratorAI.Text = AnalysisGeneratorAI.Text.Replace("{{OrmJsonSchema}}", OrmSchema.ToString());
            AnalysisGeneratorAI.Text = AnalysisGeneratorAI.Text.Replace("{{ExampleOutput}}", PivotConfigurationHelper.SerializeConfiguration(PivotConfigurationHelper.CreateSampleSalesAnalysisConfig()));


            JsonNode ReportRequirementSchema = options.GetJsonSchemaAsNode(typeof(ReportConfiguration));
            reportGeneratorAI.Text = reportGeneratorAI.Text.Replace("{{OutputSchema}}", ReportRequirementSchema.ToString());
            reportGeneratorAI.Text = reportGeneratorAI.Text.Replace("{{OrmJsonSchema}}", OrmSchema.ToString());
            reportGeneratorAI.Text = reportGeneratorAI.Text.Replace("{{ExampleOutput}}", RuntimeReportBuilder.CreateCustomerBehaviorReport());

            foreach (var item in allReports)
            {
                var example = ObjectSpace.CreateObject<PromptExample>();
                example.Name = item.FolderName;
                example.Prompt = item.PromptContent;
                example.Json = item.JsonContent;
                Image image = Image.FromStream(new MemoryStream(item.ImageContent));
                example.ReportExample = image;
                reportGeneratorAI.PromptExamples.Add(example);

            }

        }
     

        ObjectSpace.CommitChanges(); //This line persists created object(s).

        ObjectSpace.GenerateDataIfEmpty();
    }

    private SystemPrompt GetReportGeneratorAI(string textContent, string Name,AiModule aiModule)
    {
        SystemPrompt systemPrompt = ObjectSpace.CreateObject<SystemPrompt>();
        systemPrompt.Name = Name;
        systemPrompt.Text = textContent;
        systemPrompt.Module = aiModule;
        return systemPrompt;
    }

    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateAdminRole() {
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if(adminRole == null) {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
            adminRole.IsAdministrative = true;
        }
        return adminRole;
    }
    private PermissionPolicyRole CreateDefaultRole() {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if(defaultRole == null) {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddObjectPermission<ModelDifference>(SecurityOperations.ReadWriteAccess, "UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, "Owner.UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermission<AuditDataItemPersistent>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddObjectPermissionFromLambda<AuditDataItemPersistent>(SecurityOperations.Read, a => a.UserId == CurrentUserIdOperator.CurrentUserId().ToString(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermission<AuditedObjectWeakReference>(SecurityOperations.Read, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
