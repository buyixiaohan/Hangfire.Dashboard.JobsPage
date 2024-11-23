using Hangfire.Dashboard.Pages;

namespace Hangfire.Dashboard.JobsPage.Pages
{
	internal partial class ManagementPage : RazorPage
	{
		public const string Title = "任务管理";
		public const string UrlRoute = "/management";

		public override void Execute()
		{
			Layout = new LayoutPage(Title);

			WriteLiteral(
				$@"
    <link rel=""stylesheet"" type=""text/css"" href=""{Url.To($"{UrlRoute}/jsmcss")}"" />
    <div class=""row"">
        <div class=""col-md-3"">
"
			);
			Write(Html.RenderPartial(new CustomSidebarMenu(ManagementSidebarMenu.Items)));

			WriteLiteral(
				$@"
        </div>
        <div class=""col-md-9"">
            <h1 class=""page-header mgmt-title"">{Title}</h1>
            <div class=""visible-md-block visible-lg-block"">
               <p class=""d-none""> Select a page from the menu on the left to see the jobs available.</p>
				<p class=""d-block""> 在左侧选择需要管理的任务</p>
            </div>
            <div class=""hidden-md hidden-lg"">
               <p class=""d-none"">  Select a page from the tabs at the top to see the jobs available.</p>
					<p class=""d-block""> 再上方选择需要管理的任务</p>
            </div>
        </div>
    </div>
"
			);
		}
	}
}
