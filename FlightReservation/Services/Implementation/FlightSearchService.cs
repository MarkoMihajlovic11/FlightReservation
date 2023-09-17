using FlightReservationConsole.Models;
using FlightReservationConsole.Puppeteer;
using FlightReservationConsole.Services.Interfaces;
using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightReservationConsole.Services.Implementation
{
    public class FlightSearchService : IFlightSearchService
    {
        private readonly string _path;
        private readonly string _mainUrl = "https://www.kiwi.com";

        public FlightSearchService(string path)
        {
            _path = path;
        }

        public async Task<CheapestFlight> FindCheapestFlight(FlightReservationModel flightReservation)
        {
            try
            {
                var mainUrl = "https://www.kiwi.com";
                var fromFlyBack = flightReservation.DateFrom.AddDays(-flightReservation.LessThanDays);
                var toArrive = flightReservation.DateTo.AddDays(-flightReservation.MoreThanDays);
                var url = $"{mainUrl}/en/search/results/{flightReservation.FlightFrom}/{flightReservation.FlightTo}" +
                          $"/{flightReservation.DateFrom:yyyy-MM-dd}_{toArrive:yyyy-MM-dd}/{fromFlyBack:yyyyMMdd}_{flightReservation.DateTo:yyyy-MM-dd}?sortBy=price";

                var puppeteer = new Downloader(_path, false);

                using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(puppeteer.SetBrowserOptions());
                using var page = (await browser.PagesAsync())[0];

                await GetPageContent(url, page);
                CheapestFlight cheapestFlight = new();
                int flightNumber = 0;
                while (true)
                {
                    var isFinded = await FindCheapestPriceAndBookingUrl(flightReservation, cheapestFlight, page, flightNumber);

                    if (isFinded)
                        return cheapestFlight;

                    flightNumber++;
                    await ScrollPage(page);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't find any flight for those dates and location. Please check input parameters");
            }
        }

        private async Task GetPageContent(string url, Page page)
        {
            await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);
            var button = await page.QuerySelectorAsync("button");
            await button.ClickAsync();
            Thread.Sleep(10000);
        }

        private async Task ScrollPage(Page page)
        {
            await page.EvaluateExpressionAsync("window.scrollBy(1, window.innerHeight)");
            await Task.Delay(5000); // Use Task.Delay instead of Thread.Sleep
                                    // await page.GetContentAsync();
        }

        private async Task<bool> FindCheapestPriceAndBookingUrl(FlightReservationModel flightReservation, CheapestFlight cheapestFlight, Page page, int flightNumber)
        {
            var url = "";
            while (true)
            {
                var pageContent = await page.GetContentAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(pageContent);

                var nightsNode = doc.DocumentNode.SelectNodes("//div[@class='bg-white-normal px-sm']")[flightNumber];
                var nights = int.Parse
                    (
                    nightsNode.InnerHtml
                    .Split(" ")
                    .FirstOrDefault()
                    );

                if (nights >= flightReservation.LessThanDays && nights <= flightReservation.MoreThanDays)
                {
                    var bookingUrlNode = doc.DocumentNode.SelectNodes("//a[@class='ButtonPrimitive__StyledButtonPrimitive-sc-j8pavp-0 kyEVVq']")[flightNumber];
                    var bookingUrl = bookingUrlNode.GetAttributeValue("href", "");
                    url = $"{_mainUrl}{url}";
                    cheapestFlight.BookingUrl = url;


                    var priceNode = doc.DocumentNode.SelectNodes("//span[contains(@class, 'length')]")[flightNumber];
                    var price = priceNode.InnerHtml
                        .Replace("&nbsp;", "");

                    cheapestFlight.Price = price;

                    return true;
                }
                else
                {
                    return false;
                }


            }
        }
    }
}
