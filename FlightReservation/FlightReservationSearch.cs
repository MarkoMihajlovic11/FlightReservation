using FlightReservationConsole.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace FlightReservationConsole
{
    public class FlightReservationSearch
    {
        private readonly IUserInputService _userInputService;
        private readonly IFlightSearchService _flightSearchService;
        private readonly IResponseService _responseService;

        public FlightReservationSearch(IFlightSearchService flightSearchService, IResponseService responseService, IUserInputService userInputService)
        {
            _flightSearchService = flightSearchService;
            _responseService = responseService;
            _userInputService = userInputService;
        }

        public async Task<string> RunAsync()
        {
            try
            {
                var flightReservation = _userInputService.GetFlightReservation();
                var cheapestFlight = await _flightSearchService.FindCheapestFlight(flightReservation);

                var response = _responseService.WriteResponse(cheapestFlight);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Try again y/n:");
                var answer = Console.ReadLine();

                if (answer == "y")
                    return await RunAsync();
                else
                    return ex.Message;
            }
        }
    }
}
