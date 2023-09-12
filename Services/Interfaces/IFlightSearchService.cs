using FlightReservationConsole.Models;
using System.Threading.Tasks;

namespace FlightReservationConsole.Services.Interfaces
{
    public interface IFlightSearchService
    {
        Task<CheapestFlight> FindCheapestFlight(FlightReservation flightReservation);
    }
}
