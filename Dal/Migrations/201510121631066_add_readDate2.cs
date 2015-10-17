namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_readDate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "ReadDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "ReadDate");
        }
    }
}
