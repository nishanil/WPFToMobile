using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MyExpenses.Droid.Renderers;
using Button = Android.Widget.Button;
using Options = MyExpenses.Options;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(Options), typeof(OptionsRenderer))]
namespace MyExpenses.Droid.Renderers
{
    public class OptionsRenderer : ViewRenderer<Options, Button>
    {

        public void OnClick()
        {
            PopupMenu menu = new PopupMenu(Context, Control);
            foreach (var item in Element.Items)
            {
                menu.Menu.Add(new Java.Lang.String(item));
            }

            menu.MenuItemClick += (s, e) =>
            {
                var itemTitle = e.Item.TitleFormatted;
                Element.SelectedIndex = Element.Items.IndexOf(itemTitle.ToString());
            };

            menu.Show();

        }


        protected override void OnElementChanged(ElementChangedEventArgs<Options> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var button = new Button(Context)
                    {
                        Focusable = false,
                        Clickable = true,
                        Tag = this,
                        //TODO: Fix this with a TitleProperty in Options
                        Text = "Report Type",
                    };
                    button.SetOnClickListener(OptionsListener.Instance);
                    SetNativeControl(button);
                }
            }
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Options.SelectedIndexProperty.PropertyName)
                UpdateOptionsMenu();
        }

        private void UpdateOptionsMenu()
        {
            Control.Text = Element.Items[Element.SelectedIndex];
        }

        class OptionsListener : Java.Lang.Object, IOnClickListener
        {
            public static readonly OptionsListener Instance = new OptionsListener();
            

            public void OnClick(global::Android.Views.View v)
            {

                if (v.Tag is OptionsRenderer options) options.OnClick();
            }
        }
    }
}