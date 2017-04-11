using Microsoft.Azure.Mobile.Server;

namespace MyExpenses_SIService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}