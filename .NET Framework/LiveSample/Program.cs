using Helpers;
using System;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

namespace LiveSample
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
            VideoOS.Mobile.SDK.Portable.Environment.Instance.Initialize();

            Application.Run(new LoginForm(OnOkayAction));

            if (Initialized)
                Application.Run(new FormLive(Connection));
        }

        private static bool Initialized = false;
        public static Connection Connection { get; private set; }

        private static void OnOkayAction(Uri uri, string username, string password, UserType userType)
        {
            var channelType = uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase)
                ? ChannelTypes.HTTP
                : ChannelTypes.HTTPSecure;
            Connection = new Connection(channelType, uri.Host, (uint)uri.Port);
            Connection.CommandsQueueing = CommandsQueueing.SingleThread;

            var connectResponse = Connection.Connect(null, TimeSpan.FromSeconds(15));
            if (connectResponse.ErrorCode != ErrorCodes.Ok)
                throw new Exception("Not connected to surveillance server");

            var loginResponse = Connection.LogIn(username, password, ClientTypes.MobileClient, TimeSpan.FromMinutes(2), userType);
            if (loginResponse.ErrorCode != ErrorCodes.Ok)
                throw new Exception("Not loged in to the surveillance server");

            Connection.RunHeartBeat = true;

            Initialized = true;
        }

        
    }
}
