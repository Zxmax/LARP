using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LARP.Models
{
    public class ScriptUser : IdentityUser
    {
        public string NickName { get; set; }
        public int Sex { get; set; }
        public string Location { get; set; }
        public DateTime RegisterTime { get; set; }
        public byte[] Avatar { get; set; }
    }
}
