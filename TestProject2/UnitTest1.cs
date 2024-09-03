
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace BookingProject
{
    [TestClass]
    public class UnitTest1
    {

        private static readonly string baseUrl = "https://restful-booker.herokuapp.com";
        private string _token;
        private int _bookingId;

        [TestMethod]
        public async Task TestMethodAuth()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/auth");
            var content = new StringContent($@"
            {{
            ""username"" : ""admin"",    
            ""password"" : ""password123""
            }}", null, "application/json");

            request.Content = content;
            var response = await client.SendAsync(request);
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(200, statusCode, $"O código retornado foi {statusCode}, esperado 200");
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            // Ler o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse o JSON e extrair o token
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                var root = doc.RootElement;
                _token = root.GetProperty("token").GetString();
                Console.WriteLine($"Token: {_token}");
            }

        }

        [TestMethod]
        public async Task TestMethodCreate()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/booking");
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
            Assert.AreEqual(200,statusCode,$"O código retornado foi {statusCode}, esperado 200");
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            // Ler o conteúdo da resposta como string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse o JSON e extrair o bookingid
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                var root = doc.RootElement;
                _bookingId = root.GetProperty("bookingid").GetInt32();
                Console.WriteLine($"Booking ID: {_bookingId}");
            }

        }

        [TestMethod]
        public async Task TestMethodRead()
        {

            if (_bookingId == 0)
            {
                await TestMethodCreate();
            }

            //Utiliza o bookingId para obter detalhes da reserva
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://restful-booker.herokuapp.com/booking/{_bookingId}");
            request.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(request);
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(200, statusCode, $"O código retornado foi {statusCode}, esperado 200");
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }

        [TestMethod]
        public async Task TestMethodUpdate()
        {
            if (_token == null)
            {
                await TestMethodAuth();
            }

            if (_bookingId == 0)
            {
                await TestMethodCreate();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://restful-booker.herokuapp.com/booking/{_bookingId}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Cookie", $"token={_token}");
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
        public async Task TestMethodDelete()
        {
            // Obter um novo token antes de continuar
            await TestMethodAuth();

            // Certifica que o bookingId foi obtido antes de continuar
            if (_bookingId == 0)
            {
                await TestMethodCreate();
            }

            if (_bookingId != 0)
            {
                await TestMethodRead();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://restful-booker.herokuapp.com/booking/{_bookingId}");
            request.Headers.Add("Cookie", $"token={_token}");
            var content = new StringContent(string.Empty);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Ler o conteúdo da resposta como string (se necessário)
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Delete Response: {jsonResponse}");

            // Verificar se a exclusão foi bem-sucedida tentando ler a reserva excluída
            var readResponse = await client.GetAsync($"{baseUrl}/booking/{_bookingId}");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, readResponse.StatusCode);

        }

    }
}