using System;
using System.Collections.Generic;
using System.Text;

namespace PushY.Models
{
    public class UserChatModel
    {
        public string Id { get; set; }
        public string from_id { get; set; }
        public string message { get; set; }
        public string to_id { get; set; }
        public string NickName { get; set; }
    }
}
