using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Helpers
{
    public partial class LoginFormAsync 
        : Helpers.LoginForm
    {
        internal LoginFormAsync()
            : this(null)
        {
        }

        public LoginFormAsync(Action<Uri, string, string> onOkayAction)
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
