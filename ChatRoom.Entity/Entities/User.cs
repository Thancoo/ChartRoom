using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatRoom.Entity.Entities
{
    public class User: IdentityUser,IUser,IUser<string>
    {
        [Key] 
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public User()
        {
            Messages = new List<Message>();
        }

        string IUser<string>.Id
        {
            get { return base.Id; }
        }
    }
}
