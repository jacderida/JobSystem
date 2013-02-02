using System;
using System.Collections.Generic;
using FluentMigrator;

namespace JobSystem.Migrations
{
    [Migration(20120225225031)]
    public class M20120225225031_AddJobAttachments : Migration
    {
        public override void Up()
        {
            Create.Table("AttachmentsJobs")
                .WithColumn("JobId").AsGuid().NotNullable()
                .WithColumn("AttachmentId").AsGuid().NotNullable();
            Create.ForeignKey("FK_AttachmentsJobsJobs")
                .FromTable("AttachmentsJobs")
                .ForeignColumn("JobId")
                .ToTable("Jobs")
                .PrimaryColumn("Id");
            Create.ForeignKey("FK_AttachmentsJobsAttachments")
                .FromTable("AttachmentsJobs")
                .ForeignColumn("AttachmentId")
                .ToTable("Attachments")
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_AttachmentsJobsAttachments");
            Delete.ForeignKey("FK_AttachmentsJobsJobs");
            Delete.Table("AttachmentsJobs");
        }
    }
}