using BookmarkSample.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.ViewGroups;

namespace BookmarkSample
{
    public partial class FormBookmark
        : Form
    {
        #region Private data

        private Connection _connection;
        private TimeSpan _timeout = TimeSpan.FromSeconds(10);
        private IDictionary<string, Guid> _cameraGuidsByName;

        #endregion

        #region Constructors

        public FormBookmark(Connection connection)
        {
            InitializeComponent();
            this.Text = string.Format(Resources.FormTitle, connection.ServerFeatures.ServerDescription);
            _connection = connection;

            InitializeCameraList();
        }

        #endregion

        #region Init/Close

        private void InitializeCameraList()
        {
            _cameraGuidsByName = ViewGroupsHelper.GetAllCamerasView(_connection.Views, new ViewParams(), TimeSpan.FromSeconds(15))
                .Descendants(ViewItemType.Camera)
                .ToDictionary(item => item.ItemName, item => item.CameraId);
            comboBoxCamera.Items.Clear();
            comboBoxCamera.Items.AddRange(_cameraGuidsByName.Keys.ToArray());
            comboBoxCamera.SelectedIndex = 0;
        }

        private void FormBookmark_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Connection.Dispose();
        }

        #endregion

        #region Create

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            var result = CreateBookmark(
                textBoxName.Text,
                _cameraGuidsByName[comboBoxCamera.Text],
                dateTimePickerTime.Value.ToUniversalTime()); // always provide UTC time

            HandleCreateBookmarkResponse(result);
        }

        private BaseCommandResponse CreateBookmark(string name, Guid cameraId, DateTime time)
        {
            var bookmarkParams = new BookmarkParams
            {
                Name = name,
                CameraId = cameraId,
                Time = time,
            };

            return _connection.Bookmarks.CreateBookmark(bookmarkParams, _timeout);
        }

        private void HandleCreateBookmarkResponse(BaseCommandResponse response)
        {
            if (response.Result == Command.CommandResultTypes.ResultOk)
            {
                var guid = Guid.Parse(response.OutputParams[CommunicationCommands.BookmarkId]);
                textBoxUpdateId.Text = guid.ToString();
                textBoxDeleteId.Text = guid.ToString();
                InfoMessage(string.Format(Resources.CreateOK, guid));
            }
            else
            {
                ErrorMessage(string.Format(Resources.CreateError, response.ErrorCode));
            }
        }

        #endregion

        #region Update

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (!Guid.TryParse(textBoxUpdateId.Text, out var bookmarkId))
            {
                InfoMessage(Resources.MissingID);
                return;
            }

            var result = UpdateBookmark(bookmarkId,
                textBoxNewName.Text,
                dateTimePickerNewTime.Value.ToUniversalTime()); // always provide UTC time

            HandleUpdateBookmarkResponse(result);
        }

        private BaseCommandResponse UpdateBookmark(Guid id, string newName, DateTime newTime)
        {
            var bookmarkParams = new BookmarkParams
            {
                BookmarkId = id,
                Name = newName,
                Time = newTime,
                // the next two rows are added simply for convinience of the sample. Time should always be between StartTime and EndTime, otherwise the update will fail
                StartTime = newTime.AddSeconds(-3),
                EndTime = newTime.AddSeconds(30),
            };

            return _connection.Bookmarks.UpdateBookmark(bookmarkParams, _timeout);
        }

        private void HandleUpdateBookmarkResponse(BaseCommandResponse response)
        {
            if (response.Result == Command.CommandResultTypes.ResultOk)
            {
                InfoMessage(Resources.UpdateOK);
            }
            else
            {
                ErrorMessage(string.Format(Resources.UpdateError, response.ErrorCode));
            }
        }

        #endregion

        #region Delete

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (!Guid.TryParse(textBoxDeleteId.Text, out var bookmarkId))
            {
                InfoMessage(Resources.MissingID);
                return;
            }

            var result = DeleteBookmark(bookmarkId);

            HandleDeleteBookmarkResponse(result);
        }

        private BaseCommandResponse DeleteBookmark(Guid id)
        {
            var bookmarkParams = new BookmarkParams
            {
                BookmarkId = id,
            };

            return _connection.Bookmarks.DeleteBookmark(bookmarkParams, _timeout);
        }

        private void HandleDeleteBookmarkResponse(BaseCommandResponse response)
        {
            if (response.Result == Command.CommandResultTypes.ResultOk)
            {
                InfoMessage(Resources.DeleteOK);
            }
            else
            {
                ErrorMessage(string.Format(Resources.DeleteError, response.ErrorCode));
            }
        }

        #endregion

        #region Get

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPageGet)
            {
                var result = GetLatestBookmarks(30);

                HandleGetBookmarksResponse(result);
            }
        }

        private BaseCommandResponse GetLatestBookmarks(int count)
        {
            var bookmarkParams = new BookmarkParams
            {
                Count = count,
            };

            return _connection.Bookmarks.GetBookmarks(bookmarkParams, _timeout);
        }

        private void HandleGetBookmarksResponse(BaseCommandResponse response)
        {
            if (response.Result == Command.CommandResultTypes.ResultOk)
            {
                if (response.Command.Items.Count > 0)
                {
                    dataGridView.DataSource = response.Command.Items
                        .Select(item => new BookmarkViewEntry(item))
                        .ToArray();
                }
                else
                {
                    dataGridView.DataSource = null;
                    InfoMessage(Resources.NoBookmarks);
                }
            }
            else
            {
                ErrorMessage(string.Format(Resources.GetError, response.ErrorCode));
            }
        }

        #endregion

        #region Message handling

        private void ErrorMessage(string message)
        {
            MessageBox.Show(message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InfoMessage(string message)
        {
            MessageBox.Show(message, Resources.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
