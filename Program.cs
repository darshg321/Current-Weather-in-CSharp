using Newtonsoft.Json;
using System.Net;

namespace Current_Weather
{
    class Program
    {
        static void Main()
        {
            Console.Write("Input US zipcode: ");
            string zipChoice = Console.ReadLine();

            if (zipChoice.ToString().Length >= 5)
            {
                string weatherData = GetData(zipChoice);

                if (weatherData == "Invalid zipcode")
                {
                    InvalidZipcode();
                }

                else
                {
                    dynamic readableData = JsonConvert.DeserializeObject(weatherData);
                    Console.WriteLine("");
                    Console.WriteLine($"Coordinates: {readableData.coord.lon} {readableData.coord.lat}");
                    Console.WriteLine($"Weather: {readableData.weather[0].main}");
                    Console.WriteLine($"Temperature: {readableData.main.temp}");
                    Console.WriteLine($"Pressure: {readableData.main.pressure}");
                    Console.WriteLine($"Humidity: {readableData.main.humidity}");
                    Console.WriteLine($"Visibility: {readableData.main.visibility}");
                    Console.WriteLine($"Wind Speed: {readableData.wind.speed}");
                    Console.WriteLine($"Location Name: {readableData.name}");
                    Console.WriteLine("Use Again? (Y/N)");
                    Console.WriteLine("");

                    var useAgain = Console.ReadKey();

                    if (useAgain.Key == ConsoleKey.Y)
                    {
                        Console.Clear();
                        Main();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                InvalidZipcode();
            }

            static string GetData(string zipChoice)
            {
                try
                {
                    var url = $"https://api.openweathermap.org/data/2.5/weather?zip={zipChoice},US&appid=a3a04c7371bdac851f2d6fbaa5235159";

                    var request = WebRequest.Create(url);
                    request.Method = "GET";

                    using var webResponse = request.GetResponse();
                    using var webStream = webResponse.GetResponseStream();

                    using var reader = new StreamReader(webStream);
                    var data = reader.ReadToEnd();
                    return data;
                }
                catch (WebException ex) when (ex.Message.Contains("404"))
                {
                    return "Invalid zipcode";
                }
            }
        }

        private static void InvalidZipcode()
        {
            Console.Clear();
            Console.WriteLine("Invalid zipcode");
            Console.WriteLine(" ");
            Main();
        }
    }
}