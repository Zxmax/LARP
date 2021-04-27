using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LARP.Models
{
    public class ScriptType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScriptTypeId { get; set; }
        public string ScriptTypeName { get; set; }
    }
}
