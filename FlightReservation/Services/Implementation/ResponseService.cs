using FlightReservationConsole.Models;
using FlightReservationConsole.Services.Interfaces;
using System;

namespace FlightReservationConsole.Services.Implementation
{
    public class ResponseService : IResponseService
    {
        public string WriteResponse(CheapestFlight cheapestFlight)
        {
            string message = $"Cheapest flight {cheapestFlight.Price} {Environment.NewLine}Can be booked on url: {cheapestFlight.BookingUrl}";
            Console.WriteLine(message);
            return message;

        }
    }
}
