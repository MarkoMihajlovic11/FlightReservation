using FlightReservationConsole.Services.Implementation;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace FlightReservationConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var flightReservation = UserInputService.GetFlightReservation();
                var flightSearchService = new FlightSearchService(ConfigurationManager.AppSettings["Path"]);
                var cheapestFlight = await flightSearchService.FindCheapestFlight(flightReservation);

                ResponseService.WriteResponse(cheapestFlight);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}