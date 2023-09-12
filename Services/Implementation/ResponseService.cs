using FlightReservationConsole.Models;
using System;

namespace FlightReservationConsole.Services.Implementation
{
    public class ResponseService
    {
        public static void WriteResponse(CheapestFlight cheapestFlight)
        {
            Console.WriteLine($"Cheapest flight {cheapestFlight.Price}");
            Console.WriteLine($"Can be booked on url: {cheapestFlight.BookingUrl}");

        }
    }
}
