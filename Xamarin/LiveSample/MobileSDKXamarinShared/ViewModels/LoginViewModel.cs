using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Samples.Xamarin.Views;
using Xamarin.Forms;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    /// <seealso cref="VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels.BaseViewModel" />
    public class LoginViewModel : BaseViewModel
    {
        private string _address;
        private uint _port;
        private string _username;
        private string _password;
        private int _selectedUserTypeIndex;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public uint Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected user type index
        /// </summary>
        public int SelectedUserTypeIndex
        {
            get
            {
                return _selectedUserTypeIndex;
            }
            set
            {
                if (_selectedUserTypeIndex != value)
                {
                    _selectedUserTypeIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel()
        {
            Port = 8081;
        }
        
        /// <summary>
        /// Initializes the connection.
        /// </summary>
        public async void ConnectAndLogin()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            if (Connection != null)
            {
                Connection.Dispose();
            }

            Connection = new Connection(ChannelTypes.HTTP, Address, Port)
            {
                CommandsQueueing = CommandsQueueing.PerCommandTask
            };

            var response = await Connection.ConnectAsync(null, App.MaxTimeout);

            if (response == null || response.ErrorCode != ErrorCodes.Ok)
            {
                CloseConnection();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = false;
                    await DisplayMessage("Error", "Unable to connect", "Close");
                });

                return;
            }

            var userType = SelectedUserTypeIndex == 0 ? UserType.ActiveDirectory : UserType.Basic;
            Connection.LogIn(Username, Password, ClientTypes.MobileClient, userType, null, OnLoginResponseSuccess, OnLoginResponseFail);
        }

        private void OnLoginResponseSuccess(BaseCommandResponse response)
        {
            Connection.RunHeartBeat = true;
            Connection.Start();

            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = false;
                var cameraListView = new CameraListView(Connection);

                await Application.Current.MainPage.Navigation.PushAsync(cameraListView);
            });
        }

        private void OnLoginResponseFail(BaseCommandResponse response)
        {
            CloseConnection();

            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = false;
                await DisplayMessage("Error", "Unable to login", "Close");
            });
        }

        private void CloseConnection()
        {
            Connection.Dispose();
            Connection = null;
        }
    }
}