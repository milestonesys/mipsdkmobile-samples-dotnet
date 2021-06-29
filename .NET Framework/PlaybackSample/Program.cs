using Helpers;
using System;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

namespace PlaybackSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize the Mobile SDK
            VideoOS.Mobile.SDK.Environment.Instance.Initialize();

            Application.Run(new LoginForm(OnOkayAction));

            if (Connection != null)
                Application.Run(new FormPlayback(Connection));
        }

        private static bool Initialized = false;
        private static Connection Connection { get; set; }
        private static string Username { get; set; }
        private static string Password { get; set; }

        private static void OnOkayAction(Uri uri, string username, string password)
        {
            Initialized = false;
            Username = username;
            Password = password;

            var channelType = 0 == string.Compare(uri.Scheme, "http", StringComparison.InvariantCultureIgnoreCase)
                ? ChannelTypes.HTTP
                : ChannelTypes.HTTPSecure;
            Connection = new Connection(channelType, uri.Host, (uint)uri.Port);
            Connection.CommandsQueueing = CommandsQueueing.SingleThread;

            Connection.Connect(null, OnConnectSuccess, OnFail);

            while (false == Initialized)
            {
                Thread.Sleep(1000);
            }

            if (null == Connection)
                throw new Exception("Not connected to surveillance server." + Environment.NewLine + "Check server address or credentials.");
        }

        private static void OnFail(BaseCommandResponse responseParams)
        {
            Connection = null;
            Initialized = true;
        }

        private static void OnConnectSuccess(ConnectResponse responseParams)
        {
            Connection.LogIn(Username, Password, ClientTypes.MobileClient, UserType.Unknown, null,
                OnLoginSuccess, OnFail);
        }

        private static void OnLoginSuccess(BaseCommandResponse responseParams)
        {
            Connection.RunHeartBeat = true;
            Initialized = true;
        }
    }
}
