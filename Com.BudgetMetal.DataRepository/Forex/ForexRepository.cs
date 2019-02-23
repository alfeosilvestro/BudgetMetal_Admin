using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DataRepository.Forex
{
    public class ForexRepository : IForexRepository
    {
        private const string URL = "http://forex.cbm.gov.mm/api/latest";
        private string urlParameters = "";

        public async Task<string> GetForexDataFromBankApi()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.GetAsync(urlParameters);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var forexData = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll
                client.Dispose();
                return forexData;
            }
            else
            {
                client.Dispose();
                return "";
            }

        }
    }
}
