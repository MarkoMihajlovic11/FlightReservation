using FlightReservationConsole.Models;

namespace FlightReservationConsole.Services.Interfaces
{
    public interface IResponseService
    {
        string WriteResponse(CheapestFlight cheapestFlight);
    }
}
