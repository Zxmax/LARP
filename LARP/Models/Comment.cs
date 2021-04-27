using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LARP.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CommentId { get; set; }

        [Required]
        public string UserId { get; set; }
        public int ScriptId { get; set; }
        [Required]
        public StarRate? Rate { get; set; }
        [Required]
        public StarRate? EmotionDegree { get; set; }
        [Required]
        public StarRate? InferenceDifficulty { get; set; }
        [Required]
        public StarRate? DmImportance { get; set; }
        public string CommentText { get; set; }

    }
}
