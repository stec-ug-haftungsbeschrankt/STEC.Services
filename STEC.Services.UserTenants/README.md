# STEC.Services.UserTenants

This package provides multi tenancy bansed on EntityFramework Core. The sharding is done on the table level (schemas are used). It makes the DbContext and the Migrations Schema aware.

To use this package, you have to keep some things in mind:

- Your Database context has to inherit from ApplicationDbContextBase
- The constructor should take an ITenantProvider argument and pass it to the base class constructor
- You should register each DbSet that has to be schema aware in the OnModelCreating()-Method

```
// Example Registration
modelBuilder.ApplyConfiguration(new SchemaEntityConfiguration<ProjectDbModel>(Schema, nameof(Projects)));
```

- Migrations need to be adapted. Add the constructor below and use the _schema Variables in the up and down methods

```
        private string _schema;

        public InitialCreate(ApplicationDbContext _context)
        {
            _schema = _context.Schema ?? throw new ArgumentNullException(nameof(_context));
        }
```



## Database Setup

Database Setup in the Startup.cs

```
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("ApplicationDbContext")));

            services.AddDbContext<TenantDbContext>(options =>
                options.UseNpgsql(
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