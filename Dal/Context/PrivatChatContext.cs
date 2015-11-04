using Dal.DbEntities;
using Dal.DbEntities.PrivateChat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Context
{
    public class PrivatChatContext: System.Data.Entity.DbContext
    {

        public PrivatChatContext() : base("privateChatDb") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

      //  public DbSet<Category> Categories { get; set; }

        //public DbSet<User> User2Role { get; set; }


        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasRequired(t => t.Role)
                .WithMany(t => t.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(x => x.Author)
                .WithMany(x => x.MyMessages)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(x => x.Recipient)
                .WithMany(x => x.MessagesToMe)
                .WillCascadeOnDelete(false);
        }
    }
}
