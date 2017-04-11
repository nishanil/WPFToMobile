using System.Runtime.Serialization;

namespace Expenses.Data.CommunicationModels
{
    [DataContract]
    public class Employee
    {
        public Employee()
        {}

        public Employee(DbEmployee dbEmployee)
        {
            this.EmployeeId = dbEmployee.EmployeeId;
            this.Name = dbEmployee.Name;
            this.Alias = dbEmployee.Alias;
            this.Manager = dbEmployee.Manager;
        }

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Alias { get; set; }

        [DataMember]
        public string Manager { get; set; }
    }
}
