using System;

namespace Expenses.WPF
{
    public class EventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public EventArgs(T data)
        {
            this.Data = data;
        }

    }
}
