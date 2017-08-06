using System.Data.Entity;
using ChatRoom.Entity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatRoom.Entity
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ChatContext: IdentityDbContext<IdentityUser>
    {
        public ChatContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> ChatUsers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}
