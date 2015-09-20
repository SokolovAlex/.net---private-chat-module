namespace Dal.Migrations
{
    using DbEntities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Dal.Context.PrivatChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Dal.Context.PrivatChatContext context)
        {
            context.Roles.AddOrUpdate(
             new Role { Name = "Admin", Id = (int)Core.Enums.UserRole.Admin, CreateDate = DateTime.Now },
             new Role { Name = "Client", Id = (int)Core.Enums.UserRole.Client, CreateDate = DateTime.Now }
            );
            context.SaveChanges();

            context.Users.AddOrUpdate(
              new User { Email = "a@a.com", Name = "Admin", Password="Admin", UserRole = Core.Enums.UserRole.Admin, CreateDate = DateTime.Now },
              new User { Email = "cl@cl.com", Name = "Viny-puh", Password = "123", UserRole = Core.Enums.UserRole.Client, CreateDate = DateTime.Now },
              new User { Email = "client@client.com", Name = "Tigra", Password = "123", UserRole = Core.Enums.UserRole.Client, CreateDate = DateTime.Now }
            );
            context.SaveChanges();
        }
    }
}
