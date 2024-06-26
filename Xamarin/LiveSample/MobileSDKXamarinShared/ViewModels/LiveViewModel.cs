using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using Xamarin.Forms;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels
{
    /// <summary>
    /// Live video view model.
    /// </summary>
    /// <seealso cref="VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels.BaseViewModel" />
    public class LiveViewModel : BaseViewModel
    {
        private bool _firstFrame;
        private Guid _cameraId;
        private string _cameraName;
        private Video _video;

        /// <summary>
        /// Occurs when the video image refreshes.
        /// </summary>
        public event Action<VideoFrame> RefreshVideoImage;

        /// <summary>
        /// Gets or sets the name of the camera.
        /// </summary>
        public string CameraName
        {
            get
            {
                return _cameraName;
            }
            set
            {
                if (_cameraName != value)
                {
                    _cameraName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveViewModel"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="cameraId">The camera identifier.</param>
        /// <param name="cameraName">Name of the camera.</param>
        public LiveViewModel(Connection connection, Guid cameraId, string cameraName)
        {
            Connection = connection;
            CameraName = cameraName;
            _cameraId = cameraId;
        }

        /// <summary>
        /// Loads the view model data.
        /// </summary>
        public override void LoadData()
        {
            if (GetConnectionState() != ConnectionStates.LoggedIn || IsBusy)
            {
                return;
            }

            if (_cameraId != Guid.Empty)
            {
                StartReceivingVideo();
            }
        }

        public void StopVideoStream()
        {
            if (_video != null)
            {
                _video.NewFrame = null;
                _video.Stop();
                _video.Dispose();
                _video = null;
            }
        }

        private void StartReceivingVideo()
        {
            try
            {
                IsBusy = true;

                if (_video == null)
                {
                    _firstFrame = true;
                    var videoParams = GetVideoParams();

                    var response = Connection.Video.RequestStream(videoParams, null, App.MaxTimeout);
                    _video = Connection.VideoFactory.CreateLiveVideo(new RequestStreamResponseLive(response));

                    new VideoPullProxy(_video, _video, (int)videoParams.FPS) { NewFrame = OnVideoFrameReceived };
                }

                _video.Start();
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayMessage("Error", "Unable to retrieve video", "Close");
                });
            }
        }

        private VideoParams GetVideoParams()
        {
            return new VideoParams
            {
                SignalType = StreamParamsHelper.SignalType.Live,
                MethodType = StreamParamsHelper.MethodType.Pull,
                FPS = 8,
                CompressionLvl = 90,
                CameraId = _cameraId,
                DestWidth = App.ScreenHeight >= App.ScreenWidth ? App.ScreenWidth : App.ScreenHeight,
                DestHeight = App.ScreenHeight >= App.ScreenWidth ? App.ScreenHeight : App.ScreenWidth,
                RequiresDownsampling = false,
            };
        }

        private void OnFirstFrameReceived()
        {
            if (_firstFrame)
            {
                _firstFrame = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                });
            }
        }

        private void OnVideoFrameReceived(VideoFrame frame)
        {
            OnFirstFrameReceived();

            if (frame != null && frame.Data != null && frame.Data.Length > 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    RefreshVideoImage?.Invoke(frame);
                });
            }
        }
    }
}