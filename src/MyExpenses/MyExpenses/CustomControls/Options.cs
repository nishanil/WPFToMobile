using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyExpenses
{
    public class Options : View
    {
        public event EventHandler SelectedIndexChanged;
        public IList<string> Items { get; private set; }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create("SelectedIndex", typeof(int), typeof(Options), -1, BindingMode.TwoWay,
        propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            ((Options)bindable).SelectedIndexChanged?.Invoke(bindable, EventArgs.Empty);
        });

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public Options()
        {
            // todo: make it observable
            Items = new List<string>();
        }
    }
}
