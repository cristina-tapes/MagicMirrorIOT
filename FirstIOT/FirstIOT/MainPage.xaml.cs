using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FirstIOT
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
