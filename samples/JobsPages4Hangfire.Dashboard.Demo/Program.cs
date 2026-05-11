using Hangfire;
using Hangfire.MemoryStorage;
using JobsPages4Hangfire.Dashboard;
using JobsPages4Hangfire.Dashboard.Demo.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(configuration =>
{
    configuration
        .UseMemoryStorage()
    .UseManagementPages(typeof(DemoManagementJobs).Assembly, "zh-CN");
    //.UseManagementPages(typeof(DemoManagementJobs).Assembly, "en");
});

builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "default", "demo" };
});

var app = builder.Build();

app.MapGet("/", () => Results.Content(
    """
    <!doctype html>
    <html lang="zh-CN">
    <head>
        <meta charset="utf-8" />
        <title>JobsPages4Hangfire.Dashboard Demo</title>
    </head>
    <body>
        <h1>JobsPages4Hangfire.Dashboard Demo</h1>
        <p>打开 Hangfire Dashboard 后，在导航中进入“任务管理系统”查看扩展页面。</p>
        <ul>
            <li><a href="/hangfire">Hangfire Dashboard</a></li>
            <li><a href="/hangfire/management">Management Pages</a></li>
        </ul>
    </body>
    </html>
    """,
    "text/html; charset=utf-8"));

app.UseHangfireDashboard("/hangfire");

app.Run();
