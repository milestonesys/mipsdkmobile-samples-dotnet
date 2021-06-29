using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.ViewGroups;
using Xamarin.Forms;
using VideoOS.Mobile.SDK.Samples.Xamarin.Views;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels
{
    /// <summary>
    /// Camera list view model.
    /// </summary>
    /// <seealso cref="VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels.BaseViewModel" />
    public class CameraListViewModel : BaseViewModel
    {
        private List<ViewGroupTree> _cameras;

        /// <summary>
        /// Gets or sets the cameras.
        /// </summary>
        public List<ViewGroupTree> Cameras
        {
            get
            {
                return _cameras;
            }
            set
            {
                if (_cameras != value)
                {
                    _cameras = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraListViewModel"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public CameraListViewModel(Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Loads the view model data.
        /// </summary>
        public override void LoadData()
        {
            if (IsBusy || GetConnectionState() != ConnectionStates.LoggedIn)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var allCamerasViews = ViewGroupsHelper.GetAllCamerasViews(Connection.Views, App.MaxTimeout);

                var flatList = new List<ViewGroupTree>();
                ProcessViewItem(allCamerasViews, flatList);
                Cameras = flatList;
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayMessage("Error", "Unable to retrieve camera list", "Close");
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Navigates to live view.
        /// </summary>
        /// <param name="cameraId">The camera identifier.</param>
        /// <param name="cameraName">Name of the camera.</param>
        public void GoToLive(Guid cameraId, string cameraName)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var liveView = new LiveView(Connection, cameraId, cameraName);

                await Application.Current.MainPage.Navigation.PushAsync(liveView);
            });
        }

        private void ProcessViewItem(ViewGroupTree item, List<ViewGroupTree> flatList)
        {
            if (item.ItemType == ViewItemType.Camera)
            {
                flatList.Add(item);
            }

            foreach (var subItem in item.GetMembersList())
            {
                ProcessViewItem((ViewGroupTree)subItem, flatList);
            }
        }
    }
}