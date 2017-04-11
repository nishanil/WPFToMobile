using System;
using System.Windows;
using System.Windows.Controls;

namespace Expenses.WPF.Misc
{
    /// <summary>
    /// This attached behavior is meant for use with DataGrid.
    /// </summary>
    public static class SelectedItemsBehavior
    {
        public static readonly DependencyProperty SelectedItemsChangedHandlerProperty =
            DependencyProperty.RegisterAttached("SelectedItemsChangedHandler", typeof(RelayCommand), typeof(SelectedItemsBehavior), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemsChangedHandlerChanged)));

        public static RelayCommand GetSelectedItemsChangedHandler(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (RelayCommand)element.GetValue(SelectedItemsChangedHandlerProperty);
        }

        public static void SetSelectedItemsChangedHandler(DependencyObject element, RelayCommand value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(SelectedItemsChangedHandlerProperty, value);
        }

        private static void OnSelectedItemsChangedHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)d;

            if (e.OldValue == null && e.NewValue != null)
            {
                dataGrid.SelectionChanged += ItemsControl_SelectionChanged;
            }

            if (e.OldValue != null && e.NewValue == null)
            {
                dataGrid.SelectionChanged -= ItemsControl_SelectionChanged;
            }
        }

        public static void ItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            RelayCommand itemsChangedHandler = GetSelectedItemsChangedHandler(dataGrid);

            itemsChangedHandler.Execute(dataGrid.SelectedItems);
        }
    }
}
