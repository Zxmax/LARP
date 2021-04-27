using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LARP.Models
{
    public class CommentShow
    {
        public Script Script { get; set; }

        public List<InnerComment> Comments { get; set; }
    }

    public class InnerComment
    {
        public InnerComment(string userNickName, string comCommentText)
        {
            UserNickName = userNickName;
            CommentText = comCommentText;
        }

        public string UserNickName { get; set; }
        public string Avatar { get; set; }
        public string CommentText { get; set; }
        public StarRate? Rate { get; set; }
        public StarRate? EmotionDegree { get; set; }
        public StarRate? InferenceDifficulty { get; set; }
        public StarRate? DmImportance { get; set; }
    }
}
