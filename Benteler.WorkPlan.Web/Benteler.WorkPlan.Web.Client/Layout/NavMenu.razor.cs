using Microsoft.AspNetCore.Components;

namespace Benteler.WorkPlan.Web.Client.Layout	
{
	public partial class NavMenu
	{
		public string LastPage { get; set; }
		public NavMenu()
		{
			if (NavigationManager != null)
				LastPage = NavigationManager.Uri;
		}
	}
}

