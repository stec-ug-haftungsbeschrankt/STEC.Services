# STEC.Services.UserTenants


## Database Setup

```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("ApplicationDbContext")));

            services.AddDbContext<TenantDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("TenantDbContext"),
                    b => b.MigrationsAssembly("TaskManagement")
                )
            );

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TenantDbContext>();

            services.AddScoped<ITenantProvider, PostgreSqlTenantProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRazorPages();
        }
```