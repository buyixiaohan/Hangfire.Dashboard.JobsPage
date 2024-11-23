﻿using Hangfire.Dashboard.JobsPage.resx;

namespace Hangfire.Dashboard.JobsPage.Pages
{
    internal partial class CronJobsPage : RazorPage
    {
        public CronJobsPage()
        {

        }

        public override void Execute()
        {

            WriteLiteral($@"
<style>
	.tab-content input[type=number] {{
		height: 25px;
		width: 52px;
	}}
.d-none{{
display:none;
				}}
</style>
<div>
	<div>
		<!-- Nav tabs -->
		<ul class=""nav nav-tabs"" role=""tablist"">

			<li role=""presentation"" class="""" style=""display:none"">
				<a href=""#second"" aria-controls=""second"" role=""tab"" data-toggle=""tab"">{Resource.Common_Second}</a>
			</li>

			<li role=""presentation"" class=""active"">
				<a href=""#minute"" aria-controls=""minute"" role=""tab"" data-toggle=""tab"">{Resource.Common_Minute}</a>
			</li>
			<li role=""presentation"">
				<a href=""#hour"" aria-controls=""hour"" role=""tab"" data-toggle=""tab"">{Resource.Common_Hour}</a>
			</li>
			<li role=""presentation"">
				<a href=""#day"" aria-controls=""day"" role=""tab"" data-toggle=""tab"">{Resource.Common_Day}</a>
			</li>
			<li role=""presentation"">
				<a href=""#month"" aria-controls=""month"" role=""tab"" data-toggle=""tab"">{Resource.Common_Month}</a>
			</li>
			<li role=""presentation"">
				<a href=""#week"" aria-controls=""week"" role=""tab"" data-toggle=""tab"">{Resource.Common_Week}</a>
			</li>
<!--
			<li role=""presentation"">
				<a href=""#year"" aria-controls=""year"" role=""tab"" data-toggle=""tab"">{Resource.Common_Year}</a>
			</li>
-->
			<li role=""presentation"">
				<a href=""#general"" aria-controls=""general"" role=""tab"" data-toggle=""tab"">{Resource.CronJobsPage_TabTitle_CommonCronExpressions}</a>
			</li>
			<li role=""presentation"">
				<a href=""#cron"" aria-controls=""cron"" role=""tab"" data-toggle=""tab"">{Resource.CronJobsPage_TabTitle_CronExpressionAnalysis}</a>
			</li>
		</ul>
		<!-- Tab panes -->
		<div class=""tab-content"">
			<!--second-->
			<div role=""tabpanel"" class=""tab-pane "" id=""second""  style=""display:none"">
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""secondType"" value=""All"" checked=""checked"">
							Wildcards allowed per second[, - * /]
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""secondType"" value=""Cyclic"">
							Cycle from
							<input type=""number"" maxlength=""2"" id=""secondTypeCyclic_1"" value=""1"">
							-
							<input type=""number"" id=""secondTypeCyclic_2"" value=""2"">
							second
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""secondType"" value=""Interval"">
							From
							<input type=""number"" id=""secondTypeInterval_1"" value=""0"">
							Start in _ seconds
							<input type=""number"" id=""secondTypeInterval_2"" value=""1"">
							Execute once every second
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""secondType"" value=""Assigned"">
							Specified
						</label>
					</div>
					<div style=""margin-left: 20px;"">
						<select id=""secondTypeAssigned_1"" data-placeholder=""Select the specified seconds...""
								style=""width:350px;"" multiple></select>
					</div>
				</div>
			<!--minute-->
			<div role=""tabpanel"" class=""tab-pane active"" id=""minute"">
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""minuteType"" value=""All"" checked=""checked"">
						{Resource.CronJobsPage_Tabpanel_Minute_EveryMinute}
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""minuteType"" value=""Cyclic"">
						{Resource.CronJobsPage_Tabpanel_Minute_TimeRangeIsFrom}
						<input type=""number"" id=""minuteTypeCyclic_1"" value=""1"">
						-
						<input type=""number"" id=""minuteTypeCyclic_2"" value=""2"">
						{Resource.Minute}
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""minuteType"" value=""Interval"">
						{Resource.CronJobsPage_Tabpanel_Minute_StartToBeginAt}
						<input type=""number"" id=""minuteTypeInterval_1"" value=""0"">
						{Resource.CronJobsPage_Tabpanel_Minute_AndRunsOnceEvery}
						<input type=""number"" id=""minuteTypeInterval_2"" value=""1"">
						{Resource.CronJobsPage_Tabpanel_Minute_Minutes}
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""minuteType"" value=""Assigned"">
						{Resource.CronJobsPage_Tabpanel_Minute_Assigned}
					</label>
				</div>
				<div style=""margin-left: 20px;"">
					<select id=""minuteTypeAssigned_1"" data-placeholder=""Please select the minute type you want to assigned...""
							style=""width:350px;"" multiple></select>
				</div>
			</div>
			<!--hour-->
			<div role=""tabpanel"" class=""tab-pane"" id=""hour"">
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""hourType"" value=""All"" checked=""checked"">
						Every hour (Permitted wildcard character[, - * /])
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""hourType"" value=""Cyclic"">
						Time range is from
						<input type=""number"" id=""hourTypeCyclic_1"" value=""0"">
						-
						<input type=""number"" id=""hourTypeCyclic_2"" value=""2"">
						hour
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""hourType"" value=""Interval"">
						Start to begin at
						<input type=""number"" id=""hourTypeInterval_1"" value=""0"">
						and runs once every
						<input type=""number"" id=""hourTypeInterval_2"" value=""1"">
						hours
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""hourType"" value=""Assigned"">
						Assigned
					</label>
				</div>
				<div style=""margin-left: 20px;"">
					<select id=""hourTypeAssigned_1"" data-placeholder=""Please select the hour type you want to assigned""
							style=""width:350px;"" multiple></select>
				</div>
			</div>

			<!--日-->
			<div role=""tabpanel"" class=""tab-pane"" id=""day"">
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""dayType"" value=""All"" checked=""checked"">
						Daily (Permitted wildcard character[, - * /])
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""dayType"" value=""Cyclic"">
						Time range is from
						<input type=""number"" id=""dayTypeCyclic_1"" value=""1"">
						-
						<input type=""number"" id=""dayTypeCyclic_2"" value=""2"">
						day
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""dayType"" value=""Interval"">
						Start to begin at
						<input type=""number"" id=""dayTypeInterval_1"" value=""1"">
						day and runs once every
						<input type=""number"" id=""dayTypeInterval_2"" value=""1"">
						days
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""dayType"" value=""Assigned"">
						Assigned
					</label>
				</div>
				<div style=""margin-left: 20px;"">
					<select id=""dayTypeAssigned_1"" data-placeholder=""Please select the day type you want to assigned""
							style=""width:350px;"" multiple></select>
				</div>
				<!--<div class=""radio"">
						<label>
							<input type=""radio"" name=""dayType"" value=""NotAssigned"">
							Not specify
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""dayType"" value=""RecentDays"">
							per month
							<input type=""number"" id=""dayTypeRecentDays_1"" value=""1"">
							The nearest working day
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""dayType"" value=""LastDayOfMonth"">
							Last day of the month
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""dayType"" value=""LastDayOfMonthRecentDays"">
							Last business day of the month
						</label>
					</div>-->
			</div>

			<!--月-->
			<div role=""tabpanel"" class=""tab-pane"" id=""month"">
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""monthType"" value=""All"" checked=""checked"">
						Monthly (Permitted wildcard character[, - * /])
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""monthType"" value=""Cyclic"">
						Time range is from
						<input type=""number"" id=""monthTypeCyclic_1"" value=""1"">
						-
						<input type=""number"" id=""monthTypeCyclic_2"" value=""2"">
						month
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""monthType"" value=""Interval"">
						Start to begin at
						<input type=""number"" id=""monthTypeInterval_1"" value=""1"">
						month and runs once every
						<input type=""number"" id=""monthTypeInterval_2"" value=""1"">
						months
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""monthType"" value=""Assigned"">
						Assigned
					</label>
				</div>
				<div style=""margin-left: 20px;"">
					<select id=""monthTypeAssigned_1"" data-placeholder=""Please select the month type you want to assigned""
							style=""width:350px;"" multiple></select>
				</div>
				<!--<div class=""radio"">
						<label>
							<input type=""radio"" name=""monthType"" value=""NotAssigned"">
							Not specify
						</label>
					</div>-->
			</div>

			<!--周-->
			<div role=""tabpanel"" class=""tab-pane"" id=""week"">
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""weekType"" value=""All"" checked=""checked"">
						Weekly (Permitted wildcard character[, - * /])
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""weekType"" value=""Cyclic"">
						Time range is from
						<input type=""number"" id=""weekTypeCyclic_1"" value=""1"">
						-
						<input type=""number"" id=""weekTypeCyclic_2"" value=""2"">
						week
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""weekType"" value=""WeeksOfWeek"">
						Start to run at the
						<input type=""number"" id=""weekTypeWeeksOfWeek_1"" value=""1"">
						 and the day of the week is
						<input type=""number"" id=""weekTypeWeeksOfWeek_2"" value=""1"">
					</label>
				</div>
				<div class=""radio"">
					<label>
						<input type=""radio"" name=""weekType"" value=""Assigned"">
						Assigned
					</label>
				</div>
				<div style=""margin-left: 20px;"">
					<select id=""weekTypeAssigned_1"" data-placeholder=""Please select the week type you want to assigned""
							style=""width:350px;"" multiple></select>
				</div>
				<!--<div class=""radio"">
						<label>
							<input type=""radio"" name=""weekType"" value=""NotAssigned"">
							Not specify
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""weekType"" value=""LastWeekOfMonth"">
							Last week of the month
							<input type=""number"" id=""weekTypeLastWeekOfMonth_1"" value=""1"">
						</label>
					</div>-->
			</div>
			<!--年-->
			<!--<div role=""tabpanel"" class=""tab-pane"" id=""year"">
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""yearType"" value=""All"" checked=""checked"">
							Wildcard allowed per year [, - * /]
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""yearType"" value=""NotAssigned"">
							Not specify
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""yearType"" value=""Cyclic"">
							Cycle from
							<input type=""number"" id=""yearTypeCyclic_1"" value=""2015"">
							-
							<input type=""number"" id=""yearTypeCyclic_2"" value=""2299"">
							year
						</label>
					</div>
					<div class=""radio"">
						<label>
							<input type=""radio"" name=""yearType"" value=""Assigned"">
							Specified
						</label>
					</div>
					<div style=""margin-left: 20px;"">
						<select id=""yearTypeAssigned_1"" data-placeholder=""Select specified year ...""
								style=""width:350px;"" multiple></select>
					</div>
				</div>-->
			<!--常用Cron表达式-->
		<div role=""tabpanel"" class=""tab-pane"" id=""general"">
			Every minute： (* * * * *)<br />
			Every hour： (0 * * * *)<br />
			Daily： (0 0 * * *)<br />
			Weekly： (0 0 * * 1)<br />
			Monthly： (0 0 1 * *)<br />
			Annually： (0 0 1 1 *)<br />
			<br /><br />
			<b>Commonly used Example</b><br />
			0 10 * * * ?--------------------Run once every hour after 10 minutes<br />
			0 0/32 8,12 * * ? --------------Run once at 8:32 am and 12:32 pm every day<br />
			0 0/2 * * * ?-------------------Run once every 2 minutes<br />
			0 0 12 * * ?--------------------Run once at 12:00 pm every day<br />
			0 15 10 ? * *-------------------Run once at 10:15 am every day<br />
			0 15 10 * * ?-------------------Run once at 10:15 am every day<br />
			0 15 10 * * ? *-----------------Run once at 10:15 am every day<br />
			0 15 10 * * ? 2005--------------Run once at 10:15 am every day on 2005<br />
			0 * 14 * * ?--------------------Run once every minute between 2:00 pm and 2:59 pm every day<br />
			0 0/5 14 * * ?------------------Run once every 5 minutes between 2:00 pm and 2:59 pm every day<br />
			0 0/5 14,18 * * ?---------------Run once every 5 minutes between 2:00 pm and 2:59 pm and between 6:00 pm and 6:59 pm every day<br />
			0 0-5 14 * * ?------------------Run once every minute between 2:00 pm and 2:05 pm every day<br />
			0 10,44 14 ? 3 WED--------------Run once at 2:10 pm and 2:44 pm every Wednesday on March every year<br />
			0 15 10 ? * MON-FRI-------------Run once at 10:15 am from Monday to Friday<br />
			0 15 10 15 * ?------------------Run once at 10:15 am on the 15th day of each month<br />
			0 15 10 L * ?-------------------Run once at 10:15 am on the last day of each month<br />
			0 15 10 ? * 6L------------------Run once at 10:15 am on the last Friday of each month<br />
			0 15 10 ? * 6L 2002-2005--------Run once at 10:15 am on the last Friday of each month between 2002 to 2005<br />
			0 15 10 ? * 6#3-----------------Run once at 10:15 am on the third Friday of each month<br />
			0 0 12 1/5 * ?------------------Run once at 12:00 pm every 5 days since the first day of each month<br />
			0 11 11 11 11 ?-----------------Run once at 11:11 am on every November 11th<br />




			<br /><br />

			Run once at 11:11 am on every November 11th：0 0 0 * * ?<br />
			At midnight every Monday：0 0 0 ? * MON or 0 0 0 ? * 2 (Note:1=SUN,2=MON,3=TUE,4=WED,5=THU,6=FRI,7=SAT)<br />
			At midnight on the first day of each month： 0 0 0 1 * ?<br />
			At midnight on January 1st every year：0 0 0 1 1 ?or 0 0 0 1 JAN ?(Note:The months from 1 to 12 represents JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC)<br />
			At midnight on August 1st, 2046：0 0 0 1 8 ? 2046<br />

			Run once every 5 minutes：*/5 * * * * ?<br />

			Run once every minute：0 */1 * * * ?<br />

			Run once at 11:00 pm every day：0 0 23 * * ?<br />

			Run once at 1:00 am every day：0 0 1 * * ?<br />

			Run once at 1:00 am on the first day of each month：0 0 1 1 * ?<br />

			Run once at 11:00 pm on the last day of each month：0 0 23 L * ?<br />

			Run once at 1:00 am on every Sunday of each week：0 0 1 ? * L<br />

			Run once at 26, 29, 33 minutes every hour：0 26,29,33 * * * ?<br />

			Run once at 12:00 am(midnight), 1:00 pm, 6:00 pm and 9:00 pm every day：0 0 0,13,18,21 * * ?<br />

			Run once every 5 minutes：0 0/5 * * * ?<br />
			""0 0 12 * * ?"" Run once at 12:00 pm every day<br />
			""0 15 10 ? * *"" Run once at 10:15 am every day<br />
			""0 15 10 * * ?"" Run once at 10:15 am every day<br />
			""0 15 10 * * ? *"" Run once at 10:15 am every day<br />
			""0 15 10 * * ? 2005"" Run once at 10:15 am every day on 2005<br />
			""0 * 14 * * ?"" Run once every minute between 2:00 pm and 2:59 pm every day<br />
			""0 0/5 14 * * ?"" Run once every 5 minutes between 2:00 pm and 2:59 pm every day<br />
			""0 0/5 14,18 * * ?"" Run once every 5 minutes between 2:00 pm and 2:59 pm and between 6:00 pm and 6:59 pm every day<br />
			""0 0-5 14 * * ?"" Run once every minute between 2:00 pm and 2:05 pm every day<br />
			""0 10,44 14 ? 3 WED"" Run once at 2:10 pm and 2:44 pm every Wednesday on March every year<br />
			""0 15 10 ? * MON-FRI"" Run once at 10:15 am from Monday to Friday<br />
			""0 15 10 15 * ?"" Run once at 10:15 am on the 15th day of each month<br />
			""0 15 10 L * ?"" Run once at 10:15 am on the last day of each month<br />
			""0 15 10 ? * 6L"" Run once at 10:15 am on the last Friday of each month<br />
			""0 15 10 ? * 6L 2002-2005"" Run once at 10:15 am on the last Friday of each month between 2002 to 2005<br />
			""0 15 10 ? * 6#3"" Run once at 10:15 am on the third Friday of each month<br />
		</div>

		<!--Cron Expression Analysis-->
		<div role=""tabpanel"" class=""tab-pane"" id=""cron"">
			Verified address {Url.To("/cron?cron=*+*+*+*+*")}<br /><br />
			Precise to the minute in the format of 5 characters
<pre>
					* * * * *
					- - - - -
					| | | | |
					| | | | +----- day of week (0 - 6) (Sunday=0)
					| | | +------- month (1 - 12)
					| | +--------- day of month (1 - 31)
					| +----------- hour (0 - 23)
					+------------- min (0 - 59)
</pre>
			Precise to the minute and second in the format of 6 characters
<pre>
				* * * * * *
				- - - - - -
				| | | | | |
				| | | | | +--- day of week (0 - 6) (Sunday=0)
				| | | | +----- month (1 - 12)
				| | | +------- day of month (1 - 31)
				| | +--------- hour (0 - 23)
				| +----------- min (0 - 59)
				+------------- sec (0 - 59)
</pre>
			Definition of special wildcard characters<br />
																 <pre>
				* It means any value. For example, if you put ""*"" at the month place, it means it will happen every month. If it is placed at the day of week spot, the operation would happen every day of week;
				- It means a specific range;
				, It means adding a possible value for the same place;
				/ The value placed before the character means the starting time and the value placed after that means the incremental value for every time;
</pre>

			<b>The Cron expression includes 7 characters displayed as below</b><br />
			<ul class=""list-unstyled"">
				<li>Format: [Second] [Minute] [Hour] [Day of month] [Month] [Week] [Year]</li>
				<li>
					<table class=""table table-hover"">
						<tr>
							<th>Num</th>
							<th>Description</th>
							<th>Required</th>
							<th>Allowed values</th>
							<th>v</th>
						</tr>
						<tr>
							<td>1</td>
							<td>second</td>
							<td>Yes</td>
							<td>0-59</td>
							<td>, - * /</td>
						</tr>
						<tr>
							<td>2</td>
							<td>Minute</td>
							<td>Yes</td>
							<td>0-59</td>
							<td>, - * /</td>
						</tr>
						<tr>
							<td>3</td>
							<td>hour</td>
							<td>Yes</td>
							<td>0-23</td>
							<td>, - * /</td>
						</tr>
						<tr>
							<td>4</td>
							<td>day</td>
							<td>Yes</td>
							<td>0-31</td>
							<td>, - * ? / L W</td>
						</tr>
						<tr>
							<td>5</td>
							<td>month</td>
							<td>Yes</td>
							<td>1-12 or JAN-DEC</td>
							<td>, - * /</td>
						</tr>
						<tr>
							<td>6</td>
							<td>week</td>
							<td>Yes</td>
							<td>1-7 or SUN-SAT</td>
							<td>, - * ? / L #</td>
						</tr>
						<tr>
							<td>7</td>
							<td>year</td>
							<td>No</td>
							<td>empty or 1970-2099</td>
							<td>, - * /</td>
						</tr>
					</table>
				</li>
			</ul>
			<b>The description of Wildcard character</b><br />
			<ol>
				<li>The forward slash(/) means the incremental value. For example, ""5/15"" in the place of second show that it will run once every 15 seconds since the fifth second every minute.</li>
				<li>Asterisk (*) is one of the wildcard characters, which means it will accept any possible value(e.g. it means it will run every minute when the asterisk is set at the place of minute.).</li>
				<li>The question mark(?) means it does not include any specific value, so it can be set ""?"" at the place of the day of month if it does not matter what date it is. Letter L is the abbreviation of last. It means the operation is scheduled at the last day of month if this letter is put at the place of month. In the place of week, it is equal to 7 if letter L exits alone. Otherwise, it represents the last day of the last week of the month. For instance, ""0L"" shows that the operation will be triggered on Sunday of the last week for that month.</li>
				<li>""-"" means that users can specify the time range. For example, if the place of hour is set ""10-12"", it means that the operation will run at 10:00, 11:00, 12:00 am.</li>
				<li>Comma(,) means it can be set to different values at the same spot of cron expression format. For example, ""MON,WED,FRI"" at the place of week means the operation is set to run on Monday, Wednesday and Friday.</li>
				<li>The pound sign means that it is assigned a specific business day of the given month. ""MON#2"" in the place of week means the task will start to begin on the second Monday of a month.</li>
				<li>L means the last one. if L is put alone at the place of day of month, it means the last day of month(The number of day would be different depending on which month it is. If it is on February, it will also check whether it is leap year or not). The letter L is equal to ""7"" or ""SAT"". If there is a number value before ""L"", it means it is the last instance for that number value. For example, ""6L"" at the place of day of week means it is set to the last Friday of that month.</li>
				<li>W means the closest business day(Monday to Friday) to the assigned day. For instance, ""15W"" at the place of day of month means the task will be triggered to run at the closest business day to the 15th of each month. If the 15th day is Saturday, the operation will run on recent Friday(14th day of month). If the 15th day is Sunday, the task will be triggered on next Monday(16th day of month). The task will run on the assigned day if it is weekday(Monday to Friday). For instance, ""1W"" means it will perform the job on the closest weekday to the first day of each month.</li>
			</ol>
			<p class=""text-danger"">Note: 'L' and 'W' can be used together. ""LW"" placed at the day of month means the job will be ran at the last business day for that month.</p><br />

		</div>
		</div>
	</div>
	<hr>
	<div class=""panel panel-info"">
		<div class=""panel-heading"">
			<h3 class=""panel-title"">{Resource.CronJobsPage_Tabpanel_Result}</h3>
		</div>
		<div class=""panel-body"">
			<form class=""form-inline"">
				<div class=""form-group"">
					<div class=""input-group input-group-sm"">
						<input type=""text"" class=""form-control"" style=""width: 450px;"" id=""result"" placeholder=""Result"">
						<div class=""input-group-btn "">
							<button type=""button"" id=""analysis"" class=""btn btn-default""><span class=""glyphicon glyphicon-ok""></span>  &nbsp;{Resource.CronJobsPage_Tabpanel_Analysis}</button>
						</div>
					</div>
				</div>
			</form>
		</div>
	</div>
</div>");
        }
    }

}
