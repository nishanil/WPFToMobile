using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MyExpenses;
using MyExpenses.iOS.Renderers;

[assembly: ExportRenderer(typeof(TintedImage), typeof(TintedImageRenderer))]
namespace MyExpenses.iOS.Renderers
{
    public class TintedImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            SetTint();
        }
        void SetTint()
        {
            if (Control?.Image == null || Element == null)
                return;
            var colorFromResources = ((Xamarin.Forms.Color)App.Current.Resources["ImageTintColor"]);

            Control.Image = Control.Image.ImageWithRenderingMode(UIKit.UIImageRenderingMode.AlwaysTemplate);
            Control.TintColor = colorFromResources.ToUIColor();

        }
    }
}