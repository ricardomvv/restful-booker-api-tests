using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingProject.Utilities;

namespace BookingProject.RestfulBooker
{
    [TestClass]
    public class ReadBooking
    {
        private readonly CreateBooking _createBooking;

        public ReadBooking()
        {
            _createBooking = new CreateBooking(); // Cria uma instância de CreateBooking
        }

        [TestMethod]
        public async Task TestMethodRead()
        {
            var variables = new BaseVariables();

            if (BaseVariables.bookingId == 0)
            {
                await _createBooking.TestMethodCreate();
            }

            //Utiliza o bookingId para obter detalhes da reserva
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseVariables.baseUrl}/booking/{BaseVariables.bookingId}");
            request.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(request);
            int statusCode = (int)response.StatusCode;
            Assert.AreEqual(200, statusCode, $"O código retornado foi {statusCode}, esperado 200");
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }
    }
}
