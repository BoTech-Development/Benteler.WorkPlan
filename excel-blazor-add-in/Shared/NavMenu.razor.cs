namespace Benteler.WorkPlan.Web.Shared
{
    public partial class NavMenu
    {
        public string LastPage { get; set; }
        public NavMenu()
        {
            if(NavigationManager != null)
                LastPage = NavigationManager.Uri;
        }
    }
}
