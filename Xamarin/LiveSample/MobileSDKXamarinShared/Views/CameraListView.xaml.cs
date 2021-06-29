using VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraListView
    {
        public CameraListView(Connection connection)
        {
            InitializeComponent();

            BindingContext = new CameraListViewModel(connection);
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (CamerasList.SelectedItem != null)
            {
                var selectedCamera = (ViewGroupTree)CamerasList.SelectedItem;
                if (selectedCamera != null)
                {
                    ((CameraListViewModel)BindingContext).GoToLive(selectedCamera.CameraId, selectedCamera.ItemName);
                }

                CamerasList.SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((CameraListViewModel)BindingContext).LoadData();
        }
    }
}