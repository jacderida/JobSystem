using FluentMigrator.Builders.Create.Table;

namespace JobSystem.Migrations.SqlLite
{
	public static class MigrationExtensions
	{
		public static ICreateTableWithColumnSyntax WithIdColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
		{
			return tableWithColumnSyntax.WithColumn("Id").AsGuid().NotNullable().PrimaryKey();
		}

		public static ICreateTableWithColumnSyntax WithArchiveColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
		{
			return tableWithColumnSyntax.WithColumn("IsArchived").AsBoolean().NotNullable().WithDefaultValue(false);
		}
	}
}