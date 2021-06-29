using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helpers
{
    public partial class LoginForm 
        : Form
    {
        protected readonly Action<Uri, string, string> _onOkayAction;
        public LoginForm(Action<Uri, string, string> onOkayAction)
        {
            InitializeComponent();

            _onOkayAction = onOkayAction;
            UpdateState(true);
        }

        internal LoginForm()
            : this(null)
        {
        }

        protected virtual void OnButtonOkClick(object sender, EventArgs e)
        {
            UpdateState(false);

            Task.Factory.StartNew(OnOkayClickedTask);
        }

        private void OnButtonCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void UpdateState(bool inputEnabled)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new Action<bool>(UpdateState), new object[] {inputEnabled});
            else
            {
                panelInput.Enabled = inputEnabled;
                buttonOk.Enabled = inputEnabled;
                buttonCancel.Enabled = inputEnabled;

                progressBarOnOk.Visible = !inputEnabled;
                progressBarOnOk.Enabled = !inputEnabled;
            }
        }

        private void OnOkayClickedTask()
        {
            try
            {
                CallTeAction();

                CloseDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

                UpdateState(true);
            }
        }

        public void CloseDialog()
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new Action(CloseDialog));
            else
                this.Close();
        }

        protected void CallTeAction()
        {
            var uri = new Uri(textBoxUrl.Text);
            _onOkayAction(uri, textBoxUsername.Text, textBoxPassword.Text);
        }
    }
}
