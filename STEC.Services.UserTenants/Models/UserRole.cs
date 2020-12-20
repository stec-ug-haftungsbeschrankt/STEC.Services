using System;

namespace STEC.Services.UserTenants
{
    public enum UserRoleType
    {
        Free,
        Subscriber,
        Administrator
    }

    public class UserRole
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public UserRoleType RoleType { get; set; }
    }
}