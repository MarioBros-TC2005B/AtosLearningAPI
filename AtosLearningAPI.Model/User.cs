using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtosLearningAPI.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserNickname { get; set; }
        public int CharacterId { get; set; }
        public string UserImage { get; set; }
        public float UserTotalScore { get; set; }
        
    }
}
