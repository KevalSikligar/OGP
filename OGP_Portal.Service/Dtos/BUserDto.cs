using System;
using System.Collections.Generic;
using System.Text;

namespace OGP_Portal.Service.Dtos
{
    public class BUserDto : BaseModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }

    }

    public class BPartnerDto : BaseModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public int Quantity_of_OGP { get; set; }
        public string   location  { get; set; }


    }
}
