using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace EmployeeManagement.Notifications
{
    public class WhatsAppNotification
    {
        public static bool Send(WhatsAppRequest whatsAppRequest)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = WhatsAppHelper.GetWhatsAppApiRequest(whatsAppRequest);

                    var postTask = client.GetAsync(apiUrl);
                    postTask.Wait();

                    var result = postTask.Result;

                    return result.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}