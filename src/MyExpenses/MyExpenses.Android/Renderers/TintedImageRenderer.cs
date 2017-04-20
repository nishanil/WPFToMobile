using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.ComponentModel;
using Android.Graphics;
using MyExpenses;
using MyExpenses.Droid.Renderers;

[assembly: ExportRenderer(typeof(TintedImage), typeof(TintedImageRenderer))]
namespace MyExpenses.Droid.Renderers
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
            if (Control == null || Element == null)
                return;

            var colorFromResources = ((Xamarin.Forms.Color)App.Current.Resources["ImageTintColor"]).ToAndroid();

            var colorFilter = new PorterDuffColorFilter(colorFromResources, PorterDuff.Mode.SrcIn);
            Control.SetColorFilter(colorFilter);
        }
    }
}