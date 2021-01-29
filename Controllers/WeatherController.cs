using Newtonsoft.Json;
using SampleDBConn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace WebApplication5.Controllers
{
    public class WeatherController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> City(string city)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=4e10532cbf8d78b102818e29b2cb93a5&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<RootObject>(stringResult);
                    var test = (new
                    {
                        Temp = string.Join("", rawWeather.weather.Select(x => x.description)),
                        Summary = string.Join(",", rawWeather.weather.Select(x => x.main)),
                        City = rawWeather.name,
                        Country = rawWeather.sys.country
                    });
                    ViewBag.Temp = test.Temp;
                    ViewBag.Summary = test.Summary;
                    ViewBag.City = test.City;
                    ViewBag.Country = test.Country;
                    return View();

                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }

        private ActionResult BadRequest(string v)
        {
            throw new NotImplementedException();
        }
    }
}


