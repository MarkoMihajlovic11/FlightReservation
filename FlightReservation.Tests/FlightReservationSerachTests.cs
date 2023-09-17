using AutoFixture;
using FlightReservationConsole;
using FlightReservationConsole.Models;
using FlightReservationConsole.Services.Interfaces;
using NSubstitute;
using Xunit;

namespace FlightReservation.Tests
{
    public class FlightReservationSerachTests
    {
        private readonly FlightReservationSearch _sut;
        private readonly IUserInputService _userInputService = Substitute.For<IUserInputService>();
        private readonly IFlightSearchService _flightSearchService = Substitute.For<IFlightSearchService>();
        private readonly IResponseService _responseService = Substitute.For<IResponseService>();
        private readonly IFixture _fixture = new Fixture();

        public FlightReservationSerachTests()
        {
            _sut = new FlightReservationSearch(_flightSearchService, _responseService, _userInputService);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnValidResponse_WhenAllParametersAreGood()
        {
            //Arrange
            FlightReservationModel flightReservation = new()
            {
                DateFrom = DateTime.UtcNow.AddDays(10),
                DateTo = DateTime.UtcNow.AddDays(15),
                LessThanDays = 1,
                MoreThanDays = 2
            };

            _userInputService.GetFlightReservation().Returns(flightReservation);

            var cheapestFlight = _fixture.Create<CheapestFlight>();
            _flightSearchService.FindCheapestFlight(flightReservation)
                .Returns(cheapestFlight);

            _responseService.WriteResponse(cheapestFlight)
                .Returns($"Cheapest flight {cheapestFlight.Price} {Environment.NewLine}Can be booked on url: {cheapestFlight.BookingUrl}");

            //Act
            var result = await _sut.RunAsync();

            //Assert
            Assert.Equal($"Cheapest flight {cheapestFlight.Price} {Environment.NewLine}Can be booked on url: {cheapestFlight.BookingUrl}", result);
            await _flightSearchService.Received(1).FindCheapestFlight(flightReservation);
        }
    }
}