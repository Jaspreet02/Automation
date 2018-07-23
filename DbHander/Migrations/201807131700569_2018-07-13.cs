namespace DbHander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20180713 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Contact", c => c.String(nullable: false, maxLength: 13));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Contact", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
