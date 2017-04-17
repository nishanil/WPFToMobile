using System;

namespace Expenses.WPF.Services
{
    public interface ICurrentIdentityService
    {
        string Alias { get; }
        string EmployeeId { get; }
        string Manager { get; }
        string Name { get; }
        bool IsManager { get; }

        void SetNewIdentity(string alias, string employeeId, string manager, string name, bool isManager);

        event EventHandler IdentityChanged;
    }
}
