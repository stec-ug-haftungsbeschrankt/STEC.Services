using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace STEC.Services.UserTenants
{
    public partial class SchemaAwareMigrationBase : Migration
    {
        protected string _schema;


        public SchemaAwareMigrationBase()
        {
            _schema = "public";
        }


        public SchemaAwareMigrationBase(ApplicationDbContextBase _context)
        {
            _schema = _context.Schema ?? throw new ArgumentNullException(nameof(_context));
        }

        protected override void Up([NotNullAttribute] MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down([NotNullAttribute] MigrationBuilder migrationBuilder)
        {

        }
    }
}
