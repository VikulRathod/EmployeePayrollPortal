using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Notifications
{
    public class WhatsAppRequest
    {
        public string Phone { get; set; }
        public string Body { get; set; }
        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
        text, media
    }
}