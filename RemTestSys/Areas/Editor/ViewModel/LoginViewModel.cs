using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(20)]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
