using System;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace Helpers
{
    public partial class LoginFormAsync 
        : Helpers.LoginForm
    {
        internal LoginFormAsync()
            : this(null)
        {
        }

        public LoginFormAsync(Action<Uri, string, string, UserType> onOkayAction)
            : base(onOkayAction)
        {
            InitializeComponent();
        }

        protected override void OnButtonOkClick(object sender, EventArgs e)
        {
            UpdateState(false);

            CallTeAction();
        }

        public void ProcessError(string message)
        {
            MessageBox.Show(message);

            UpdateState(true);
        }
    }
}
