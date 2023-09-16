using FlightReservationConsole.Services.Implementation;
using System.Configuration;
using System.Threading.Tasks;

namespace FlightReservationConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var chromiumPath = ConfigurationManager.AppSettings["Path"];
            FlightReservationSearch flightReservationSearch = new(new FlightSearchService(chromiumPath), new ResponseService(), new UserInputService());
            await flightReservationSearch.RunAsync();
        }
    }
}