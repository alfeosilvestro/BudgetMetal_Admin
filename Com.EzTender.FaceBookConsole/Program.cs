using Com.BudgetMetal.Services.Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Com.EzTender.FaceBookConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //string accessToken = "EAADZAmS24c8kBACYQU1vh3UFoUCF323UnVZCBUF7A8BDg4ZBJtdBt1YtU6JCO7ho5J9oLVnbOPSUViFhnhsAmHoA6NtptejFV6En2kxxQuOKpn8esglUwgHzOPTPBl8BIlu0ECa4e1AmEtO6hNId1LgB72j7EUr30RmRXipRm0ZA0BPlrZBxhooP5oqtQ6JhlU7RkJaXsDAZDZD";


            //IFacebookClient facebookClient = new FacebookClient();
            //var facebookService = new FacebookService(facebookClient);
            //var getAccountTask = facebookService.GetAccountAsync(accessToken);

            //Task.WaitAll(getAccountTask);
            //var account = getAccountTask.Result;
            //Console.WriteLine($"{account.Id} {account.Name}");

            //var postOnWallTask = facebookService.PostOnWallAsync(accessToken, "Hello from C# .NET Core!");
            //Task.WaitAll(postOnWallTask);

            //Console.WriteLine("Completed Post.");

            //Console.ReadLine();

            getpageTokens();
        }

        public static void getpageTokens()
        {
            // User Access Token  we got After authorization
            string UserAccesstoken = "EAADZAmS24c8kBACYQU1vh3UFoUCF323UnVZCBUF7A8BDg4ZBJtdBt1YtU6JCO7ho5J9oLVnbOPSUViFhnhsAmHoA6NtptejFV6En2kxxQuOKpn8esglUwgHzOPTPBl8BIlu0ECa4e1AmEtO6hNId1LgB72j7EUr30RmRXipRm0ZA0BPlrZBxhooP5oqtQ6JhlU7RkJaXsDAZDZD";
            string url = string.Format("https://graph.facebook.com/" +
                         "me/accounts?access_token={0}", UserAccesstoken);

            WebRequest wr= null ;
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.Method = "Get";

            var webResponse = wr.GetResponse();
            StreamReader sr = null;

            sr = new StreamReader(webResponse.GetResponseStream());
            var vdata = new Dictionary<string, string>();
            string returnvalue = sr.ReadToEnd();

            //using Jobject to parse result
            JObject mydata = JObject.Parse(returnvalue);
            JArray data = (JArray)mydata["data"];

            PosttofacePage(data);
        }


        public static void PosttofacePage(JArray obj)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                string name = (string)obj[i]["name"];
                string Accesstoken = (string)obj[i]["access_token"];
                string category = (string)obj[i]["category"];
                string id = (string)obj[i]["id"];

                string Message = "Test message";

                if (string.IsNullOrEmpty(Message)) return;

                // Append the user's access token to the URL
                string path = String.Format("https://graph.facebook.com/{0}", id);

                var url = path + "/feed?" +
                AppendKeyvalue("access_token", Accesstoken);

                // The POST body is just a collection of key=value pairs,
                // the same way a URL GET string might be formatted
                var parameters = ""
                + AppendKeyvalue("name", "name")
                + AppendKeyvalue("caption", "a test caption")
                + AppendKeyvalue("description", "test description ")
                + AppendKeyvalue("message", Message);
                // Mark this request as a POST, and write the parameters
                // to the method body (as opposed to the query string for a GET)
                var webRequest = WebRequest.Create(url);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
                webRequest.ContentLength = bytes.Length;
                System.IO.Stream os = webRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();

                // Send the request to Facebook, and query the result to get the confirmation code
                try
                {
                    var webResponse = webRequest.GetResponse();
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(webResponse.GetResponseStream());
                        string PostID = sr.ReadToEnd();
                    }
                    finally
                    {
                        if (sr != null) sr.Close();
                    }
                }
                catch (WebException ex)
                {
                    // To help with debugging, we grab
                    // the exception stream to get full error details
                    StreamReader errorStream = null;
                    try
                    {
                        errorStream = new StreamReader(ex.Response.GetResponseStream());
                        //this.ErrorMessage = errorStream.ReadToEnd();
                    }
                    finally
                    {
                        if (errorStream != null) errorStream.Close();
                    }
                }
            }
        }


        public static string AppendKeyvalue(string key, string value)
        {
            return string.Format("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value));
        }

    }
}
