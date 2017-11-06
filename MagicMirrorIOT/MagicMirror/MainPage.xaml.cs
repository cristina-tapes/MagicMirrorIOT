using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MagicMirror
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ThreadPoolTimer _weatherTimer = null;
        ThreadPoolTimer _clockTimer = null;

        public MainPage()
        {
            this.InitializeComponent();
            UpdateTime();
            UpdateWeather();
            _clockTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        UpdateTime();
                    });
            }, TimeSpan.FromMilliseconds(1000));

            _weatherTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        UpdateWeather();
                    });
            }, TimeSpan.FromHours(1));
        }

        private void UpdateTime()
        {
            var currentRegionTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
            this.time.Text = currentRegionTime.ToString("hh:mm tt");
            this.date.Text = currentRegionTime.ToString("dddd d MMMM");
            this.year.Text = currentRegionTime.ToString("yyyy");
        }

        private async void UpdateWeather()
        {
            WebRequest request = WebRequest.Create(@"http://dataservice.accuweather.com/currentconditions/v1/287713?apikey=WSLi9zcgj8ag9qhEc6Zgd05YpN3jR5oA&details=true&_=1509913114456");
            request.Method = "GET";
            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                Stream stream = response.GetResponseStream();
                var result = new StreamReader(stream).ReadToEnd();
                var currentConditions = JsonConvert.DeserializeObject<List<CurrentCondition>>(result);
                this.textBlock.Text = Math.Round(currentConditions[0].Temperature.Metric.Value) + "\u2103";
                var tempUri = @"ms-appx:///WeatherIcons/" + currentConditions[0].WeatherIcon + ".png";
                this.imageWeatherCurrentConditions.Source = new BitmapImage(new Uri(tempUri, UriKind.Absolute));
            }
        }

    }
}
