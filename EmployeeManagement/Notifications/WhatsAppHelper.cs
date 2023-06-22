using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EmployeeManagement.Notifications
{
    public class WhatsAppHelper
    {
        public static string GetWhatsAppApiRequest(WhatsAppRequest whatsAppRequest)
        {
            string url = ConfigConstants.WhatsAppApiUrl;
            string toMobile = whatsAppRequest.Phone;
            string type = whatsAppRequest.Type.ToString();
            string message = HttpUtility.UrlEncode(whatsAppRequest.Body);

            StringBuilder request = new StringBuilder();
            request.Append(url);
            request.Append($"?number=91{toMobile}");
            request.Append($"&type={type}");
            request.Append($"&message={message}");
            request.Append($"&instance_id={ConfigConstants.WhatsAppInstanceId}");
            request.Append($"&access_token={ConfigConstants.WhatsAppAccessToken}");

            return request.ToString();
        }
    }
}