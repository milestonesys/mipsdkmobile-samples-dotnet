using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace PlaybackSample
{
    public partial class FormPlayback : Form
    {
        private readonly Connection _connection;
        private PlaybackVideo _playbackVideo;
        private VideoPullProxy _videoPullProxy;

        public FormPlayback(Connection connection)
        {
            InitializeComponent();

            _connection = connection;
            InitializeViewGroups();
        }

        #region Get all the views and cameras

        private void InitializeViewGroups()
        {
            treeViewViews.Nodes.Clear();

            _connection.Views.GetAllViewsAndCameras(new ViewParams(), OnGetAllItemSuccess, OnFail);
        }

        private void OnFail(BaseCommandResponse responseParams)
        {
            MessageBox.Show("Error in MoS communication");
        }

        private void OnGetAllItemSuccess(GetAllItemsResponse responseParams)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new Action<GetAllItemsResponse>(OnGetAllItemSuccess), new object[] { responseParams });
            else
                AddTreeNode(treeViewViews.Nodes, responseParams.AllSubItems);
        }

        private void AddTreeNode(TreeNodeCollection nodes, ViewGroupTree viewGroupItem)
        {
            if ((null == nodes) ||
                (null == viewGroupItem))
                return;

            if ((viewGroupItem.ItemType == ViewItemType.Map) ||
                (viewGroupItem.ItemType == ViewItemType.Carousel))
                return;

            var currentNode = new TreeNode(viewGroupItem.ItemName,
                (int)viewGroupItem.ItemType,
                (int)viewGroupItem.ItemType)
            {
                Tag = viewGroupItem
            };
            nodes.Add(currentNode);

            foreach (ViewGroupTree childItem in viewGroupItem.GetMembersList())
            {
                AddTreeNode(currentNode.Nodes, childItem);
            }
        }

        #endregion

        #region Start & Stop the video 

        private void TreeViewViewsNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var viewTree = e.Node.Tag as ViewGroupTree;
            if ((null == viewTree) ||
                (viewTree.ItemType != ViewItemType.Camera))
                return;

            CloseVideo();

            StartVideo(viewTree.ItemId);
        }

        private void CloseVideo()
        {
            _videoPullProxy?.Stop();
            _videoPullProxy = null;

            _playbackVideo?.Stop();
            _playbackVideo?.Dispose();
            _playbackVideo = null;

            pictureBoxVideo.Image = null;
            UpdatePlaybackControls(0);
        }

        private void StartVideo(Guid itemId)
        {
            var videoParams = new VideoParams()
            {
                CameraId = itemId,
                DestWidth = pictureBoxVideo.Width,
                DestHeight = pictureBoxVideo.Height,
                CompressionLvl = 83,
                FPS = 30,
                MethodType = StreamParamsHelper.MethodType.Pull,
                SignalType = StreamParamsHelper.SignalType.Playback,
                StreamType = StreamParamsHelper.StreamType.Transcoded,
            };

            _connection.Video.RequestStream(videoParams, null, OnRequestStreamSuccess, OnFail);
        }

        private void OnRequestStreamSuccess(RequestStreamResponse responseParams)
        {
            _playbackVideo = _connection.VideoFactory.CreatePlaybackVideo(new RequestStreamResponsePlayback(responseParams));
            _videoPullProxy = new VideoPullProxy(_playbackVideo, _playbackVideo, 15);
            _videoPullProxy.NewFrame = OnNewFrame;
            _videoPullProxy.Start();
            _playbackVideo.Start();
        }

        private void OnNewFrame(VideoFrame frame)
        {
            if (frame != null)
            {
                if (InvokeRequired)
                {
                    // invoke in correct (UX) context
                    BeginInvoke(new Action<VideoFrame>(OnNewFrame), new object[] { frame });
                }
                else
                {
                    if (frame.Data?.Length > 0)
                    {
                        using (var ms = new MemoryStream(frame.Data))
                        {
                            pictureBoxVideo.Image = new Bitmap(ms);
                        }
                    }

                    labelCurrentTime.Text = "Current time: " + TimeConverter.FromLong((long)frame.MainHeader.TimeStampUtcMs).ToString();

                    if (frame.ExtensionPresence(BinaryFrameHeaderHelper.HeaderExtensionFlags.PlaybackEvents))
                    {
                        UpdatePlaybackControls(frame.HeaderExtensionPlaybackEvents.CurrentFlags);
                    }
                }
            }
        }

        #endregion

        #region Handle Playback Commands

        private const string PlayBack = "<";
        private const string PlayNext = ">";
        private const string Pause = "||";

        private void OnButtonBackClick(object sender, EventArgs e)
        {
            OnButtonChangeSpeed(buttonBack, -1);
        }

        private void OnButtonForwardClick(object sender, EventArgs e)
        {
            OnButtonChangeSpeed(buttonForward, 1);
        }

        private void OnButtonChangeSpeed(Button button, int speed)
        {
            speed = (button.Text == Pause) ? 0 : speed;
            _playbackVideo?.PlaybackControl.ChangeSpeed(speed);
        }

        private void UpdatePlaybackControls(uint currentFlags)
        {
            var playbackStatus = currentFlags & (uint)PlaybackFlags.PlayMask;
            switch (playbackStatus)
            {
                case (uint)PlaybackFlags.PlayStopped:
                default:
                    buttonBack.Text = PlayBack;
                    buttonForward.Text = PlayNext;
                    break;

                case (uint)PlaybackFlags.PlayBackward:
                    buttonBack.Text = Pause;
                    buttonForward.Text = PlayNext;
                    break;

                case (uint)PlaybackFlags.PlayForward:
                    buttonBack.Text = PlayBack;
                    buttonForward.Text = Pause;
                    break;
            }
        }

        #endregion

        #region Handle resize

        private void FormPlaybackResize(object sender, EventArgs e)
        {
            _playbackVideo?.StreamControl.Rescale(pictureBoxVideo.Width, pictureBoxVideo.Height);
        }

        #endregion

        #region Change time

        private void GoToTime(object sender, EventArgs e)
        {
            _playbackVideo?.PlaybackControl.GoToTime(dateTimePickerGoTo.Value);
        }

        #endregion
    }
}

