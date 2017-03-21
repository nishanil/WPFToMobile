using System;

namespace Expenses.WPF.Services
{
    public interface ICurrentIdentityService
    {
        string Alias { get; }
        int EmployeeId { get; }
        string Manager { get; }
        string Name { get; }
        bool IsManager { get; }

        void SetNewIdentity(string alias, int employeeId, string manager, string name, bool isManager);

        event EventHandler IdentityChanged;
    }
}
