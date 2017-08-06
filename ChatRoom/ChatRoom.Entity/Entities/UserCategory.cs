using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Entity.Entities
{
    public class UserCategory
    {
        [Key]
        public int CaegroyId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public string Describe { get; set; }
    }
}
