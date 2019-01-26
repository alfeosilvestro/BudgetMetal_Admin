using Com.BudgetMetal.Services.Facebook;
using System;
using System.Threading.Tasks;

namespace Com.EzTender.FaceBookConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Refer to these urls:
            // https://piotrgankiewicz.com/2017/02/06/accessing-facebook-api-using-c/
            // https://retifrav.github.io/blog/2017/11/25/csharp-dotnet-core-publish-facebook/

            //string accessToken = "EAADalhNJu1oBAIZAx9YIrFQV1qlWCGhGTktXUZBmuISW2fH492oZAl8XfglYd0YZADAUZBkSoS61EbKBreExbqZAqByoLLDCeEEiQA4GcCen5ZCv8njilRJwm8YxjvotHr9jqnTwgQSgEqka9cQNhiBQVCPwCnt353rNZCvHyZCVI8AYY4W6ya9gQiiL5bngkTbaCZAUwq2oaBYgZDZD";
            string accessToken = "EAALZC244IWroBALB0BR7btIEykrZBlBxkQgJsXCmZCVlsUvgwkGDFhBcVJ8pKXY2owYisAwNCKMYEZCGoEoZC2YO94KeXldUgeVWR2BfCi6C3ZBJKNsNMZBxhngGKq6kRJR6IaHaDJDR3hlA8uzIO2xrMSMGixjFCDTZAqvP6kUZBJiIoErGt20V1HCZAj9y1KFVKpSVY8eTidJp2XAtbNZCGfTUZAdO9M2f1KwZD";
            string pageId = "me";

            string facebookAppId = "844268399254202";
            string facebookAppSecret = "1fabfd9399940e1aa4d386be2173ae38";

            /*
             Facebook user name: ezytender@gmail.com
             Facebook user pass: nnhhyy66
             */

            IFacebookService fsvs = new FacebookService(accessToken, pageId);
            fsvs.SimplePost("Test");

            //IFacebookClient facebookClient = new FacebookClient();
            //var facebookService = new FacebookService(facebookClient, pageId);
            //var getAccountTask = facebookService.GetAccountAsync(accessToken);

            //Task.WaitAll(getAccountTask);
            //var account = getAccountTask.Result;
            //Console.WriteLine($"{account.Id} {account.Name} {account.Id} {account.Email}");

            //var postOnWallTask = facebookService.PostOnWallAsync(accessToken, "Hello from C# .NET Core!");
            //Task.WaitAll(postOnWallTask);

            Console.WriteLine("Completed Post.");

            Console.ReadLine();
        }
    }
}
