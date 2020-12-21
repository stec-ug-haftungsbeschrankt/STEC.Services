using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace STEC.Services.UserTenants
{
    public class ApplicationCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            if (context is ApplicationDbContextBase)
            {
                ApplicationDbContextBase myContext = context as ApplicationDbContextBase;
                return (context.GetType(), myContext.Schema);
            }
            return context.GetType();
        }
    }
}