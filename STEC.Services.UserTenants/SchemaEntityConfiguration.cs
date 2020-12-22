using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace STEC.Services.UserTenants
{
    public class SchemaEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
	{
		private readonly string _schema;

		private readonly string _dbSet;

		public SchemaEntityConfiguration(string schema, string dbSet)
		{
			_schema = schema;
			_dbSet = dbSet;
		}

		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<T> builder)
		{
			if (!string.IsNullOrWhiteSpace(_schema))
				builder.ToTable(_dbSet, _schema);

			//builder.HasKey(o => o.ID);
		}
	}
}
