using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Expenses.WPF.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this._propertyChanged += value; }
            remove { this._propertyChanged -= value; }
        }

        /// <summary>
        /// The private event.
        /// </summary>
        private event PropertyChangedEventHandler _propertyChanged = delegate { };


        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)lambda.Body;

            this.NotifyOfPropertyChange(memberExpression.Member.Name);
        }

        /// <summary>
        /// Raises the property changed event for the given property.
        /// </summary>
        /// <param name="property">The property that is raising the event.</param>
        public void NotifyOfPropertyChange(string property)
        {
            this.RaisePropertyChanged(property, true);
        }

        /// <summary>
        /// Raises the property changed event for the given property.
        /// </summary>
        /// <param name="property">The property that is raising the event.</param>
        /// <param name="verifyProperty">if set to <c>true</c> the property should be verified.</param>
        private void RaisePropertyChanged(string property, bool verifyProperty)
        {
            var handler = this._propertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }


        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; this.NotifyOfPropertyChange(() => this.Title); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; this.NotifyOfPropertyChange(() => this.IsBusy); }
        }


        private ICommand refreshDataCommand;

        public ICommand RefreshDataCommand {
            get { return refreshDataCommand; }
            set { refreshDataCommand = value; this.NotifyOfPropertyChange(() => this.RefreshDataCommand); }
        }

    }
}
