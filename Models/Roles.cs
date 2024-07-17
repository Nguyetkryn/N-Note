using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Note.Models
{
    public class Roles : IdentityRole<int> //chỉ định Id kiểu int
    {
        //IdentityRole cung cấp id, name
       public ICollection<Users>? Users { get; set; }  
    }
}