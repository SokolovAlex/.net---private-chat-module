namespace Dal.Migrations
{
    using DbEntities;
    using DevOne.Security.Cryptography.BCrypt;
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
            var salt1 = BCryptHelper.GenerateSalt();
            var salt2 = BCryptHelper.GenerateSalt();
            var salt3 = BCryptHelper.GenerateSalt();
            //  This method will be called after migrating to the latest version.

            context.Roles.AddOrUpdate(
             new Role { Name = "Admin", Id = (int)Core.Enums.UserRole.Admin, CreateDate = DateTime.Now },
             new Role { Name = "Client", Id = (int)Core.Enums.UserRole.Client, CreateDate = DateTime.Now }
            );
            context.SaveChanges();

            context.Users.AddOrUpdate(
              new User { Email = "a@a.com", Name = "Admin", PasswordHash = BCryptHelper.HashPassword("Admin", salt1),
                  Salt = salt1, UserRole = Core.Enums.UserRole.Admin, CreateDate = DateTime.Now, HashId = Guid.NewGuid()
              },
              new User { Email = "cl@cl.com", Name = "Viny-puh", PasswordHash = BCryptHelper.HashPassword("123", salt2),
                  Salt = salt2, UserRole = Core.Enums.UserRole.Client, CreateDate = DateTime.Now, HashId = Guid.NewGuid()
              },
              new User { Email = "client@client.com", Name = "Tigra", PasswordHash = BCryptHelper.HashPassword("123", salt3),
                  Salt = salt3, UserRole = Core.Enums.UserRole.Client, CreateDate = DateTime.Now, HashId = Guid.NewGuid()
              }
            );
            context.SaveChanges();
        }
    }
}
