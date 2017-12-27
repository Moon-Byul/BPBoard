using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPBoard
{
    public class SendWeb
    {
        public static String GetJsonToWeb(String webURI, String parameter)
        {
            if (parameter != null)
            {
                var request = WebRequest.Create(webURI);
                byte[] data = Encoding.UTF8.GetBytes(parameter);

                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                request.ContentLength = data.Length;
                request.Method = "POST";

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            else
                return "";
        }
    }
}
