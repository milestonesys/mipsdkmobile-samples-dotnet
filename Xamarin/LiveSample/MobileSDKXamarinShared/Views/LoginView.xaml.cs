using System;
using VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
            
            ToolbarItems.Add(new ToolbarItem("About", "about", About));
        }

        private void OnLogin(object sender, EventArgs e)
        {
            ((LoginViewModel)BindingContext).ConnectAndLogin();
        }

        private void About()
        {
            DisplayAlert("About", "Milestone SDK Live Video Sample." + System.Environment.NewLine + "For more information visit www.milestonesys.com.", "OK");
        }
    }
}