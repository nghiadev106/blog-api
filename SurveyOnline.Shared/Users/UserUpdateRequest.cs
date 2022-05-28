using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Shared.Users
{
    public class UserUpdateRequest
    {
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage = "Bạn phải nhập email")]
        [StringLength(256, ErrorMessage = "Email không quá 256 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập họ và tên")]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn quyền")]
        public string Role { get; set; }

        [StringLength(50, ErrorMessage = "Mật khẩu không quá 50 ký tự")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không chính xác")]
        [StringLength(50, ErrorMessage = "Mật khẩu xác nhận không quá 50 ký tự")]
        public string ConfirmPassword { get; set; }
    }
}
