using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Notifications
{
    public class ConfigConstants
    {
        public static string WhatsAppApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["whatsAppApiUrl"];
            }
        }

        public static string WhatsAppInstanceId
        {
            get
            {
                return ConfigurationManager.AppSettings["whatsAppInstanceId"];
            }
        }

        public static string WhatsAppAccessToken
        {
            get
            {
                return ConfigurationManager.AppSettings["whatsAppAccessToken"];
            }
        }

        public static string WAToNumbers
        {
            get
            {
                return ConfigurationManager.AppSettings["WAToNumbers"];
            }
        }
    }
}