using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace STEC.Services.UserTenants
{

    public class DbSchemaAwareMigrationAssembly : MigrationsAssembly
    {
        private readonly DbContext _context;

        public DbSchemaAwareMigrationAssembly(ICurrentDbContext currentContext,
              IDbContextOptions options, IMigrationsIdGenerator idGenerator,
              IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
          : base(currentContext, options, idGenerator, logger)
        {
            _context = currentContext.Context;
        }

        public override Migration CreateMigration(TypeInfo migrationClass,
              string activeProvider)
        {
            if (activeProvider == null)
                throw new ArgumentNullException(nameof(activeProvider));

            if (migrationClass == null)
                throw new ArgumentNullException(nameof(migrationClass));

            var hasCtorWithSchema = migrationClass
                    .GetConstructor(new[] { typeof(ApplicationDbContextBase) }) != null;

            if (hasCtorWithSchema && _context is ApplicationDbContextBase schema)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), schema);
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}