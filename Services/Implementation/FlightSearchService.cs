using FlightReservationConsole.Models;
using FlightReservationConsole.Puppeteer;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightReservationConsole.Services.Implementation
{
    public class FlightSearchService //: IFlightSearchService
    {
        private readonly string _path;
        private string _html;
        private readonly string _mainUrl = "https://www.kiwi.com";

        public FlightSearchService(string path)
        {
            _path = path;
        }

        public async Task<CheapestFlight> FindCheapestFlight(FlightReservation flightReservation)
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

                while (true)
                {
                    var cheapestFlight = await FindCheapestPriceAndBookingUrl(page);
                    int nights = int.Parse(cheapestFlight.BookingUrl.Split("ResultCardItinerarystyled__SectorLayoverTextBackground-sc-iwhyue-9 cJMqrQ\">")[1]
                        .Split("nights")[0].Trim());

                    if (nights >= flightReservation.LessThanDays && nights <= flightReservation.MoreThanDays)
                        return cheapestFlight;

                    await ScrollPage(page);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There is no cheap flights for that dates", ex);
            }
        }

        private async Task GetPageContent(string url, Page page)
        {
            await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);
            var button = await page.QuerySelectorAsync("button");
            await button.ClickAsync();
            Thread.Sleep(10000);
            _html = await page.GetContentAsync();
        }

        private async Task ScrollPage(Page page)
        {
            await page.EvaluateExpressionAsync("window.scrollBy(1, window.innerHeight)");
            await Task.Delay(5000); // Use Task.Delay instead of Thread.Sleep
           // await page.GetContentAsync();
        }

        private async Task<CheapestFlight> FindCheapestPriceAndBookingUrl(Page page)
        {
            var url = "";
            while (true)
            {
                try
                {
                    var pageContent = await page.GetContentAsync();
                    url = _html.Split("<a class=\"ButtonPrimitive__StyledButtonPrimitive-sc-1lbd19y-0 kBsuLf\"")[1]
                        .Split("rel=\"nofollow\"")[0]
                        .Replace("\"", "")
                        .Replace("href=", "")
                        .Replace(";", "&")
                        .Trim();

                    url = $"{_mainUrl}{url}";

                    var price = _html.Split("<span class=\" length-10\">")[1]
                        .Split("</span>")[0];

                    return new CheapestFlight { Price = price, BookingUrl = url };
                }
                catch (Exception)
                {
                    await ScrollPage(page);
                }
            }
        }
    }
}
