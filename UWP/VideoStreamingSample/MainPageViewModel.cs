using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.ViewGroups;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;

namespace VideoOS.Mobile.SDK.Samples.UWP
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private Connection _connection;
        private ViewGroupTree _selectedCamera;
        private bool _isConnceted;
        private IEnumerable<ViewGroupTree> _allCameras;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the server address.
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        public string ServerPort { get; set; }

        /// <summary>
        /// Gets or sets the name of the server user.
        /// </summary>
        public string ServerUserName { get; set; }

        /// <summary>
        /// Gets or sets the server password.
        /// </summary>
        public string ServerPassword { get; set; }
        
        /// <summary>
        /// Gets or sets the selected user type index
        /// </summary>
        public int UserTypeIndex { get; set; }

        private string _mediaPlayerState;

        /// <summary>
        /// Current state of media player
        /// </summary>
        public string MediaPlayerState
        {
            get { return _mediaPlayerState; }
            set
            {
                SetProperty(ref _mediaPlayerState, value);
            }
        }

        /// <summary>
        /// Get or set Media player.
        /// </summary>
        public MediaPlayerElement AppMediaPlayer { get; set; }

        /// <summary>
        /// Gets or sets the selected camera.
        /// </summary>
        public ViewGroupTree SelectedCamera
        {
            get
            {
                return _selectedCamera;
            }
            set
            {
                SetProperty(ref _selectedCamera, value);
                if (value != null)
                {
                    StopStream();
                    LoadLiveStream();
                }
            }
        }

        /// <summary>
        /// Gets or sets all cameras.
        /// </summary>
        public IEnumerable<ViewGroupTree> AllCameras
        {
            get
            {
                return _allCameras;
            }
            set
            {
                SetProperty(ref _allCameras, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected to mobile server.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has been connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get
            {
                return _isConnceted;
            }
            set
            {
                SetProperty(ref _isConnceted, value);
            }
        }


        /// <summary>
        /// Gets or sets the connect and load cameras command.
        /// </summary>
        public ICommand ConnectAndLoadCamerasCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel(MediaPlayerElement appPlayer)
        {
            AppMediaPlayer = appPlayer;

            // Initialize the Mobile SDK
            VideoOS.Mobile.SDK.Portable.Environment.Instance.Initialize();

            // Initialize commands
            ConnectAndLoadCamerasCommand = new RelayCommand(ConnectAndLoadCameras);
        }

        /// <summary>
        /// Connects and loads cameras.
        /// </summary>
        /// <param name="objData">The object data.</param>
        public void ConnectAndLoadCameras(object objData)
        {
            _connection?.Dispose();
            _connection = new Connection(ChannelTypes.HTTP, ServerAddress, uint.Parse(ServerPort))
            {
                CommandsQueueing = CommandsQueueing.PerCommandTask
            };

            var connectResponse = _connection.Connect(null, DefaultTimeout);
            if (connectResponse.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage("Could not connect.");
                return;
            }

            var userType = UserTypeIndex == 0 ? UserType.ActiveDirectory : UserType.Basic;
            var loginResponse = _connection.LogIn(ServerUserName, ServerPassword, ClientTypes.MobileClient, DefaultTimeout,
                userType, new LoginParams { SupportsAudioIn = true, SupportsAudioOut = true });
            if (loginResponse.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage("Could not login.");
                return;
            }

            _connection.RunHeartBeat = true;
            ViewGroupsHelper.SupportsAudio = true;

            var allCamerasViews = ViewGroupsHelper.GetAllCamerasViews(_connection.Views, DefaultTimeout);
            var cameras = new List<ViewGroupTree>();

            ProcessViewItem(allCamerasViews, cameras);

            AllCameras = cameras.ToList();
            IsConnected = true;

            if (!AllCameras.Any())
            {
                ShowErrorMessage("There are no cameras");
            }
        }

        /// <summary>
        /// Called when a property is changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="storage">The storage.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void LoadLiveStream()
        {
            var streamingParams = GetRequestStreamParams(StreamParamsHelper.SignalType.Live, SelectedCamera.CameraId);

            var response = _connection.Video.RequestStream(streamingParams, null, DefaultTimeout);

            if (response.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage($"Incorrect request: Error code '{response.ErrorCode}'");
                return;
            }

            AppMediaPlayer.MediaPlayer.MediaFailed += (mediaPlayer, eventArgs) =>
            {
                CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MediaPlayerState = "Could not start direct streaming";
                });
            };
            AppMediaPlayer.MediaPlayer.PlaybackSession.PlaybackStateChanged += (playbackSession, eventArgs) =>
            {
                CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MediaPlayerState = playbackSession.PlaybackState.ToString();
                });
            };

            AppMediaPlayer.Source = MediaSource.CreateFromUri(new Uri($@"http://{_connection.IpAddress}:{_connection.Port}/{_connection.VideoAlias}/{response.VideoId}"));

        }

        private void StopStream()
        {
        }

        private static async void ShowErrorMessage(string errorMessage)
        {
            var errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = errorMessage,
                PrimaryButtonText = "Ok"
            };

            await errorDialog.ShowAsync();
        }

        private static void ProcessViewItem(ViewGroupTree item, List<ViewGroupTree> list)
        {
            if (item.ItemType == ViewItemType.Camera)
            {
                list.Add(item);
            }

            foreach (var subItem in item.GetMembersList())
            {
                ProcessViewItem((ViewGroupTree)subItem, list);
            }
        }

        private static VideoParams GetRequestStreamParams(StreamParamsHelper.SignalType signalType, Guid cameraId)
        {
            var videoParams = new VideoParams()
            {
                ItemId = cameraId,
                MethodType = StreamParamsHelper.MethodType.Push,
                SignalType = StreamParamsHelper.SignalType.Live,
                StreamType = StreamParamsHelper.StreamType.FragmentedMP4,
                FragmentDuration = TimeSpan.FromMilliseconds(-1000),
                DestWidth = 860,
                DestHeight = 640,
                FPS = 15,
                CompressionLvl = 71,
                StreamHeaders = StreamParamsHelper.StreamHeaders.NoHeaders,
            };

            return videoParams;
        }
    }
}