using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20130623175035)]
    public class M20130623175035_AddCarriageColumnToPendingItems : Migration
    {
        public override void Up()
        {
            Alter.Table("PendingOrderItems").AddColumn("Carriage").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Carriage").FromTable("PendingOrderItems");
        }
    }
}