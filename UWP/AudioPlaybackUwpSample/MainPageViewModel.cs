using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.Audio;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.ViewGroups;

namespace AudioPlaybackUwpSample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
       public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        public static Size StreamSize = new Size(640, 480);
        public static StreamParamsHelper.MethodType DefaultMethodType = StreamParamsHelper.MethodType.Push;

        private Connection _connection;
        private PlaybackVideo _video;
        private RelatedViewGroupItem _selectedCamera;
        private bool _isVideoLoaded;
        private bool _selectedCameraHasRelatedMicrophone;
        private bool _isConnceted;
        private IEnumerable<RelatedViewGroupItem> _allCameras;
        private BitmapImage _imageViewSource;
        private bool _isAudioMuted = true;
        private string _audioIconPath;
        private bool _audioButtonEnabled;
        private MediaPlayer _mediaPlayer = null;
        private readonly string _mutedIconPath = @"Resources/volume-off.png";
        private readonly string _unmutedIconPath = @"Resources/volume-high.png";

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
                SelectedCameraHasRelatedMicrophone = _selectedCamera.RelatedMicrophone != Guid.Empty;
                _video?.Stop();
                _video?.Dispose();
                _video = null;
                if (value != null )
                {
                    StopAudio();
                    LoadPlaybackVideo();
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
        /// Gets or sets the image view source.
        /// </summary>
        public BitmapImage ImageViewSource
        {
            get { return _imageViewSource; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _imageViewSource, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether for this instance the video is loaded.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has loaded video; otherwise, <c>false</c>.
        /// </value>
        public bool IsVideoLoaded
        {
            get
            {
                return _isVideoLoaded;
            }
            set
            {
                SetProperty(ref _isVideoLoaded, value);
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
        /// Gets or sets a value indicating whether this camera item has a microphone associated with it.
        /// </summary>
        /// <value>
        /// <c>true</c> if this camera item has related microphone; otherwise, <c>false</c>.
        /// </value>
        public bool SelectedCameraHasRelatedMicrophone
        {
            get { return _selectedCameraHasRelatedMicrophone; }
            set { SetProperty(ref _selectedCameraHasRelatedMicrophone, value); }
        }


       /// <summary>
        /// Gets or sets the connect and load cameras command.
        /// </summary>
        public ICommand ConnectAndLoadCamerasCommand { get; set; }

        /// <summary>
        /// Gets or sets the play backwards command.
        /// </summary>
        public ICommand PlayBackwardsCommand { get; set; }

        /// <summary>
        /// Gets or sets the play forward command.
        /// </summary>
        public ICommand PlayForwardCommand { get; set; }

        /// <summary>
        /// Gets or sets the pause video command.
        /// </summary>
        public ICommand PauseVideoCommand { get; set; }

        /// <summary>
        /// Mutes or Umnutes the audio player.
        /// </summary>
        public ICommand MuteUnmuteAudioCommand { get; set; }

        public string AudioIconPath
        {
            get { return _audioIconPath; }
            set
            {
                SetProperty(ref _audioIconPath , value);
            }
        }

        public bool AudioButtonEnabled
        {
            get { return _audioButtonEnabled; }
            set
            {
                SetProperty(ref _audioButtonEnabled , value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            // Initialize the Mobile SDK
            VideoOS.Mobile.SDK.Phone.Environment.Instance.Initialze();

            // Initialize commands
            ConnectAndLoadCamerasCommand = new RelayCommand(ConnectAndLoadCameras);
            PlayBackwardsCommand = new RelayCommand(PlayBackwards);
            PlayForwardCommand = new RelayCommand(PlayForward);
            PauseVideoCommand = new RelayCommand(PauseVideo);
            MuteUnmuteAudioCommand = new RelayCommand(MuteUnmuteAudio);
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
            
            var loginResponse = _connection.LogIn(ServerUserName, ServerPassword, ClientTypes.MobileClient, DefaultTimeout,
                UserType.Unknown, new LoginParams {SupportsAudioIn = true, SupportsAudioOut = true});

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

            AllCameras = cameras;
            IsConnected = true;
            
        }

        /// <summary>
        /// Plays the video backwards.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void PlayBackwards(object obj)
        {
            _video?.PlaybackControl.ChangeSpeed(-1);
            StopAudio();
        }

        /// <summary>
        /// Plays the video forward.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void PlayForward(object obj)
        {
            _video?.PlaybackControl.ChangeSpeed(1);
            StartAudio();
            
        }
        
        /// <summary>
        /// Pauses the video.
        /// </summary>
        /// <param name="obj">The object.</param>
         public void PauseVideo(object obj)
        {
            _video?.PlaybackControl.ChangeSpeed(0);
            StopAudio();
        }

        /// <summary>
        /// Plays or stops the audio.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void MuteUnmuteAudio(object obj)
        {
            _isAudioMuted = !_isAudioMuted;
            UpdateAudioIcon(_isAudioMuted);
            if(_mediaPlayer != null)
                _mediaPlayer.IsMuted = _isAudioMuted;
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
        
        private void LoadPlaybackVideo()
        {
            var vParams = GetRequestVideoStreamParams(StreamSize, StreamParamsHelper.SignalType.Playback, SelectedCamera.CameraItem.CameraId);
            vParams.MethodType = DefaultMethodType;

            var playbackParams = new PlaybackParams
            {
                SeekType = PlaybackParamsHelper.SeekType.Time,

                //ask for video playback from a minute ago
                Time = TimeConverter.FromLong(TimeConverter.ToLong(DateTime.Now.AddMinutes(-1)))
            };

            
            var response = _connection.Video.RequestStream(vParams, playbackParams, DefaultTimeout);

            if (response.ErrorCode != ErrorCodes.Ok)
            {
                ImageViewSource = new BitmapImage();
                ShowErrorMessage(response.ErrorCode == ErrorCodes.InsufficientUserRights ? "Insufficient Rights." : "Camera Not Available.");
                return;
            }

            _video = _connection.VideoFactory.CreatePlaybackVideo(new RequestStreamResponsePlayback(response));

            if (vParams.MethodType == StreamParamsHelper.MethodType.Pull)
            {
                new VideoPullProxy(_video, _video, (int)vParams.FPS) { NewFrame = OnVideoFrame };
            }
            else
            {
                _video.NewFrame = OnVideoFrame;
            }

            _video.Start();

            IsVideoLoaded = true;
            UpdateAudioIcon(_isAudioMuted);
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
        
        private async void OnVideoFrame(VideoFrame frame)
        {
            if (frame?.Data != null && frame.Data.Length > 0)
            {
                var randomAccessStream = await ConvertTo(frame.Data);
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(randomAccessStream);
                        ImageViewSource = bitmapImage;
                    });
            }
        }

        private static async Task<InMemoryRandomAccessStream> ConvertTo(byte[] arr)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            await randomAccessStream.WriteAsync(arr.AsBuffer());
            randomAccessStream.Seek(0);
            return randomAccessStream;
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

        private static VideoParams GetRequestVideoStreamParams(Size streamSize, StreamParamsHelper.SignalType signalType, Guid cameraId)
        {
            var vParams = new VideoParams
            {
                SignalType = signalType,
                MethodType = DefaultMethodType,
                FPS = 8,
                CompressionLvl = 71,
                CameraId = cameraId,
                DestWidth = (int)streamSize.Width,
                DestHeight = (int)streamSize.Height,
                RequiresDownsampling = false
            };

            return vParams;
        }

        private static AudioParams GetRequestAudioStreamParams(StreamParamsHelper.SignalType signalType, Guid microphoneId)
        {
            var audioParams = new AudioParams()
            {
                ItemId = microphoneId,
                CompressionLvl = 100,
                AudioEncoding = StreamParamsHelper.AudioEncoding.Mp3,
                SignalType = signalType,
                MethodType = StreamParamsHelper.MethodType.Push,
                StreamHeaders = StreamParamsHelper.StreamHeaders.NoHeaders,
            };

            return audioParams;
        }

        private void UpdateAudioIcon(bool isAudioMuted)
        {
            AudioIconPath = isAudioMuted ? _mutedIconPath : _unmutedIconPath;
        }

        private void StopAudio()
        {
            BackgroundMediaPlayer.Shutdown();
            _mediaPlayer = null;
            _isAudioMuted = true;
            UpdateAudioIcon(_isAudioMuted);
            AudioButtonEnabled = false;
        }

        
        private void StartAudio()
        {
            if (!_selectedCameraHasRelatedMicrophone)
            {
                return;
            }
            
            var audioParams = GetRequestAudioStreamParams(StreamParamsHelper.SignalType.Playback, SelectedCamera.RelatedMicrophone);

            var playbackParams = new PlaybackParams
            {
                ControllerId = _video.StreamId
            };

            var response = _connection.Audio.RequestAudioStream(audioParams, playbackParams, DefaultTimeout);
            if (response.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage(response.ErrorCode == ErrorCodes.InsufficientUserRights ? "Insufficient Rights." : "Microphone Not Available.");
                return;
            }
            _mediaPlayer = BackgroundMediaPlayer.Current;
            _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri($@"http://{_connection.IpAddress}:{_connection.Port}/{_connection.AudioAlias}/{response.VideoId}"));
            _mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            _isAudioMuted = false;
            UpdateAudioIcon(_isAudioMuted);
            AudioButtonEnabled = true;
        }
    }

    public class RelatedViewGroupItem
    {
        public ViewGroupTree CameraItem { get; set; }
        public Guid RelatedMicrophone { get; set; }
        public Guid RelatedSpeaker { get; set; }
    }
}
