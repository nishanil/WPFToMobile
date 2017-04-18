using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MyExpenses;
using CoreGraphics;
using System.ComponentModel;
using MyExpenses.iOS.Renderers;

[assembly: ExportRenderer(typeof(Options), typeof(OptionsRenderer))]
namespace MyExpenses.iOS.Renderers
{
    public class OptionsRenderer : ViewRenderer<Options, UISegmentedControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Options> e)
        {

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var segmentedControl = new UISegmentedControl(CGRect.Empty);
                    segmentedControl.ValueChanged += (o, args) =>
                    {
                        var selectedSegmentId = (o as UISegmentedControl).SelectedSegment;
                        e.NewElement.SelectedIndex = (int)selectedSegmentId;
                    };
                    //TODO:
                    //segmentedControl.TintColor = Theme.PrimaryColor.ToUIColor();
                    SetNativeControl(segmentedControl);
                }

                Control.RemoveAllSegments();

                int pos = 0;
                foreach (var item in Element.Items)
                {
                    Control.InsertSegment(item, (int)pos, false);
                    pos++;
                }

            }
            base.OnElementChanged(e);
        }




        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Options.SelectedIndexProperty.PropertyName)
                UpdateSelectedSegment();
        }

        private void UpdateSelectedSegment()
        {
            Control.SelectedSegment = (int)Element.SelectedIndex;
        }
    }
}