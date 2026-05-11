using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using Hangfire.Server;
using Hangfire.States;
using JobsPages4Hangfire.Dashboard.Metadata;
using JobsPages4Hangfire.Dashboard.resx;
using JobsPages4Hangfire.Dashboard.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace JobsPages4Hangfire.Dashboard.Pages
{
    public class ManagementBasePage : RazorPage
    {
        private static readonly HashSet<string> RegisteredCommandRoutes = new HashSet<string>();
        private static readonly object RegisteredCommandRoutesSyncRoot = new object();
        private readonly string menuName;


        protected internal ManagementBasePage(string menuName)
        {
            this.menuName = menuName;
        }

        public static void AddCommands(string menuCode)
        {
            var jobs = JobsHelper.JobMetadatas.Where(j => j.MenuCode == menuCode);

            foreach (var jobMetadata in jobs)
            {

                var route = $"{ManagementPage.UrlRoute}/{jobMetadata.JobId.ScrubURL()}";
                lock (RegisteredCommandRoutesSyncRoot)
                {
                    if (!RegisteredCommandRoutes.Add(route))
                    {
                        continue;
                    }
                }

                DashboardRoutes.Routes.Add(route, new CommandWithResponseDispatcher(context =>
                {
                    string errorMessage = null;
                    string jobLink = null;
                    var par = new List<object>();
                    string GetFormVariable(string key)
                    {
                        return Task.Run(() => context.Request.GetFormValuesAsync(key)).Result.FirstOrDefault();
                    }
                    var id = GetFormVariable("id");
                    var type = GetFormVariable("type");

                    foreach (var parameterInfo in jobMetadata.MethodInfo.GetParameters())
                    {
                        if (parameterInfo.ParameterType == typeof(PerformContext) || parameterInfo.ParameterType == typeof(IJobCancellationToken))
                        {
                            par.Add(null);
                            continue;
                        }

                        DisplayDataAttribute displayInfo = null;
                        if (parameterInfo.GetCustomAttributes(true).OfType<DisplayDataAttribute>().Any())
                        {
                            displayInfo = parameterInfo.GetCustomAttribute<DisplayDataAttribute>();
                        }
                        else
                        {
                            displayInfo = new DisplayDataAttribute();
                        }

                        var variable = $"{id}_{parameterInfo.Name}";
                        if (parameterInfo.ParameterType == typeof(DateTime))
                        {
                            variable = $"{variable}_datetimepicker";
                        }

                        variable = variable.Trim('_');
                        var formInput = GetFormVariable(variable);

                        object item = null;
                        if (parameterInfo.ParameterType == typeof(string))
                        {
                            item = formInput;
                            if (displayInfo.IsRequired && string.IsNullOrWhiteSpace((string)item))
                            {
                                errorMessage = string.Format(Resource.VerifyInfo_Required, parameterInfo.Name);

                                break;
                            }
                        }
                        else if (parameterInfo.ParameterType == typeof(int))
                        {
                            int intNumber;
                            if (int.TryParse(formInput, out intNumber) == false)
                            {
                                errorMessage = string.Format(Resource.VerifyInfo_FormatNotCorrect, parameterInfo.Name);
                                break;
                            }
                            item = intNumber;
                        }
                        else if (parameterInfo.ParameterType == typeof(DateTime))
                        {
                            item = formInput == null ? DateTime.MinValue : DateTime.Parse(formInput);
                            if (displayInfo.IsRequired && item.Equals(DateTime.MinValue))
                            {
                                errorMessage = string.Format(Resource.VerifyInfo_Required, parameterInfo.Name);
                                break;
                            }
                        }
                        else if (parameterInfo.ParameterType == typeof(bool))
                        {
                            item = formInput == "on";
                        }
                        else if (!parameterInfo.ParameterType.IsValueType)
                        {
                            if (formInput == null || formInput.Length == 0)
                            {
                                item = null;
                                if (displayInfo.IsRequired)
                                {
                                    errorMessage = string.Format(Resource.VerifyInfo_Required, parameterInfo.Name);

                                    break;
                                }
                            }
                            else
                            {
                                item = JsonConvert.DeserializeObject(formInput, parameterInfo.ParameterType);
                            }
                        }
                        else
                        {
                            item = formInput;
                        }

                        par.Add(item);
                    }

                    if (errorMessage == null)
                    {
                        var job = new Job(jobMetadata.Type, jobMetadata.MethodInfo, par.ToArray());
                        var client = new BackgroundJobClient(context.Storage);
                        switch (type)
                        {
                            case "CronExpression":
                                {
                                    var manager = new RecurringJobManager(context.Storage);
                                    var schedule = GetFormVariable($"{id}_schedule");
                                    var cron = GetFormVariable($"{id}_sys_cron");
                                    var name = GetFormVariable($"{id}_sys_name");

                                    if (string.IsNullOrWhiteSpace(schedule ?? cron))
                                    {
                                        errorMessage = Resource.Error_NoCronExpressionDefined;
                                        break;
                                    }
                                    if (jobMetadata.AllowMultiple && string.IsNullOrWhiteSpace(name))
                                    {
                                        errorMessage = Resource.Error_NoJobNameDefined;
                                        break;
                                    }

                                    try
                                    {
                                        var jobId = jobMetadata.AllowMultiple ? name : jobMetadata.JobId;
                                        manager.AddOrUpdate(jobId, job, schedule ?? cron, TimeZoneInfo.Local, jobMetadata.Queue);
                                        jobLink = new UrlHelper(context).To("/recurring");
                                    }
                                    catch (Exception e)
                                    {
                                        errorMessage = e.Message;
                                    }
                                    break;
                                }
                            case "ScheduleDateTime":
                                {
                                    var datetime = GetFormVariable($"{id}_sys_datetime");

                                    if (string.IsNullOrWhiteSpace(datetime))
                                    {
                                        errorMessage = Resource.Error_NoScheduleDefined;
                                        break;
                                    }

                                    if (!TryParseManagementDateTime(datetime, out DateTime dt))
                                    {
                                        errorMessage = Resource.Error_UnableToParseSchedule;
                                        break;
                                    }
                                    try
                                    {
                                        var jobId = client.Create(job, new ScheduledState(dt.ToLocalTime()));//Queue
                                        jobLink = new UrlHelper(context).JobDetails(jobId);
                                    }
                                    catch (Exception e)
                                    {
                                        errorMessage = e.Message;
                                    }
                                    break;
                                }
                            case "ScheduleTimeSpan":
                                {
                                    var schedule = GetFormVariable("schedule");
                                    var timeSpan = GetFormVariable($"{id}_sys_timespan");

                                    if (string.IsNullOrWhiteSpace(schedule ?? timeSpan))
                                    {
                                        errorMessage = Resource.Error_NoDelayDefined;
                                        break;
                                    }

                                    if (!TimeSpan.TryParse(schedule ?? timeSpan, out TimeSpan dt))
                                    {
                                        errorMessage = Resource.Error_UnableToParseDelay;
                                        break;
                                    }

                                    try
                                    {
                                        var jobId = client.Create(job, new ScheduledState(dt));//Queue
                                        jobLink = new UrlHelper(context).JobDetails(jobId);
                                    }
                                    catch (Exception e)
                                    {
                                        errorMessage = e.Message;
                                    }
                                    break;
                                }
                            case "Enqueue":
                            default:
                                {
                                    try
                                    {
                                        var jobId = client.Create(job, new EnqueuedState(jobMetadata.Queue));
                                        jobLink = new UrlHelper(context).JobDetails(jobId);
                                    }
                                    catch (Exception e)
                                    {
                                        errorMessage = e.Message;
                                    }
                                    break;
                                }
                        }
                    }

                    context.Response.ContentType = "application/json";

                    if (!string.IsNullOrEmpty(jobLink))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.WriteAsync(JsonConvert.SerializeObject(new { jobLink }));
                        return true;
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage }));
                    return false;
                }));
            }
        }

        private static bool TryParseManagementDateTime(string value, out DateTime dateTime)
        {
            return DateTime.TryParseExact(
                       value,
                       "yyyy-MM-dd HH:mm:ss",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.AssumeLocal,
                       out dateTime)
                   || DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dateTime);
        }

        public override void Execute()
        {
            Write(Html.RenderPartial(new ManagementJobsPage(BuildModel())));
        }

        private ManagementJobsPageModel BuildModel()
        {
            var jobs = JobsHelper.JobMetadatas.Where(j => j.MenuCode == menuName).OrderBy(x => x.SectionTitle).ThenBy(x => x.Name).ToList();
            var sectionTitles = jobs.Select(j => j.SectionTitle).Distinct().ToList();
            var menu = JobsHelper.ManagementPageAttrs.FirstOrDefault(s => s.MenuCode == menuName);
            var sections = sectionTitles.Select(section =>
            {
                var sectionJobs = jobs.Where(j => j.SectionTitle == section).ToList();
                var scrubbedSection = section.ScrubURL();

                return new ManagementSectionViewModel
                {
                    Title = section,
                    ScrubbedTitle = scrubbedSection,
                    Expanded = sectionTitles.First() == section,
                    ShowWrapper = sectionTitles.Count > 1,
                    Jobs = sectionJobs.Select(job => BuildJobModel(scrubbedSection, sectionJobs, job)).ToList()
                };
            }).ToList();

            return new ManagementJobsPageModel
            {
                MenuName = menuName,
                MenuDescription = menu?.Desc,
                Sections = sections
            };
        }

        private static ManagementJobViewModel BuildJobModel(string section, List<JobMetadata> sectionJobs, JobMetadata job)
        {
            var id = $"{section}_{job.MethodName.ScrubURL()}";
            var showMDAttr = job.MethodInfo.GetCustomAttributes(true).OfType<ShowMetaDataAttribute>().FirstOrDefault();
            var showMeta = showMDAttr != default && showMDAttr.ShowOnUI;

            return new ManagementJobViewModel
            {
                Id = id,
                Name = job.Name,
                Description = job.Description,
                Expanded = sectionJobs.First() == job,
                ShowMetadata = showMeta,
                MetadataJson = showMeta ? BuildMetadataJson(job) : null,
                AllowMultiple = job.AllowMultiple,
                CommandUrl = $"{ManagementPage.UrlRoute}/{job.JobId.ScrubURL()}",
                Inputs = BuildInputModels(id, job).ToList()
            };
        }

        private static string BuildMetadataJson(JobMetadata job)
        {
            var options = new JObject();
            var qAttr = job.MethodInfo.GetCustomAttributes(true).OfType<QueueAttribute>().FirstOrDefault();
            options.Add("Queue", (qAttr == default ? "default" : qAttr.Queue).ToUpper());

            var retryAttr = job.MethodInfo.GetCustomAttributes(true).OfType<AutomaticRetryAttribute>().FirstOrDefault();
            if (retryAttr != default)
            {
                var ar = new JObject
                {
                    { "Attempts", retryAttr.Attempts },
                    { "AllowMultiple", retryAttr.AllowMultiple },
                    { "DelaysInSeconds", (retryAttr.DelaysInSeconds != null ? JsonConvert.SerializeObject(retryAttr.DelaysInSeconds) : null) },
                    { "LogEvents", retryAttr.LogEvents },
                    { "OnAttemptsExceeded", (retryAttr.OnAttemptsExceeded == AttemptsExceededAction.Delete ? "Delete" : "Fail") }
                };
                options.Add("AutomaticRetryAttribute", ar);
            }

            return JsonConvert.SerializeObject(options, Formatting.Indented);
        }

        private static IEnumerable<ManagementInputViewModel> BuildInputModels(string id, JobMetadata job)
        {
            foreach (var parameterInfo in job.MethodInfo.GetParameters())
            {
                if (parameterInfo.ParameterType == typeof(PerformContext) || parameterInfo.ParameterType == typeof(IJobCancellationToken))
                {
                    continue;
                }

                var displayInfo = parameterInfo.GetCustomAttributes(true).OfType<DisplayDataAttribute>().FirstOrDefault() ?? new DisplayDataAttribute();
                var labelText = displayInfo?.Label ?? parameterInfo.Name;
                var placeholderText = displayInfo?.Placeholder ?? parameterInfo.Name;
                var inputId = $"{id}_{parameterInfo.Name}";
                var input = new ManagementInputViewModel
                {
                    Id = inputId,
                    Kind = "input",
                    Type = "text",
                    CssClasses = displayInfo.CssClasses,
                    Label = labelText,
                    Placeholder = placeholderText,
                    Description = displayInfo.Description,
                    DefaultValue = displayInfo.DefaultValue,
                    IsDisabled = displayInfo.IsDisabled,
                    IsRequired = displayInfo.IsRequired
                };

                if (parameterInfo.ParameterType == typeof(int))
                {
                    input.Type = "number";
                }
                else if (parameterInfo.ParameterType == typeof(Uri))
                {
                    input.Type = "url";
                }
                else if (parameterInfo.ParameterType == typeof(DateTime))
                {
                    input.Kind = "date";
                }
                else if (parameterInfo.ParameterType == typeof(bool))
                {
                    input.Kind = "checkbox";
                    input.IsRequired = false;
                }
                else if (parameterInfo.ParameterType.IsEnum)
                {
                    var data = new Dictionary<string, string>();
                    foreach (int v in Enum.GetValues(parameterInfo.ParameterType))
                    {
                        data.Add(Enum.GetName(parameterInfo.ParameterType, v), v.ToString());
                    }

                    var defaultValue = displayInfo.DefaultValue?.ToString();
                    input.Kind = "datalist";
                    input.Options = data;
                    input.InitialText = defaultValue ?? (!string.IsNullOrWhiteSpace(placeholderText) ? placeholderText : Resource.SelectValue);
                    input.InitialValue = defaultValue != null && data.ContainsKey(defaultValue) ? data[defaultValue] : "";
                    input.IsRequired = false;
                }

                yield return input;
            }
        }
    }
}
