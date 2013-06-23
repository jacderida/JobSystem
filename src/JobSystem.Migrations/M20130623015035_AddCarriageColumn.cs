using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130623015035)]
    public class M20130623015035_AddCarriageColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("OrderItems").AddColumn("Carriage").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Carriage").FromTable("OrderItems");
        }
    }
}