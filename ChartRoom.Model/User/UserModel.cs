using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.Model.User
{
    public class UserModel
    {
        [Required(ErrorMessage = "{0}不可为空！")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [Display(Name = "性别")]
        [DefaultValue("男")]
        public string Sex { get; set; }
        [Display(Name = "头像")]
        [DefaultValue("")]
        [DataType(dataType:DataType.ImageUrl,ErrorMessage = "必须为ImageUrl")]
        public string HeardImage { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}不可为空！")]
        [StringLength(30, ErrorMessage = "{0}必须含有7-30个字符的字符！", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次密码输入不一致！")]
        public string ConfirmPassword { get; set; }
    }
}
