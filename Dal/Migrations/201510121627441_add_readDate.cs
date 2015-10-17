namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_readDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "StatusId", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "StatusId");
        }
    }
}
