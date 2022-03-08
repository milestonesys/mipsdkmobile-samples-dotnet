using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace PtzSample
{
    public partial class FormLivePtz 
        : Form
    {
        private List<ViewGroupTree> _listViewItems = new List<ViewGroupTree>();
        private LiveVideo _liveVideo = null;
        private readonly Connection _connection = null;
        private Guid _activeCameraId = Guid.Empty;

        #region Construction and initialization

        public FormLivePtz(Connection connection)
        {
            _connection = connection;

            InitializeComponent();

            EnablePtzMoveButtons(false);
            EnablePtzPresets(false);
        }

        private async void OnFormLivePtzLoad(object sender, EventArgs e)
        {
            var allCamerasViews = await _connection.Views.GetAllViewsAndCamerasAsync(new ViewParams(), TimeSpan.FromSeconds(30));
            _listViewItems = allCamerasViews.AllSubItems.Descendants(ViewItemType.Camera).OfType<ViewGroupTree>().ToList();
            viewGroupTreeBindingSource.DataSource = _listViewItems;
            viewGroupTreeBindingSource.ResetBindings(false);
            dataGridViewCameras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        #endregion

        #region Video creation

        private void OnDataGridViewCamerasSelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCameras.SelectedRows.Count > 0)
            {
                var selIndex = dataGridViewCameras.SelectedRows[0].Index;

                CloseVideo();

                _activeCameraId = _listViewItems[selIndex].CameraId;
                StartVideo(_activeCameraId);
            }
        }

        private void CloseVideo()
        {
            _liveVideo?.Stop();
            _liveVideo?.Dispose();
            _liveVideo = null;

            pictureBoxVideo.Image = null;
        }

        private async void StartVideo(Guid cameraId)
        {
            var videoParams = new VideoParams()
            {
                CameraId = cameraId,
                DestWidth = pictureBoxVideo.Width,
                DestHeight = pictureBoxVideo.Height,
                CompressionLvl = 81,
                FPS = 30,
                MethodType = StreamParamsHelper.MethodType.Push,
                SignalType = StreamParamsHelper.SignalType.Live,
                StreamType = StreamParamsHelper.StreamType.Transcoded,
            };
            var response = await _connection.Video.RequestStreamAsync(videoParams, null, TimeSpan.FromSeconds(30));

            if (response.ErrorCode != ErrorCodes.Ok)
            {
                MessageBox.Show("Error requesting stream from camera", response.ErrorCode.ToString());
                return;
            }

            ProcessStreamResponse(response);

            _liveVideo = _connection.VideoFactory.CreateLiveVideo(new RequestStreamResponseLive(response));
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
            }
        }

        #endregion

        #region Processing stream response in order to enable/disable ptz and presets 

        private void ProcessStreamResponse(BaseCommandResponse response)
        {
            var ptzMoveEnabled =
                response.OutputParams.ContainsKey(CommunicationCommands.AuthorizationPtz) &&
                response.OutputParams[CommunicationCommands.AuthorizationPtz] == CommunicationCommands.AuthorizationYes;
            EnablePtzMoveButtons(ptzMoveEnabled);

            var ptzPresetEnabled =
                response.OutputParams.ContainsKey(CommunicationCommands.AuthorizationPreset) &&
                response.OutputParams[CommunicationCommands.AuthorizationPreset] == CommunicationCommands.AuthorizationYes;
            EnablePtzPresets(ptzPresetEnabled);
        }

        private void EnablePtzMoveButtons(bool enabled)
        {
            buttonLeft.Enabled = buttonUp.Enabled = buttonRight.Enabled =
                buttonDown.Enabled = buttonCenter.Enabled = buttonIn.Enabled = buttonOut.Enabled = enabled;
        }

        private void EnablePtzPresets(bool enabled)
        {
            comboBoxPresets.Items.Clear();
            comboBoxPresets.Enabled = enabled;

            if (enabled)
                UpdatePtzPresets();
        }

        private async void UpdatePtzPresets()
        {
            var response = await _connection.Ptz.GetPtzPresetsAsync(_activeCameraId, _maxTimeout);

            if (response.ErrorCode == ErrorCodes.NoPresetsAvailable)
                MessageBox.Show("No presets available");
            else
                ProcessMoveResponse(response);

            comboBoxPresets.Items.AddRange(response.Presets.ToArray());
        }

        #endregion

        #region Handling Ptz Commands

        private readonly TimeSpan _maxTimeout = TimeSpan.FromSeconds(30);

        private void OnButtonUpClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnCamera(PtzParamsHelper.PtzMoves.Up);
        }

        private void ButtonRightClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnCamera(PtzParamsHelper.PtzMoves.Right);
        }

        private void ButtonDownClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnStream(PtzParamsHelper.PtzMoves.Down);
        }

        private void ButtonLeftClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnStream(PtzParamsHelper.PtzMoves.Left);
        }

        private void ButtonCenterClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnCamera(PtzParamsHelper.PtzMoves.Home);
        }

        private void OnButtonInClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnCamera(PtzParamsHelper.PtzMoves.ZoomIn);
        }

        private void OnButtonOutClick(object sender, EventArgs e)
        {
            ProcessPtzMoveOnStream(PtzParamsHelper.PtzMoves.ZoomOut);
        }

        private async void ProcessPtzMoveOnCamera(PtzParamsHelper.PtzMoves move)
        {
            var response = await _connection.Ptz.ControlPtzAsync(
                new PtzParams()
                {
                    CameraId = _activeCameraId,
                    PtzMoveType = PtzParamsHelper.PtzMoveTypes.Step,
                    PtzMove = move,
                },
                _maxTimeout);

            ProcessMoveResponse(response);
        }

        private async void ProcessPtzMoveOnStream(PtzParamsHelper.PtzMoves move)
        {
            var response = await _liveVideo.Ptz.ControlPtzAsync(
                new PtzParams()
                {
                    CameraId = _activeCameraId,
                    PtzMoveType = PtzParamsHelper.PtzMoveTypes.Step,
                    PtzMove = move,
                },
                _maxTimeout);

            ProcessMoveResponse(response);
        }

        private void ProcessMoveResponse(BaseCommandResponse response)
        {
            if (response.ErrorCode != ErrorCodes.Ok)
                MessageBox.Show("Error executing ptz command");
        }

        private async void OnComboBoxPresetsSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedPreset = comboBoxPresets.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selectedPreset))
                return;

            var response = await _connection.Ptz.ControlPtzAsync(
                new PtzParams() {CameraId = _activeCameraId, PtzPreset = selectedPreset}, _maxTimeout);

            ProcessMoveResponse(response);
        }

        #endregion
    }
}
