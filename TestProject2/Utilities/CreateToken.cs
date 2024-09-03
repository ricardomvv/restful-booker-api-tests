using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookingProject.Utilities;

namespace BookingProject.Utilitarios
{
    [TestClass]
    public class CreateToken
    {

        [TestMethod]
        public async Task TestMethodAuth()
        {
            var variables = new BaseVariables();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseVariables.baseUrl}/auth");
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
                BaseVariables.token = root.GetProperty("token").GetString();
                Console.WriteLine($"Token: {BaseVariables.token}");
            }

        }
    }
}
