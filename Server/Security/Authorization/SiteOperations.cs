using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Server.Security.Authorization
{
    public static class SiteOperations
    {
        public static OperationAuthorizationRequirement CanViewProfile = new OperationAuthorizationRequirement {
            Name = Constants.CanViewProfile 
        };
    }

    public class Constants
    {
        public static readonly string CanViewProfile = "canViewProfile";

        public static readonly string SiteSuperAdminRole = "superAdmin";
        public static readonly string SiteAdministratorsRole = "admin";
        public static readonly string SiteUsersRole = "user";
    }
}
