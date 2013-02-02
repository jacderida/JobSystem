using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120509001459)]
    public class M20120509001459_RemoveLocationIdFromJobItemsTable : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_JobItems_ItemLocation").OnTable("JobItems");
            Delete.Column("LocationId").FromTable("JobItems");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}