using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Note.Models
{
    public class Users : IdentityUser<int> //chỉ định Id kiểu int
    {
        //IdentityUser đã có id, username, password
        public int? RoleID { get; set; }
        public Roles? Role { get; set; }
        public ICollection<Notes>? Notes { get; set; }
    }
}