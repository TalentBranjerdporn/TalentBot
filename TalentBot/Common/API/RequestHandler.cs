using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TalentBot.Common.API
{
    public static class RequestHandler
    {
        const int MaxRetries = 3;
        public static string data { get; set; }

        public static Task<string> GET(string url)
        {
            return GET(url, 0);
        }

        private static async Task<string> GET(string url, int retries)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get the data stream that is associated with the specified URL.  
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        // Read the bytes in responseStream and copy them to content.
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        data = await reader.ReadToEndAsync();
                        return data;
                    }
                }

                //WebResponse response = await request.GetResponseAsync();
                //using (Stream responseStream = response.GetResponseStream())
                //{
                //    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                //    data = reader.ReadToEnd();
                //}
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    if (((HttpWebResponse)ex.Response).StatusCode == (HttpStatusCode)429)
                    {
                        if (retries <= MaxRetries)
                        {
                            Thread.Sleep(1000 * (retries + 1));
                            return await GET(url, retries + 1);
                        }
                    }
                }
                return null;
            }
        }
    }
}
