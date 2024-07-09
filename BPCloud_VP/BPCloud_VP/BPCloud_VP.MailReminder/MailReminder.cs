using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud_VP.MailReminder
{
    public class MailReminder
    {
        public static string BaseAddress = ConfigurationManager.AppSettings["BaseAddress"];
        public static void StartMailReminder()
        {
            SendDeliveryReminder();
        }

        public static void SendDeliveryReminder()
        {
            try
            {
                WriteLog.WriteToFile("SendDeliveryReminder method has been called");
                string Rest_Address = "poapi/PO/SendDeliveryNotification";
                Uri url = new Uri(BaseAddress + Rest_Address);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UseDefaultCredentials = true;
                //request.PreAuthenticate = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Proxy = null;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();
                    WriteLog.WriteToFile("Item Delivery due reminder mail has been sent successfully to the vendors");
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile($"ERROR : SendDeliveryReminder :- {ex.Message}", ex);
            }
        }
    }
}
