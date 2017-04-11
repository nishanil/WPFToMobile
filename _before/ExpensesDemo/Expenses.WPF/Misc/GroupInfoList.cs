using System.Collections.Generic;

namespace Expenses.WPF.Misc
{
    public class GroupInfoList<T>: List<object>
    {
        public object Key { get; set; }
        public decimal Amount { get; set; }
        public string Summary { get; set; }
        public string Details1 { get; set; }
        public string Details2 { get; set; }
        public string Details3 { get; set; }
        public string Details4 { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>)base.GetEnumerator();
        }

        public void ImportList(IEnumerable<object> list)
        {
            foreach (object item in list)
            {
                this.Add(item);
            }
        }

    }
}
