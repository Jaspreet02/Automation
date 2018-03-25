namespace DbHander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2701201801 : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
              "GetUserList",
              p => new
              {
                  userId = p.String()
              },
              @"WITH cte AS  
	             (
	              SELECT a.Id
	              FROM AspNetUsers a
	              WHERE Id = @userId
	              UNION All
	              SELECT a.Id
	              FROM AspNetUsers a JOIN cte c ON a.parentId = c.Id
	              )
	              SELECT Id
	              FROM cte"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("GetUserList");
        }
    }
}
