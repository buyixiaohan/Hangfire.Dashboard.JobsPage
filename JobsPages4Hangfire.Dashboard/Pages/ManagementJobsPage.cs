using System.Collections.Generic;
using System.Text;
using Hangfire.Dashboard;
using JobsPages4Hangfire.Dashboard.resx;

namespace JobsPages4Hangfire.Dashboard.Pages
{
    internal partial class ManagementJobsPage
    {
        public ManagementJobsPage(ManagementJobsPageModel model)
        {
            Model = model;
        }

        public ManagementJobsPageModel Model { get; }

        public NonEscapedString RenderJobMarkup(ManagementJobViewModel job, IDictionary<string, string> timeSpanItems, IDictionary<string, string> cronItems, string loadingText)
        {
            var html = new StringBuilder();
            html.Append($@"<div class=""panel panel-info js-management card"" data-id=""{job.Id}"" style=""{(job.Expanded ? "margin-top:20px" : "")}"">");
            html.Append($@"<div id=""heading_{job.Id}"" class=""panel-heading card-header {(job.Expanded ? "" : "collapsed")}"" role=""button"" data-toggle=""collapse"" data-target=""#collapse_{job.Id}"" href=""#collapse_{job.Id}"" aria-expanded=""{(job.Expanded ? "true" : "false")}"" aria-controls=""collapse_{job.Id}""><h4 class=""panel-title"">{job.Name}</h4></div>");
            html.Append($@"<div id=""collapse_{job.Id}"" class=""panel-collapse {(job.Expanded ? "collapse in" : "collapse")}"" aria-expanded=""{(job.Expanded ? "true" : "false")}"" aria-labelledby=""heading_{job.Id}""><div class=""panel-body"" style=""padding-bottom: 0px;""><p>{job.Description}</p>");
            if (job.ShowMetadata)
            {
                html.Append($@"<div class=""well"" style=""display: flex; padding: 3px; margin-bottom: 0px;""><div class=""col-xs-1"" role=""button"" data-toggle=""collapse"" href=""#options_collapse_{job.Id}"" aria-expanded=""false"" aria-controls=""options_collapse_{job.Id}""><span class=""glyphicon glyphicon-info-sign""></span></div><pre style=""margin-bottom: 0px; border: transparent;"" class=""col-xs-11 collapse"" aria-expanded=""false"" id=""options_collapse_{job.Id}"">{job.MetadataJson}</pre></div>");
            }

            html.Append(@"</div><div class=""panel-body"" style=""padding-bottom: 0px;""><div class=""well"">");
            if (job.Inputs.Count == 0)
            {
                html.Append($"<span>{Resource.JobDoseNotRequireInputs}</span>");
            }
            else
            {
                foreach (var input in job.Inputs)
                {
                    html.Append(RenderInputMarkup(input));
                }
            }

            html.Append($@"</div><div id=""{job.Id}_error""></div><div id=""{job.Id}_success""></div></div><div class=""panel-footer"">");
            html.Append(RenderButtonMarkup(job, timeSpanItems, cronItems, loadingText));
            html.Append("</div></div></div>");
            return new NonEscapedString(html.ToString());
        }

