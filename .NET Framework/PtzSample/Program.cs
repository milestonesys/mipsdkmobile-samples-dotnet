using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

namespace PtzSample
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

            LoginForm = new LoginFormAsync(OnOkayAction);
            Application.Run(LoginForm);

            if (Connection != null)
                Application.Run(new FormLivePtz(Connection));
        }

        private static LoginFormAsync LoginForm { get; set; }
        private static Connection Connection { get; set; }

        private static async void OnOkayAction(Uri uri, string username, string password)
        {
            var channelType = 0 == string.Compare(uri.Scheme, "http", StringComparison.InvariantCultureIgnoreCase)
                ? ChannelTypes.HTTP
                : ChannelTypes.HTTPSecure;
            Connection = new Connection(channelType, uri.Host, (uint)uri.Port);
            Connection.CommandsQueueing = CommandsQueueing.SingleThread;

            var connectResponse = await Connection.ConnectAsync(null, TimeSpan.FromSeconds(15));
            if (connectResponse.ErrorCode != ErrorCodes.Ok)
            {
                Connection = null;
                LoginForm.ProcessError("Not connected to surveillance server");
                return;
            }

            var loginResponse = await Connection.LogInAsync(username, password, ClientTypes.MobileClient, null, TimeSpan.FromMinutes(2));
            if (loginResponse.ErrorCode != ErrorCodes.Ok)
            {
                Connection = null;
                LoginForm.ProcessError("Not loged in to the surveillance server");
                return;
            }

            Connection.RunHeartBeat = true;

            LoginForm.Close();
        }
    }
}
