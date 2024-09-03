using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BookingProject.Utilitarios;
using BookingProject.Utilities;
using Newtonsoft.Json.Linq;

namespace BookingProject.RestfulBooker
{
    [TestClass]
    public class DeleteBooking
    {
        private readonly CreateToken _createToken;
        private readonly CreateBooking _createBooking;
        private readonly ReadBooking _readBooking;

        public DeleteBooking()
        {
            _createToken = new CreateToken(); // Cria uma instância de CreateToken
            _createBooking = new CreateBooking(); // Cria uma instância de CreateBooking
            _readBooking = new ReadBooking(); // Cria uma instância de CreateRead
        }

        [TestMethod]
        public async Task TestMethodDelete()
        {
            

            // Obter um novo token antes de continuar
            await _createToken.TestMethodAuth();

            // Certifica que o bookingId foi obtido antes de continuar
            if (BaseVariables.bookingId == 0)
            {
                await _createBooking.TestMethodCreate();
            }

            if (BaseVariables.bookingId != 0)
            {
                await _readBooking.TestMethodRead();
            }

            var variables = new BaseVariables();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://restful-booker.herokuapp.com/booking/{BaseVariables.bookingId}");
            request.Headers.Add("Cookie", $"token={BaseVariables.token}");
            var content = new StringContent(string.Empty);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Ler o conteúdo da resposta como string (se necessário)
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Delete Response: {jsonResponse}");

            // Verificar se a exclusão foi bem-sucedida tentando ler a reserva excluída
            var readResponse = await client.GetAsync($"{BaseVariables.baseUrl}/booking/{BaseVariables.bookingId}");
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, readResponse.StatusCode);

        }
    }
}