        private string RenderInputMarkup(ManagementInputViewModel input)
        {
            var html = new StringBuilder();
            if (input.Kind == "date")
            {
                html.Append($@"<div class=""form-group {input.CssClasses} {(input.IsRequired ? "required" : "")}""><label for=""{input.Id}"" class=""control-label"">{input.Label}</label><div class='input-group date' id='{input.Id}_datetimepicker'><input type='text' class=""form-control"" placeholder=""yyyy-MM-dd HH:mm:ss"" value=""{input.DefaultValue}"" {(input.IsDisabled ? "disabled='disabled'" : "")} {(input.IsRequired ? "required='required'" : "")} /><span class=""input-group-addon""><span class=""glyphicon glyphicon-calendar""></span></span></div>");
            }
            else if (input.Kind == "checkbox")
            {
                html.Append($@"<br/><div class=""form-group {input.CssClasses}""><div class=""form-check""><input class=""form-check-input"" type=""checkbox"" id=""{input.Id}"" {(((bool)(input.DefaultValue ?? false)) ? "checked='checked'" : "")} {(input.IsDisabled ? "disabled='disabled'" : "")} /><label class=""form-check-label"" for=""{input.Id}"">{input.Label}</label></div>");
            }
            else if (input.Kind == "datalist")
            {
                html.Append($@"<div class=""{input.CssClasses}""><label class=""control-label"">{input.Label}</label><div class=""dropdown""><button id=""{input.Id}"" class=""btn btn-default dropdown-toggle input-control-data-list"" type=""button"" data-selectedvalue=""{input.InitialValue}"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"" {(input.IsDisabled ? "disabled='disabled'" : "")}><span class=""{input.Id} input-data-list-text pull-left"">{input.InitialText}</span><span class=""caret""></span></button><ul class=""dropdown-menu data-list-options"" data-optionsid=""{input.Id}"" aria-labelledby=""{input.Id}"">");
                foreach (var item in input.Options)
                {
                    html.Append($@"<li><a href=""javascript:void(0)"" class=""option"" data-optiontext=""{item.Key}"" data-optionvalue=""{item.Value}"">{item.Key}</a></li>");
                }
                html.Append("</ul></div>");
            }
            else
            {
                html.Append($@"<div class=""form-group {input.CssClasses} {(input.IsRequired ? "required" : "")}""><label for=""{input.Id}"" class=""control-label"">{input.Label}</label><input class=""form-control"" type=""{input.Type}"" placeholder=""{input.Placeholder}"" id=""{input.Id}"" value=""{input.DefaultValue}"" {(input.IsDisabled ? "disabled='disabled'" : "")} {(input.IsRequired ? "required='required'" : "")} />");
            }

            if (!string.IsNullOrWhiteSpace(input.Description))
            {
                html.Append($@"<small id=""{input.Id}Help"" class=""form-text text-muted"">{input.Description}</small>");
            }
            html.Append("</div>");
            return html.ToString();
        }

        private string RenderButtonMarkup(ManagementJobViewModel job, IDictionary<string, string> timeSpanItems, IDictionary<string, string> cronItems, string loadingText)
        {
            var html = new StringBuilder();
            var commandUrl = Url.To(job.CommandUrl);
            html.Append($@"<div class=""btn-group management-command-type""><button class=""btn btn-default dropdown-toggle"" type=""button"" id=""dropdownMenu1"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">{Resource.TaskType}: <span class=""{job.Id} commandsType"">{Resource.JobManagementPage_TaskType_Immediate}</span><span class=""caret""></span></button><ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu1""><li><a href=""javascript:void(0)"" class=""commands-type"" data-commands-type=""Enqueue"" data-id=""{job.Id}"">{Resource.JobManagementPage_TaskType_Immediate}</a></li><li><a href=""javascript:void(0)"" class=""commands-type"" data-commands-type=""ScheduleDateTime"" data-id=""{job.Id}"">{Resource.JobManagementPage_TaskType_Scheduled}</a></li><li><a href=""javascript:void(0)"" class=""commands-type"" data-commands-type=""ScheduleTimeSpan"" data-id=""{job.Id}"">{Resource.JobManagementPage_TaskType_Delayed}</a></li><li><a href=""javascript:void(0)"" class=""commands-type"" data-commands-type=""CronExpression"" data-id=""{job.Id}"">{Resource.JobManagementPage_TaskType_Repeating}</a></li></ul></div>");
            html.Append($@"<div class=""commands-panel Enqueue management-command-action""><button class=""js-management-input-commands btn btn-sm btn-success"" data-url=""{commandUrl}"" data-loading-text=""{loadingText}"" input-id=""{job.Id}"" input-type=""Enqueue""><span class=""glyphicon glyphicon-play-circle""></span>&nbsp;{Resource.JobManagementPage_TaskType_ImmediateExecution}</button></div>");
            html.Append($@"<div class=""commands-options ScheduleDateTime management-command-input management-command-input-datetime"" style=""display:none;""><div class='input-group date management-schedule-datetime' id='{job.Id}_datetimepicker'><input type='text' class=""form-control"" placeholder=""yyyy-MM-dd HH:mm:ss"" id=""{job.Id}_sys_datetime"" /><span class=""input-group-addon""><span class=""glyphicon glyphicon-calendar""></span></span></div></div><div class=""commands-panel ScheduleDateTime management-command-action"" style=""display:none;""><button class=""btn btn-default btn-sm btn-primary js-management-input-commands"" type=""button"" input-id=""{job.Id}"" input-type=""ScheduleDateTime"" data-url=""{commandUrl}"" data-loading-text=""{loadingText}""><span class=""glyphicon glyphicon-calendar""></span>&nbsp;{Resource.JobManagementPage_TaskType_ScheduleExecution}</button></div>");
            html.Append($@"<div class=""commands-options ScheduleTimeSpan management-command-input management-command-input-delay"" style=""display:none;""><input type=""text"" class=""form-control time"" placeholder=""{Resource.JobManagementPage_TaskType_DelayedExecution_Placeholder}"" id=""{job.Id}_sys_timespan"" data-inputmask=""'mask':'99:99:99'"" value=""00:00:00""></div><div class=""commands-panel ScheduleTimeSpan management-command-action"" style=""display:none;""><div class=""btn-group""><button class=""btn btn-default btn-sm btn-info js-management-input-commands"" type=""button"" input-id=""{job.Id}"" input-type=""ScheduleTimeSpan"" data-url=""{commandUrl}"" data-loading-text=""{loadingText}""><span class=""glyphicon glyphicon-time""></span>&nbsp;{Resource.JobManagementPage_TaskType_DelayedExecution}</button><button type=""button"" class=""btn btn-info btn-sm dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false""><span class=""caret""></span></button><ul class=""dropdown-menu dropdown-menu-right"">");
            foreach (var o in timeSpanItems)
            {
                html.Append($@"<li><a href=""#"" class=""js-management-input-commands text-center"" input-id=""{job.Id}"" input-type=""ScheduleTimeSpan"" schedule=""{o.Value}"" data-url=""{commandUrl}"" data-loading-text=""{loadingText}"">{o.Key}</a></li>");
            }
            html.Append("</ul></div></div>");
            html.Append($@"<div class=""commands-options CronExpression management-command-input management-command-input-cron"" style=""display:none;""><div class='input-group' id='{job.Id}_cronbuilder'><input type=""text"" class=""form-control"" title=""{Resource.JobManagementPage_TaskType_RepeatingExecution_TitleTip}"" placeholder=""{Resource.JobManagementPage_TaskType_RepeatingExecution_Placeholder}"" id=""{job.Id}_sys_cron""><span class=""input-group-addon btn btn-default js-management-input-CronModal"" title=""{Resource.JobManagementPage_TaskType_RepeatingExecution_Builder}"" input-id=""{job.Id}""><span class=""glyphicon glyphicon-wrench""></span></span></div></div>");
            if (job.AllowMultiple)
            {
                html.Append($@"<div class=""commands-options CronExpression management-command-input management-command-input-name"" style=""display:none;""><div class=""input-group"" id=""{job.Id}_Name""><input type=""text"" class=""form-control"" title="""" placeholder=""{Resource.JobName}"" id=""{job.Id}_sys_name"" data-original-title=""{Resource.JobName_TitleTip}"" spellcheck=""false"" data-ms-editor=""true""></div></div>");
            }
            html.Append($@"<div class=""commands-panel CronExpression management-command-action"" style=""display:none;""><div class=""btn-group""><button class=""btn btn-default btn-sm btn-warning js-management-input-commands"" type=""button"" input-id=""{job.Id}"" input-type=""CronExpression"" data-confirm=""{Resource.Confirm_UpdateSchedule}"" data-url=""{commandUrl}"" data-loading-text=""{loadingText}""><span class=""glyphicon glyphicon-repeat""></span>&nbsp;{Resource.JobManagementPage_TaskType_RepeatingExecution}</button><button type=""button"" class=""btn btn-warning btn-sm dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false""><span class=""caret""></span></button><ul class=""dropdown-menu dropdown-menu-right"">");
            foreach (var o in cronItems)
            {
                html.Append($@"<li><a href=""#"" class=""js-management-input-commands text-right"" input-id=""{job.Id}"" input-type=""CronExpression"" schedule=""{o.Value}"" data-confirm=""{Resource.Confirm_UpdateSchedule}"" data-url=""{commandUrl}"" data-loading-text=""{loadingText}"">{o.Key}: <span>({o.Value})</span></a></li>");
            }
            html.Append("</ul></div></div>");
            return html.ToString();
        }
    }

    internal class ManagementJobsPageModel
    {
        public string MenuName { get; set; }

        public string MenuDescription { get; set; }

        public IReadOnlyList<ManagementSectionViewModel> Sections { get; set; }
    }

    internal class ManagementSectionViewModel
    {
        public string Title { get; set; }

        public string ScrubbedTitle { get; set; }

        public bool Expanded { get; set; }

        public bool ShowWrapper { get; set; }

        public IReadOnlyList<ManagementJobViewModel> Jobs { get; set; }
    }

    internal class ManagementJobViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Expanded { get; set; }

        public bool ShowMetadata { get; set; }

        public string MetadataJson { get; set; }

        public bool AllowMultiple { get; set; }

        public string CommandUrl { get; set; }

        public IReadOnlyList<ManagementInputViewModel> Inputs { get; set; }
    }

    internal class ManagementInputViewModel
    {
        public string Id { get; set; }

        public string Kind { get; set; }

        public string Type { get; set; }

        public string CssClasses { get; set; }

        public string Label { get; set; }

        public string Placeholder { get; set; }

        public string Description { get; set; }

        public object DefaultValue { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsRequired { get; set; }

        public IReadOnlyDictionary<string, string> Options { get; set; }

        public string InitialText { get; set; }

        public string InitialValue { get; set; }
    }
}
