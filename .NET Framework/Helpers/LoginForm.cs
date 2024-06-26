using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace Helpers
{
    public partial class LoginForm 
        : Form
    {
        protected readonly Action<Uri, string, string, UserType> _onOkayAction;
        public LoginForm(Action<Uri, string, string, UserType> onOkayAction)
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

        private UserType GetSelecetedAuthentication()
        {
            if (comboBoxAuthentication.InvokeRequired)
            {
                return (UserType)comboBoxAuthentication.Invoke(new Func<UserType>(GetSelecetedAuthentication));
            }
            else
            {
                return comboBoxAuthentication.SelectedItem.ToString().Equals("Windows authentication", StringComparison.InvariantCultureIgnoreCase) ?
                    UserType.ActiveDirectory : UserType.Basic;
            }
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
            _onOkayAction(uri, textBoxUsername.Text, textBoxPassword.Text, GetSelecetedAuthentication());
        }
    }
}
