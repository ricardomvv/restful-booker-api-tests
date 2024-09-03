using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookingProject.Utilities;

namespace BookingProject.RestfulBooker
{
    [TestClass]
    public class CreateBooking
    {
        [TestMethod]
        public async Task TestMethodCreate()
        {
            var variables = new BaseVariables();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseVariables.baseUrl}/booking");
            request.Headers.Add("Accept", "application/json");
            var content = new StringContent($@"
            {{
            ""firstname"" : ""Jim"",
            ""lastname"" : ""Brown"",
            ""totalprice"" : 111,
            ""depositpaid"" : true,
            ""bookingdates"" : {{
                ""checkin"" : ""2018-01-01"",
                ""checkout"" : ""2019-01-01""
            }},
            ""additionalneeds"" : ""Breakfast""
            }}", null, "application/json");

            request.Content = content;
            var response = await client.SendAsync(request);
            // response.EnsureSuccessStatusCode();
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(200, statusCode, $"O código retornado foi {statusCode}, esperado 200");
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            // Ler o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse o JSON e extrair o bookingid
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                var root = doc.RootElement;
                BaseVariables.bookingId = root.GetProperty("bookingid").GetInt32();
                Console.WriteLine($"Booking ID: {BaseVariables.bookingId}");
            }

        }

        [TestMethod]
        public async Task TestMethodNotCreate()
        {
            var variables = new BaseVariables();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseVariables.baseUrl}/booking");
            request.Headers.Add("Accept", "application/json");
            var content = new StringContent($@"
            {{
            ""firstname"" : ""Jim"",
            ""lastname"" : """",
            ""totalprice"" : dad,
            ""depositpaid"" : true,
            ""bookingdates"" : {{
                ""checkin"" : ""20180101"",
                ""checkout"" : ""20190101""
            }},
            ""additionalneeds"" : ""Breakfast""
            }}", null, "application/json");

            request.Content = content;
            var response = await client.SendAsync(request);
            // response.EnsureSuccessStatusCode();
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(400, statusCode, $"O código retornado foi {statusCode}, esperado 400");
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            // Ler o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();


        }
    }
}
