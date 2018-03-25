namespace DbHander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _19082017 : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
              "GetUserList",
              p => new
              {
                  userId = p.Int()
              },
              @"WITH cte AS  
	             (
	              SELECT a.UserId
	              FROM Users a
	              WHERE UserId = @userId
	              UNION All
	              SELECT a.UserId
	              FROM Users a JOIN cte c ON a.parentId = c.UserId
	              )
	              SELECT UserId
	              FROM cte"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("GetUserList");
        }
    }
}
