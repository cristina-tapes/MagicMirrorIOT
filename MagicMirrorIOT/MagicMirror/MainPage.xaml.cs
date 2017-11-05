using System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

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
            }, TimeSpan.FromMilliseconds(1000));
        }

        private void UpdateTime()
        {
            var currentRegionTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
            this.time.Text = currentRegionTime.ToString("hh:mm tt");
            this.date.Text = currentRegionTime.ToString("dddd d MMMM");
            this.year.Text = currentRegionTime.ToString("yyyy");
        }

        private void UpdateWeather() { }
        
    }
}
