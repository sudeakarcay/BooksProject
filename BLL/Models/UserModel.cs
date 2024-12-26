using BLL.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserModel
    {
        public User Record { get; set; }

        [DisplayName("User Name")]
        public string UserName => Record.UserName;

        public string Password => Record.Password;

        [DisplayName("Status")]
        public string IsActive => Record.IsActive ? "Active" : "Not Active";

        public string Role => Record.Role?.Name;
    }
}
