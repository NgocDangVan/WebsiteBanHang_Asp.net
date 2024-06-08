﻿namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Order_Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Status");
        }
    }
}
