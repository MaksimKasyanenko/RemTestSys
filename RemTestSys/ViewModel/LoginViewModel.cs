using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string StudentLogId { get; set; }
    }
}
