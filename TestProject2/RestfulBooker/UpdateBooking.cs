using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingProject.Utilitarios;
using BookingProject.Utilities;
using Newtonsoft.Json.Linq;

namespace BookingProject.RestfulBooker
{
    [TestClass]
    public class UpdateBooking
    {
        private readonly CreateToken _createToken;
        private readonly CreateBooking _createBooking;


        public UpdateBooking()
        {
            _createToken = new CreateToken(); // Cria uma instância de CreateToken
            _createBooking = new CreateBooking(); // Cria uma instância de CreateBooking
        }


        [TestMethod]
        public async Task TestMethodUpdate()
        {
            

            if (BaseVariables.token == null)
            {
                await _createToken.TestMethodAuth();
            }

            if (BaseVariables.bookingId == 0)
            {
                await _createBooking.TestMethodCreate();
            }
            var variables = new BaseVariables();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseVariables.baseUrl}/booking/{BaseVariables.bookingId}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Cookie", $"token={BaseVariables.token}");
            var content = new StringContent($@"
            {{
            ""firstname"" : ""Ricardo"",
            ""lastname"" : ""Medices"",
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
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(200, statusCode, $"O código retornado foi {statusCode}, esperado 200");

            // Ler o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Booking Details: {jsonResponse}");


        }

        [TestMethod]
        public async Task TestMethodNotUpdate()
        {

            if (BaseVariables.token == null)
            {
                await _createToken.TestMethodAuth();
            }

            var variables = new BaseVariables();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseVariables.baseUrl}/booking/0");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Cookie", $"token={BaseVariables.token}");
            var content = new StringContent($@"
            {{
            ""firstname"" : ""Ricardo"",
            ""lastname"" : ""Medices"",
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
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(405, statusCode, $"O código retornado foi {statusCode}, esperado 405");

            // Ler o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Booking Details: {jsonResponse}");

        }
    }
}
