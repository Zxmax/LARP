using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LARP.Models
{
    public class ScriptTypeBind
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "剧本Id")]
        public int ScriptId { get; set; }

        [Display(Name = "剧本类型")]
        public int ScriptTypeId { get; set; }
    }
}
