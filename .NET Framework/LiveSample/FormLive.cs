using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.ViewGroups;

namespace LiveSample
{
    public partial class FormLive
        : Form
    {
        private readonly List<ViewGroupTree> _listViewItems = new List<ViewGroupTree>();
        private LiveVideo _liveVideo = null;

        public FormLive(Connection connection)
        {
            InitializeComponent();

            InitializeViews();
            this.Text =connection.ServerFeatures.ServerDescription + " " + (connection.ServerCapabilities.ServerProductCode == "1" ? "Main" : "LTSB");
            bindingSourceViewGroupTree.DataSource = _listViewItems;
            bindingSourceViewGroupTree.ResetBindings(false);
            dataGridViewCameras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        #region View items initialization

        private void InitializeViews()
        {
            try
            {
                var allCamerasViews = ViewGroupsHelper.GetAllCamerasView(Program.Connection.Views, new ViewParams(), TimeSpan.FromSeconds(15));

                ProcessViewItem(allCamerasViews);
            }
            catch (Exception)
            {
            }
        }

        private void ProcessViewItem(ViewGroupTree item)
        {
            if (item.ItemType == ViewItemType.Camera)
                _listViewItems.Add(item);

            foreach (var subItem in item.GetMembersList())
            {
                ProcessViewItem((ViewGroupTree)subItem);
            }
        }

        #endregion

        #region camera selection from the list

        private void OnDataGridViewCamerasSelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCameras.SelectedRows.Count > 0)
            {
                var selIndex = dataGridViewCameras.SelectedRows[0].Index;

                CloseVideo();

                StartVideo(_listViewItems[selIndex].CameraId);
            }
        }

        private void CloseVideo()
        {
            _liveVideo?.Stop();
            _liveVideo?.Dispose();
            _liveVideo = null;

            pictureBoxVideo.Image = null;
            ProcessLiveFlags((uint)LiveFlags.Nothing);
        }

        private void StartVideo(Guid cameraId)
        {
            var videoParams = new VideoParams()
            {
                CameraId = cameraId,
                DestWidth = pictureBoxVideo.Width,
                DestHeight = pictureBoxVideo.Height,
                CompressionLvl = 83,
                FPS = 30,
                MethodType = StreamParamsHelper.MethodType.Push,
                SignalType = StreamParamsHelper.SignalType.Live,
                StreamType = StreamParamsHelper.StreamType.Transcoded,
            };
            var response = Program.Connection.Video.RequestStream(videoParams, null, TimeSpan.FromSeconds(30));

            if (response.ErrorCode != ErrorCodes.Ok)
            {
                MessageBox.Show("Error requesting stream from camera", response.ErrorCode.ToString());
                return;
            }

            _liveVideo = Program.Connection.VideoFactory.CreateLiveVideo(new RequestStreamResponseLive(response));
            _liveVideo.NewFrame = OnNewFrame;
            _liveVideo.Start();
        }

        private void OnNewFrame(VideoFrame frame)
        {
            if (this.InvokeRequired)
            {
                // invoke in correct (UX) context
                BeginInvoke(new Action<VideoFrame>(OnNewFrame), new object[] { frame });
            }
            else
            {
                if ((frame.Data != null) &&
                    (frame.Data.Any()))
                {
                    using (var ms = new MemoryStream(frame.Data))
                    {
                        pictureBoxVideo.Image = new Bitmap(ms);
                    }
                }

                if (frame.ExtensionPresence(BinaryFrameHeaderHelper.HeaderExtensionFlags.LiveEvents))
                    ProcessLiveFlags(frame.HeaderExtensionLiveEvents.CurrentFlags);
            }
        }

        private void ProcessLiveFlags(uint flags)
        {
            labelMotion.Visible = IsFlagSet(flags, LiveFlags.Motion);
            labelRecording.Visible = IsFlagSet(flags, LiveFlags.Recording);
        }

        private bool IsFlagSet(uint flags, LiveFlags mask)
        {
            return (flags & (uint)mask) == (uint)mask;
        }

        #endregion

        #region Close and Resize events

        private void FormLiveFormClosing(object sender, FormClosingEventArgs e)
        {
            CloseVideo();

            Program.Connection.Dispose();
        }

        private void FormLiveResize(object sender, EventArgs e)
        {
            _liveVideo?.StreamControl.ChangeStream(
                new VideoParams()
                {
                    DestWidth = pictureBoxVideo.Width,
                    DestHeight = pictureBoxVideo.Height,
                });
        }

        #endregion
    }
}
