using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VideoOS.Mobile.SDK.Samples.Xamarin.ViewModels;

namespace VideoOS.Mobile.SDK.Samples.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveView
    {
        private SKBitmap _webBitmap;

        public LiveView(Connection connection, Guid cameraId, string cameraName)
        {
            InitializeComponent();

            BindingContext = new LiveViewModel(connection, cameraId, cameraName);

            FramesContainer.PaintSurface += OnCanvasViewPaintSurface;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                ((LiveViewModel)BindingContext).RefreshVideoImage += newFrame =>
                {
                    _webBitmap = SKBitmap.Decode(newFrame.Data);
                    Device.BeginInvokeOnMainThread(() => FramesContainer.InvalidateSurface());
                };
            }
        }
        
        protected override void OnAppearing()
        {
            ((LiveViewModel)BindingContext).LoadData();
        }

        protected override void OnDisappearing()
        {
            ((LiveViewModel)BindingContext).StopVideoStream();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            if (_webBitmap != null)
            {
                var isWide = (double)_webBitmap.Width / _webBitmap.Height > 1.33333;
                FramesContainer.Scale = isWide ? (double)info.Width / _webBitmap.Width : (double)info.Height / _webBitmap.Height;

                var x = (float)(info.Width - _webBitmap.Width) / 2;
                var y = (float)(info.Height - _webBitmap.Height) / 2;
                canvas.DrawBitmap(_webBitmap, x, y);
            }
        }
    }
}