# JobsPages4Hangfire.Dashboard

Hangfire Dashboard 任务管理页面扩展库，用于把实现了 `IJob` 并标记了 `ManagementPageAttribute` 的任务自动展示到 Hangfire Dashboard 中，支持立即执行、定时执行、延迟执行和 Cron 循环执行。

## 本地演示

仓库包含一个最小 ASP.NET Core Demo 项目，用于展示扩展页面效果。

运行：

```bash
dotnet run --project samples/JobsPages4Hangfire.Dashboard.Demo/JobsPages4Hangfire.Dashboard.Demo.csproj
```

## Localization

`UseManagementPages` supports an optional language setting for built-in UI text:

```csharp
configuration
    .UseMemoryStorage()
    .UseManagementPages(typeof(DemoManagementJobs).Assembly, "zh-CN");
```

You can also use the options object:

```csharp
configuration.UseManagementPages(
    new[] { typeof(DemoManagementJobs).Assembly },
    new ManagementPageOptions { Language = "en-US" });
```

打开：

```text
http://localhost:5000/hangfire
```

进入 Hangfire Dashboard 后，点击导航中的“任务管理系统”，可以看到 Demo Jobs 示例任务页面。也可以直接打开：

```text
http://localhost:5000/hangfire/management
```

Demo 使用 Hangfire in-memory storage，应用重启后任务数据会丢失。Demo 的 Dashboard 配置仅用于本地演示，生产环境必须配置 Hangfire Dashboard 授权策略。
