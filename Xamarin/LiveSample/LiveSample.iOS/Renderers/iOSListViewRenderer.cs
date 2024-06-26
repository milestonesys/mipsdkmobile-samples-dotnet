using UIKit;
using VideoOS.Mobile.SDK.Samples.Xamarin.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ListView), typeof(iOSListViewRenderer))]
namespace VideoOS.Mobile.SDK.Samples.Xamarin.iOS.Renderers
{
    public class iOSListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var tableView = Control;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
                tableView.SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);
                tableView.TableFooterView = new UILabel { Text = "Footer" };
            }
        } 
    }
}