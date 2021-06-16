using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Dtos
{
    public class ResetPasswordDto
    {
        public long Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
