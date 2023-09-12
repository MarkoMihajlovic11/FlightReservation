using FlightReservationConsole.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightReservationConsole.Services.Implementation
{
    public class UserInputService
    {
        public static FlightReservation GetFlightReservation()
        {
            FlightReservation flightReservation = new();

            //Insert flight from-to locations
            Console.Write("Flight from(ex belgrade-serbia): ");
            flightReservation.FlightFrom = Console.ReadLine();
            Console.Write("Flight to(ex. barcelona-spain): ");
            flightReservation.FlightTo = Console.ReadLine();

            //Insert dates 
            Console.WriteLine("Between dates:");
            Console.Write("From(yyyy-MM-dd): ");
            flightReservation.DateFrom = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Console.Write("To(yyyy-MM-dd):");
            flightReservation.DateTo = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            //Insert how long to stay
            Console.Write("You don't want to stay less than(nights): ");
            flightReservation.LessThanDays = int.Parse(Console.ReadLine());
            Console.Write("You don't want to stay more than(nights): ");
            flightReservation.MoreThanDays = int.Parse(Console.ReadLine());

            return flightReservation;
        }
    }

    public class ResponseService
    {
        public static SearchResponse GetSearchResponse(CheapestFlight cheapestFlight)
        {
            // Define your response logic here based on the given criteria.
            // For this example, we'll assume that if the price is below a certain threshold,
            // we stop searching; otherwise, we continue searching.

            // You can adjust this logic based on your specific requirements.

            decimal priceThreshold = 300; // Set your desired price threshold here.

            if (decimal.TryParse(cheapestFlight.Price.Replace("$", ""), out decimal price))
            {
                if (price <= priceThreshold)
                {
                    return SearchResponse.StopSearch; // Stop searching if the price is below or equal to the threshold.
                }
            }

            return SearchResponse.ContinueSearch; // Continue searching if the price is above the threshold.
        }
    }

    public enum SearchResponse
    {
        ContinueSearch, // Indicates that the search should continue.
        StopSearch      // Indicates that the search should be stopped.
    }

}
