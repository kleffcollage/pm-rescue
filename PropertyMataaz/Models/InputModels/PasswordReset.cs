using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.Models.InputModels
{
    public class PasswordReset
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
