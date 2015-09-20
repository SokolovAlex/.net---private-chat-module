namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_created_date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Roles", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Login", c => c.String());
            DropColumn("dbo.Roles", "CreateDate");
            DropColumn("dbo.Users", "CreateDate");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Messages", "CreateDate");
        }
    }
}
