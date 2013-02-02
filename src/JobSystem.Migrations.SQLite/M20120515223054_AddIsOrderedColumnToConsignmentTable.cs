using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20120515223054)]
    public class M20120515223054_AddIsOrderedColumnToConsignmentTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Consignments").AddColumn("IsOrdered").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}