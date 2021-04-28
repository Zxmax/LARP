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
        [Display(Name = "整体评分")]
        public StarRate? Rate { get; set; }
        [Required]
        [Display(Name = "情感指数")]
        public StarRate? EmotionDegree { get; set; }
        [Required]
        [Display(Name = "推理指数")]
        public StarRate? InferenceDifficulty { get; set; }
        [Required]
        [Display(Name = "DM重要程度")]
        public StarRate? DmImportance { get; set; }
        [Display(Name = "评价")]
        public string CommentText { get; set; }

    }
}
