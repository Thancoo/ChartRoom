using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Entity.Entities.Identity
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "{0} 不可为空！")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} 不可为空！")]
        [StringLength(30, ErrorMessage = "{0}必须含有7-30个字符的字符！", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}
