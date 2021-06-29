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
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace VideoOS.Mobile.SDK.Samples.UWP
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private Connection _connection;
        private RelatedViewGroupItem _selectedCamera;
        private bool _isAudioPlaying;
        private bool _isConnceted;
        private IEnumerable<RelatedViewGroupItem> _allCameras;

        private MediaPlayer _mediaPlayer = null;

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
        /// Gets or sets the selected camera.
        /// </summary>
        public RelatedViewGroupItem SelectedCamera
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
                    StopAudio();
                    LoadLiveAudio();
                }
            }
        }

        /// <summary>
        /// Gets or sets all cameras.
        /// </summary>
        public IEnumerable<RelatedViewGroupItem> AllCameras
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
        /// Gets or sets a value indicating whether audio player is provided with source url.
        /// </summary>
        public bool IsAudioPlaying
        {
            get
            {
                return _isAudioPlaying;
            }
            set
            {
                SetProperty(ref _isAudioPlaying, value);
            }
        }

        /// <summary>
        /// Gets or sets the connect and load microphones command.
        /// </summary>
        public ICommand ConnectAndLoadCamerasWithMicrophonesCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            // Initialize the Mobile SDK
            Phone.Environment.Instance.Initialze();

            // Initialize commands
            ConnectAndLoadCamerasWithMicrophonesCommand = new RelayCommand(ConnectAndLoadCamerasWithMicrophones);
        }

        /// <summary>
        /// Connects and loads cameras.
        /// </summary>
        /// <param name="objData">The object data.</param>
        public void ConnectAndLoadCamerasWithMicrophones(object objData)
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

            var loginResponse = _connection.LogIn(ServerUserName, ServerPassword, ClientTypes.MobileClient, DefaultTimeout,
                UserType.Unknown, new LoginParams { SupportsAudioIn = true, SupportsAudioOut = true });
            if (loginResponse.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage("Could not login.");
                return;
            }

            _connection.RunHeartBeat = true;
            ViewGroupsHelper.SupportsAudio = true;

            var allCamerasViews = ViewGroupsHelper.GetAllCamerasViews(_connection.Views, DefaultTimeout);
            var cameras = new List<RelatedViewGroupItem>();

            ProcessViewItem(allCamerasViews, cameras);

            AllCameras = cameras.Where(c => c.RelatedMicrophone != Guid.Empty).ToList();
            IsConnected = true;

            if (!AllCameras.Any())
            {
                ShowErrorMessage("There are no cameras with related microphones");
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

        private void LoadLiveAudio()
        {
            var audioParams = GetRequestStreamParams(StreamParamsHelper.SignalType.Live, SelectedCamera.RelatedMicrophone);

            var response = _connection.Audio.RequestAudioStream(audioParams, null, DefaultTimeout);

            if (response.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage(response.ErrorCode == ErrorCodes.InsufficientUserRights ? "Insufficient Rights." : "Microphone Not Available.");
                return;
            }

    

            _mediaPlayer = BackgroundMediaPlayer.Current;
            _mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri($@"http://{_connection.IpAddress}:{_connection.Port}/{_connection.AudioAlias}/{response.VideoId}"));
            _mediaPlayer.Play();

            IsAudioPlaying = true;
     

        }

        private void StopAudio()
        {
     
            BackgroundMediaPlayer.Shutdown();
            _mediaPlayer = null;
            IsAudioPlaying = false;
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

        private static void ProcessViewItem(ViewGroupTree item, List<RelatedViewGroupItem> list)
        {
            if (item.ItemType == ViewItemType.Camera)
            {
                //check if there are any related microphones to this camera if so, add them to the list
                List<IViewGroupTree> relatedItems = item.GetMembersList();
                var mic = relatedItems.SingleOrDefault(m => (m as ViewGroupTree).ItemType == ViewItemType.Microphone);
                var speaker = relatedItems.SingleOrDefault(m => (m as ViewGroupTree).ItemType == ViewItemType.Speaker);
                RelatedViewGroupItem itemWithRelatedItems = new RelatedViewGroupItem
                {
                    CameraItem = item,
                    RelatedMicrophone = (mic as ViewGroupTree)?.ItemId ?? Guid.Empty,
                    RelatedSpeaker = (speaker as ViewGroupTree)?.ItemId ?? Guid.Empty
                };
                list.Add(itemWithRelatedItems);
            }

            foreach (var subItem in item.GetMembersList())
            {
                ProcessViewItem((ViewGroupTree)subItem, list);
            }
        }

        private static AudioParams GetRequestStreamParams(StreamParamsHelper.SignalType signalType, Guid microphoneId)
        {
            var audioParams = new AudioParams()
            {
                ItemId = microphoneId,
                CompressionLvl = 99,
                AudioEncoding = StreamParamsHelper.AudioEncoding.Mp3,
                SignalType = signalType,
                MethodType = StreamParamsHelper.MethodType.Push,
                StreamHeaders = StreamParamsHelper.StreamHeaders.NoHeaders,
            };

            return audioParams;
        }
    }

    public class RelatedViewGroupItem
    {
        public ViewGroupTree CameraItem { get; set; }
        public Guid RelatedMicrophone { get; set; }
        public Guid RelatedSpeaker { get; set; }
    }
}