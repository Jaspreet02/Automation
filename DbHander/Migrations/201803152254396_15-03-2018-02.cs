namespace DbHander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1503201802 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
        }
    }
}
