using System.Net;
using Xamarin.Forms;

namespace SimplifyHue.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

#if DEBUG
            HotReloader.Current.Run(this, new HotReloader.Configuration
            {
                ExtensionIpAddress = IPAddress.Parse("192.168.178.71")
            });
#endif

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
