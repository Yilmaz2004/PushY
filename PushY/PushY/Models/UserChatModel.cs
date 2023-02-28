using System;
using System.Collections.Generic;
using System.Text;

namespace PushY.Models
{
    public class UserChatModel
    {
        public string Id { get; set; }
        public string From_Id { get; set; }
        public string Message { get; set; }
        public string To_Id { get; set; }
    }
}
