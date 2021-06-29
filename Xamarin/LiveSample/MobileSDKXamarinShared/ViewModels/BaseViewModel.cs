using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using Xamarin.Forms;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels
{
    /// <summary>
    /// Base view model class.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        /// <summary>
        /// Occurs when property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        protected Connection Connection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance is busy (loading).
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads the view model data.
        /// </summary>
        public virtual void LoadData()
        { }

        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        /// <returns></returns>
        public ConnectionStates GetConnectionState()
        {
            if (Connection != null)
            {
                var connectionEvents = Connection.StateChange.GetState() as ConnectionEventArgs;
                if (connectionEvents != null)
                {
                    return connectionEvents.State;
                }
            }

            return ConnectionStates.Disconnected;
        }

        /// <summary>
        /// Presents an alert dialog to the application user with a single cancel button.
        /// </summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="cancelButtonText">Text to be displayed on the 'Cancel' button.</param>
        /// <returns>An awaitable Task that displays a message with a single cancel button.</returns>
        public async Task DisplayMessage(string title, string message, string cancelButtonText)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancelButtonText);
        }

        /// <summary>
        /// Called to notify when property has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}