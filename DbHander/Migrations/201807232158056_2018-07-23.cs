namespace DbHander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20180723 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FileTransferSettings", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FileTransferSettings", "UserId", c => c.Int(nullable: false));
        }
    }
}
