using FluentMigrator;

namespace JobSystem.Admin.Migrations
{
    [Migration(20120915210031)]
    public class M20120915210031_AddUserAccountTable : Migration
    {
        public override void Up()
        {
            Create.Table("UserAccounts")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("EmailAddress").AsString(255).NotNullable().Unique()
                .WithColumn("PasswordHash").AsString(255).NotNullable()
                .WithColumn("PasswordSalt").AsString(255).NotNullable();
            Execute.Sql(
                @"
                    INSERT INTO dbo.UserAccounts(Id, EmailAddress, PasswordHash, PasswordSalt)
                    VALUES('F2F85FD3-F732-4BDD-895F-DD854AE85162', 'admin@intertek.com',
                        'm03VJEu/FRkPzH1TxF20URdwUImJouESVKqh+se0P1h4o7XTLWs2N6hBSmP0B/xiao9Qr8XQeGUcraLkr2i0Ig==',
                        '1vB3mDy8/dqooo5R3h2NCXaP3mk3efoJe2nCQILgSC3jgEVHftnRLjsO42WyeSD6RJaR3LZPRRetO2i6D1adaw==')
                ");
        }

        public override void Down()
        {
            Delete.Table("UserAccounts");
        }
    }
}