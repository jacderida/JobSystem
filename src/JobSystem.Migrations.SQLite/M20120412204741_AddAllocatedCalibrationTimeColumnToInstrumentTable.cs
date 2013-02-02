using FluentMigrator;

namespace JobSystem.Migrations.SqlLite
{
    [Migration(20120412204741)]
    public class M20120412204741_AddAllocatedCalibrationTimeColumnToInstrumentTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Instruments").AddColumn("AllocatedCalibrationTime").AsInt32().Nullable(); 
        }

        public override void Down()
        {
            Delete.Column("AllocatedCalibrationTime").FromTable("Instruments");
        }
    }
}