using System.Web;
using System.Web.Mvc;

namespace EugeneCommunity
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());      // This filter sets all controllers to require authorization.
                                                        // [AllowAnonymous] annotation is used for public controls and login/register
        }
    }
}
