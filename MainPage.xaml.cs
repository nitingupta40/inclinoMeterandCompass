using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Sensors;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace inclinometer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Inclinometer _inclinometer;
        private Compass _compass;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        private async void ReadingChanged(object sender, InclinometerReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InclinometerReading reading = e.Reading;
                _x_tbx_x.Text = String.Format("{0,5:0.00}", reading.PitchDegrees);
                _x_tbx_y.Text = String.Format("{0,5:0.00}", reading.RollDegrees);
                _x_tbx_z.Text = String.Format("{0,5:0.00}", reading.YawDegrees);
            });
        }


        private async void ReadingChangedc(object sender, CompassReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 CompassReading readingc = e.Reading;
                 double _magnetic_north = readingc.HeadingMagneticNorth;
                 Double _true_north = Convert.ToDouble(readingc.HeadingTrueNorth);
                 _x_tbx_yc.Text = String.Format("{0,5:0.00}", _magnetic_north);
                 _x_tbx_zc.Text = String.Format("{0,5:0.00}", _true_north);
                 if ((Convert.ToInt32(_magnetic_north) == 0) || (Convert.ToInt32(_true_north) == 0))
                 {
                     _x_tbk_direction.Text = "North";
                 }
                 else if ((Convert.ToInt32(_magnetic_north) == 90) || (Convert.ToInt32(_true_north) == 90))
                 {
                     _x_tbk_direction.Text = "East";
                 }
                 else if ((Convert.ToInt32(_magnetic_north) == 180) || (Convert.ToInt32(_true_north) == 180))
                 {
                     _x_tbk_direction.Text = "South";
                 }
                 else if ((Convert.ToInt32(_magnetic_north) == 270) || (Convert.ToInt32(_true_north) == 270))
                 {
                     _x_tbk_direction.Text = "West";
                 }

             });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            _inclinometer = Inclinometer.GetDefault();
            if(_inclinometer != null)
            {
                // Establish the report interval for all scenarios
                uint minReportInterval = _inclinometer.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _inclinometer.ReportInterval = reportInterval;

                // Establish the event handler
                _inclinometer.ReadingChanged += new TypedEventHandler<Inclinometer, InclinometerReadingChangedEventArgs>(ReadingChanged);
            }
            _compass = Compass.GetDefault();
            if (_compass != null)
            {
                uint _minReportInterval = _compass.MinimumReportInterval;
                uint _creportInterval = _minReportInterval > 16 ? _minReportInterval : 16;
                _compass.ReportInterval = _creportInterval;
                _compass.ReadingChanged += new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChangedc);

            }

        }
    }
}
