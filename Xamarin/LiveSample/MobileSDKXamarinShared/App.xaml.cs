using System;
using Xamarin.Forms;

namespace VideoOS.Mobile.SDK.Samples.Xamarin
{
    public partial class App
    {
        public static TimeSpan MaxTimeout = new TimeSpan(0, 0, 10);

        public static int ScreenWidth;
        public static int ScreenHeight;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.LoginView());
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}