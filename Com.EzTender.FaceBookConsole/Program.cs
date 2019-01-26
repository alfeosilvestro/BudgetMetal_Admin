using Com.BudgetMetal.Services.Facebook;
using System;
using System.Threading.Tasks;

namespace Com.EzTender.FaceBookConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessToken = "EAAFJo1TZCiLQBAHNdAJO8E51r5M6czwxR6DDyonLSaxuUTfmaZBDhAUleRAU10kR0FWXGGUHa8NCncVJZBuJCJODwIPPqCJDFtbJnRzo7lgcUGVf4gU6t86Uw46WKVI8BxxYdQenJsnMR6n8jNuFAkQZAJlxJUDx1snoDvXppWZBkiZCAXm8F0uoI1pFDNYYPxcZCjuZCrtd5wZDZD";

            IFacebookClient facebookClient = new FacebookClient();
            var facebookService = new FacebookService(facebookClient);
            var getAccountTask = facebookService.GetAccountAsync(accessToken);

            Task.WaitAll(getAccountTask);
            var account = getAccountTask.Result;
            Console.WriteLine($"{account.Id} {account.Name}");

            var postOnWallTask = facebookService.PostOnWallAsync(accessToken, "Hello from C# .NET Core!");
            Task.WaitAll(postOnWallTask);

            Console.WriteLine("Completed Post.");

            Console.ReadLine();
        }
    }
}
