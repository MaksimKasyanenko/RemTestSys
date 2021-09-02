using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RemTestSys.Extensions
{
    public static class ControllerExtensions
    {
        public static bool TryGetLogIdFromCookie(this ControllerBase controller, out string logId)
        {
            logId = controller.HttpContext.User.FindFirstValue("StudentLogId");
            if (logId != null) return true;
            return false;
        }
    }
}
