using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace LARP.Models
{
    public class Script
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "剧本Id")]
        public int ScriptId { get; set; }
        [Display(Name = "剧本名")]
        public string Name { get; set; }
        [Display(Name = "评分")]
        public double? Rate { get; set; }
        [Display(Name = "简介")]
        public string Introduction { get; set; }
        [Display(Name = "封面")]
        public string Cover { get; set; }
        [NotMapped]
        public IFormFile CoverFile { get; set; }
        [Display(Name = "人数")]
        public int Players { get; set; }
        [Display(Name = "情感指数")]
        public double? EmotionDegree { get; set; }
        [Display(Name = "推理难度")]
        public double? InferenceDifficulty { get; set; }
        [Display(Name = "Dm重要程度")]
        public double? DmImportance { get; set; }

    }
}
