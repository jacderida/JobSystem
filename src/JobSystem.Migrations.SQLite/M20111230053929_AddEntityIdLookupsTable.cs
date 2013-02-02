using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20111230053929)]
    public class M20111230053929_AddEntityIdLookupsTable : Migration
    {
        public override void Up()
        {
            Create.Table("EntityIdLookups")
                .WithIdColumn()
                .WithColumn("EntityTypeName").AsString(255).NotNullable()
                .WithColumn("NextId").AsInt32().NotNullable()
                .WithColumn("Suffix").AsString(255).Nullable()
                .WithColumn("Prefix").AsString(255).Nullable()
                .WithColumn("Version").AsInt32();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}