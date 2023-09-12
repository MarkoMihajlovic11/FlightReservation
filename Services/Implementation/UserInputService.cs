using FlightReservationConsole.Models;
using System;
using System.Globalization;

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
}
