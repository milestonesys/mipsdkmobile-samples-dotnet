using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.UI.Xaml.Controls;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.Audio;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.ViewGroups;

namespace PushToTalkUwpSample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        [ComImport]
        [Guid("5B0D3235-4DBA-4D44-865E-8F1D0E4FD04D")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        unsafe interface IMemoryBufferByteAccess
        {
            void GetBuffer(out byte* buffer, out uint capacity);
        }

        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private Connection _connection;
        private RelatedViewGroupItem _selectedCamera;
        private bool _isPushingAudio;
        private bool _isConnceted;
        private IEnumerable<RelatedViewGroupItem> _allCameras;

        private PushToTalk _pushToTalk;
        private MediaFrameReader _mediaFrameReader;


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

                StopCapturing();
                StopPushingAudio();
                StartPushingAudio();
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
        public bool IsPushingAudio
        {
            get
            {
                return _isPushingAudio;
            }
            set
            {
                SetProperty(ref _isPushingAudio, value);
            }
        }

        /// <summary>
        /// Gets or sets the connect and load microphones command.
        /// </summary>
        public ICommand ConnectAndLoadCamerasWithSpeakersCommand { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            // Initialize the Mobile SDK
            VideoOS.Mobile.SDK.Phone.Environment.Instance.Initialze();

            // Initialize commands
            ConnectAndLoadCamerasWithSpeakersCommand = new RelayCommand(ConnectAndLoadCamerasWithSpeakers);
        }

        /// <summary>
        /// Connects and loads cameras.
        /// </summary>
        /// <param name="objData">The object data.</param>
        public void ConnectAndLoadCamerasWithSpeakers(object objData)
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

            AllCameras = cameras.Where(c => c.RelatedSpeaker != Guid.Empty).ToList();
            IsConnected = true;

            if (!AllCameras.Any())
            {
                ShowErrorMessage("There are no cameras with related speakers");
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

        private async void StartPushingAudio()
        {
            var mediaCapture = new MediaCapture();

            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings()
            {
                StreamingCaptureMode = StreamingCaptureMode.Audio,
            };
            await mediaCapture.InitializeAsync(settings);

            var audioFrameSources = mediaCapture.FrameSources.Where(x => x.Value.Info.MediaStreamType == MediaStreamType.Audio).ToArray();
            if (!audioFrameSources.Any())
            {
                ShowErrorMessage("No audio frame source was found.");
                return;
            }
            MediaFrameSource frameSource = audioFrameSources.FirstOrDefault().Value;

            foreach (var afs in audioFrameSources)
            {
                foreach (var valueSupportedFormat in afs.Value.SupportedFormats)
                {
                    Debug.WriteLine($"{valueSupportedFormat.AudioEncodingProperties.SampleRate} : {valueSupportedFormat.AudioEncodingProperties.BitsPerSample} : {valueSupportedFormat.AudioEncodingProperties.ChannelCount}");
                }
            }

            var audioParams = GetRequestStreamInParams(SelectedCamera.RelatedSpeaker,
                                                        frameSource.CurrentFormat.AudioEncodingProperties.SampleRate,
                                                        16,
                                                        frameSource.CurrentFormat.AudioEncodingProperties.ChannelCount);

            var response = _connection.Audio.RequestAudioStreamIn(audioParams, DefaultTimeout);
            if (response.ErrorCode != ErrorCodes.Ok)
            {
                ShowErrorMessage(response.ErrorCode == ErrorCodes.InsufficientUserRights ? "Insufficient Rights." : "Speaker Not Available.");
                return;
            }
            _pushToTalk = _connection.AudioFactory.CreatePushToTalk(response);

            _mediaFrameReader = await mediaCapture.CreateFrameReaderAsync(frameSource);
            _mediaFrameReader.FrameArrived += mediaFrameReader_DataAvailable;

            var status = await _mediaFrameReader.StartAsync();
            if (status != MediaFrameReaderStartStatus.Success)
            {
                ShowErrorMessage("The MediaFrameReader couldn't start.");
            }
            IsPushingAudio = true;
        }

        void mediaFrameReader_DataAvailable(MediaFrameReader sender, MediaFrameArrivedEventArgs args)
        {
            using (MediaFrameReference reference = sender.TryAcquireLatestFrame())
            {
                if (reference != null)
                {
                    var resultData = ProcessAudioFrame(reference.AudioMediaFrame);
                    if (resultData != null)
                    {
                        var videoPushFrame = new PushVideoFrame { FrameData = resultData };
                        _pushToTalk?.EnqueueFrame(videoPushFrame);
                    }
                }
            }
        }

        private unsafe byte[] ProcessAudioFrame(AudioMediaFrame audioMediaFrame)
        {
            using (AudioFrame audioFrame = audioMediaFrame.GetAudioFrame())
            using (AudioBuffer buffer = audioFrame.LockBuffer(AudioBufferAccessMode.Read))
            using (IMemoryBufferReference reference = buffer.CreateReference())
            {
                byte* dataInBytes;
                uint capacityInBytes;
                float* dataInFloat;

                ((IMemoryBufferByteAccess)reference).GetBuffer(out dataInBytes, out capacityInBytes);
                // The requested format was float
                dataInFloat = (float*)dataInBytes;

                float[] arr = new float[capacityInBytes/4];
                Marshal.Copy((IntPtr)dataInFloat, arr, 0, (int)capacityInBytes/4);

                byte[] newArray16Bit = new byte[capacityInBytes / 2];
                short two;
                for (int i = 0, j = 0; i < arr.Length; i += 1, j += 2)
                {
                    two = (short)(arr[i] * short.MaxValue);

                    newArray16Bit[j] = (byte)(two & 0xFF);
                    newArray16Bit[j + 1] = (byte)((two >> 8) & 0xFF);
                }

                return newArray16Bit;
            }
        }

        private async void StopCapturing()
        {
            if (_mediaFrameReader != null)
            {
                await _mediaFrameReader.StopAsync();
                _mediaFrameReader.Dispose();
                _mediaFrameReader = null;
            }
            
        }

        private void StopPushingAudio()
        {
            _pushToTalk?.Dispose();
            _pushToTalk = null;
            IsPushingAudio = false;
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
                var relatedItems = item.GetMembersList();
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

        private static AudioParams GetRequestStreamInParams(Guid microphoneId, uint samplingRate, uint bitsPerSample, uint channelsCount)
        {
            var audioParams = new AudioParams()
            {
                ItemId = microphoneId,
                AudioEncoding = StreamParamsHelper.AudioEncoding.Pcm,
                StreamHeaders = StreamParamsHelper.StreamHeaders.AllPresent,
                AudioSamplingRate = samplingRate,
                AudioBitsPerSample = bitsPerSample,
                AudioChannelsNumber = channelsCount
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