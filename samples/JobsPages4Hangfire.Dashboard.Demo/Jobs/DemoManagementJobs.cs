using System.ComponentModel;
using Hangfire;
using JobsPages4Hangfire.Dashboard.Metadata;
using JobsPages4Hangfire.Dashboard.Support;

namespace JobsPages4Hangfire.Dashboard.Demo.Jobs;

[ManagementPage("Demo Jobs", "Demo Jobs", "demo", "本页展示 JobsPages4Hangfire.Dashboard 扩展生成的任务管理界面。")]
[Description("示例任务集合，用于演示任务管理页面的输入控件和执行方式。")]
public class DemoManagementJobs : IJob
{
    [DisplayName("无参数维护任务")]
    [Description("展示没有业务参数时的任务页面效果。")]
    [JobDisplayName("Demo: Maintenance")]
    [Queue("demo")]
    public void Maintenance()
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] Demo maintenance job executed.");
    }

    [DisplayName("发送欢迎消息")]
    [Description("展示字符串、数字、布尔和日期时间输入控件。")]
    [JobDisplayName("Demo: Send welcome message")]
    [Queue("demo")]
    [ShowMetaData(true)]
    public void SendWelcomeMessage(
        [DisplayData(Label = "用户名称", Placeholder = "张三", Description = "要发送欢迎消息的用户。", DefaultValue = "张三", IsRequired = true)]
        string userName,
        [DisplayData(Label = "邮箱", Placeholder = "demo@example.com", Description = "演示用邮箱地址。", DefaultValue = "demo@example.com", IsRequired = true)]
        string email,
        [DisplayData(Label = "发送次数", Placeholder = "1", Description = "消息重复发送次数。", DefaultValue = 1, IsRequired = true)]
        int repeatCount,
        [DisplayData(Label = "仅演练", Description = "勾选后只输出日志，不代表真实发送。", DefaultValue = true)]
        bool dryRun,
        [DisplayData(Label = "计划日期", Description = "用于演示 DateTime 参数输入。")]
        DateTime planDate)
    {
        for (var index = 1; index <= repeatCount; index++)
        {
            Console.WriteLine($"[{DateTimeOffset.Now:O}] Welcome message #{index}: user={userName}, email={email}, dryRun={dryRun}, planDate={planDate:O}");
        }
    }

    [DisplayName("生成日报")]
    [Description("展示 AllowMultiple recurring job 的自定义 Job Name 输入框。")]
    [JobDisplayName("Demo: Generate daily report")]
    [Queue("demo")]
    [AllowMultiple]
    public void GenerateDailyReport(
        [DisplayData(Label = "报表日期", Description = "日报对应的业务日期。")]
        DateTime reportDate,
        [DisplayData(Label = "最大行数", Placeholder = "100", Description = "演示用最大输出行数。", DefaultValue = 100, IsRequired = true)]
        int maxRows)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] Daily report generated. reportDate={reportDate:O}, maxRows={maxRows}");
    }
}

public enum DemoPriority
{
    Low = 1,
    Normal = 2,
    High = 3
}

[ManagementPage("UI Test Jobs", "Forms and Inputs", "ui-tests", "Use these jobs to verify the left menu, grouped right-side panels, and input controls.")]
[Description("Additional jobs for validating the generated management UI.")]
public class UiTestManagementJobs : IJob
{
    [DisplayName("Text and number inputs")]
    [Description("Validates required text and numeric fields.")]
    [JobDisplayName("UI Test: Text and number inputs")]
    [Queue("demo")]
    [ShowMetaData(true)]
    public void TextAndNumberInputs(
        [DisplayData(Label = "Customer name", Placeholder = "Alice", Description = "Required text field.", DefaultValue = "Alice", IsRequired = true)]
        string customerName,
        [DisplayData(Label = "Retry count", Placeholder = "3", Description = "Required integer field.", DefaultValue = 3, IsRequired = true)]
        int retryCount)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] TextAndNumberInputs customerName={customerName}, retryCount={retryCount}");
    }

    [DisplayName("Date and checkbox inputs")]
    [Description("Validates DateTime picker and checkbox collection.")]
    [JobDisplayName("UI Test: Date and checkbox inputs")]
    [Queue("demo")]
    public void DateAndCheckboxInputs(
        [DisplayData(Label = "Run at", Description = "DateTime picker test.")]
        DateTime runAt,
        [DisplayData(Label = "Dry run", Description = "Checkbox test.", DefaultValue = true)]
        bool dryRun)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] DateAndCheckboxInputs runAt={runAt:O}, dryRun={dryRun}");
    }
}

[ManagementPage("UI Test Jobs", "Advanced Controls", "ui-tests", "Use these jobs to verify the left menu, grouped right-side panels, and input controls.")]
[Description("Advanced controls for validating dropdowns, metadata, and recurring command options.")]
public class AdvancedUiTestManagementJobs : IJob
{
    [DisplayName("Enum dropdown input")]
    [Description("Validates enum rendering as a dropdown option list.")]
    [JobDisplayName("UI Test: Enum dropdown input")]
    [Queue("demo")]
    public void EnumDropdownInput(
        [DisplayData(Label = "Priority", Placeholder = "Choose priority", Description = "Enum dropdown test.", DefaultValue = "Normal")]
        DemoPriority priority)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] EnumDropdownInput priority={priority}");
    }

    [DisplayName("Recurring report with name")]
    [Description("Validates AllowMultiple recurring job options and custom recurring job name.")]
    [JobDisplayName("UI Test: Recurring report with name")]
    [Queue("demo")]
    [AllowMultiple]
    [ShowMetaData(true)]
    public void RecurringReportWithName(
        [DisplayData(Label = "Report date", Description = "DateTime picker test.")]
        DateTime reportDate,
        [DisplayData(Label = "Max rows", Placeholder = "50", Description = "Required integer field.", DefaultValue = 50, IsRequired = true)]
        int maxRows)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] RecurringReportWithName reportDate={reportDate:O}, maxRows={maxRows}");
    }
}

[ManagementPage("Operations Jobs", "Operations Checks", "ops-tests", "Use this page to verify a second left-side menu item and single-section rendering.")]
[Description("Jobs for validating a separate management menu entry.")]
public class OperationsTestJobs : IJob
{
    [DisplayName("Health check")]
    [Description("No-argument job for validating a simple single-section page.")]
    [JobDisplayName("Ops Test: Health check")]
    [Queue("demo")]
    public void HealthCheck()
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] HealthCheck completed.");
    }

    [DisplayName("Cleanup by tenant")]
    [Description("Validates another form on a separate left menu page.")]
    [JobDisplayName("Ops Test: Cleanup by tenant")]
    [Queue("demo")]
    [ShowMetaData(true)]
    public void CleanupByTenant(
        [DisplayData(Label = "Tenant id", Placeholder = "tenant-a", Description = "Required tenant identifier.", DefaultValue = "tenant-a", IsRequired = true)]
        string tenantId,
        [DisplayData(Label = "Batch size", Placeholder = "100", Description = "Required integer field.", DefaultValue = 100, IsRequired = true)]
        int batchSize,
        [DisplayData(Label = "Preview only", Description = "Checkbox test.", DefaultValue = true)]
        bool previewOnly)
    {
        Console.WriteLine($"[{DateTimeOffset.Now:O}] CleanupByTenant tenantId={tenantId}, batchSize={batchSize}, previewOnly={previewOnly}");
    }
}
