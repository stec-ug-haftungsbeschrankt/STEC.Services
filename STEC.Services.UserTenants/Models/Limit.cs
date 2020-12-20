using System;

namespace STEC.Services.UserTenants
{
    public enum LimitType
    {
        Users,
        Objects
    }

    public class Limit
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public LimitType Type { get; set; }

        public int Value { get; set; }
    }
}