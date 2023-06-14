using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NP6hw
{
    internal static class Weather
    {
        public static string Temperature { get; set; } = string.Empty;//строка(св-во) температуры;
        public static string Description { get; set; } = string.Empty;//строка(св-во) описания;
        public static string Icon { get; set; } = string.Empty;//строка(св-во) адреса картинки;
        public static async Task GetDataFromOpenWeatherMap()//метод для получения информации о погоде с сайта openweathermap.org
        {
            using (HttpClient client = new())
            {
                string url = "https://api.openweathermap.org/data/2.5/weather?q=moscow,ru&appid=17f74e0477580d172ff3511a555e801e&units=metric&lang=ru";
                //строка адреса openweathermap.org, где:
                // - weather?q={city},{country_code} - запрос информации о погоде в городе {city}, стране {country_code};
                // - appid - ключ API;
                // - units=metric - система измерения, в данном случае метрическая;
                // - lang=ru - язык ответа, св-во description возвращается на русском языке;
                HttpRequestMessage request = new(HttpMethod.Get, url);//сообщение о запросе;
                HttpResponseMessage response = await client.SendAsync(request);//отправка запроса и получение его в ответ;
                response.EnsureSuccessStatusCode();//убедиться в успешном получении ответа;
                string body = await response.Content.ReadAsStringAsync();//перевод ответа в строку;
                dynamic weather = JsonConvert.DeserializeObject(body);//десериализация полученного JSON;
                //В случае, если ответ получен успешно, придет следующий ответ в формате JSON;
                //                {
                //                    "coord": {
                //                        "lon": 10.99,
                //    "lat": 44.34
                //                    },
                //  "weather": [
                //    {
                //                        "id": 501,
                //      "main": "Rain",
                //      "description": "moderate rain",
                //      "icon": "10d"
                //    }
                //  ],
                //  "base": "stations",
                //  "main": {
                //                        "temp": 298.48,
                //    "feels_like": 298.74,
                //    "temp_min": 297.56,
                //    "temp_max": 300.05,
                //    "pressure": 1015,
                //    "humidity": 64,
                //    "sea_level": 1015,
                //    "grnd_level": 933
                //  },
                //  "visibility": 10000,
                //  "wind": {
                //                        "speed": 0.62,
                //    "deg": 349,
                //    "gust": 1.18
                //  },
                //  "rain": {
                //                        "1h": 3.16
                //  },
                //  "clouds": {
                //                        "all": 100
                //  },
                //  "dt": 1661870592,
                //  "sys": {
                //                        "type": 2,
                //    "id": 2075663,
                //    "country": "IT",
                //    "sunrise": 1661834187,
                //    "sunset": 1661882248
                //  },
                //  "timezone": 7200,
                //  "id": 3163858,
                //  "name": "Zocca",
                //  "cod": 200
                //}
                //После успешной десериализации в св-ва данного класса записываются:
                if (weather != null)
                {
                    //температура
                    double temperature = weather.main.temp;
                    Temperature = temperature >= 0 ? "+" + temperature + "C" : temperature + "C";//добавление "+" если температура выше нуля;
                    //описание:
                    string description = weather.weather[0].description;
                    Description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(description);//заглавность;
                    Icon = @"icons\" + weather.weather[0].icon + ".png";//картинка, ее директория;
                }
            }
        }
    }
}
