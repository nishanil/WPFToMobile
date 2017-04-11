using System;

namespace Expenses.WPF.Services
{
    public class CurrentIdentityService : ICurrentIdentityService
    {
        public void SetNewIdentity(string alias, int employeeId, string manager, string name, bool isManager)
        {
            this._alias = alias;
            this._employeeId = employeeId;
            this._isManager = isManager;
            this._manager = manager;
            this._name = name;
            if (this.IdentityChanged != null)
            {
                this.IdentityChanged.Invoke(this, null);
            }
        }

        private string _alias;
        public string Alias
        {
	        get 
	        {
                return this._alias;
	        }
        }

        private int _employeeId;
        public int EmployeeId
        {
	        get 
	        {
                return this._employeeId;
	        }
        }

        private string _manager;
        public string Manager
        {
	        get 
	        {
                return this._manager;
	        }
        }

        private string _name;
        public string Name
        {
	        get 
	        { 
		        return this._name;
	        }
        }

        private bool _isManager;
        public bool IsManager
        {
	        get 
	        {
                return this._isManager;
	        }
        }

        public event EventHandler IdentityChanged;
    }
}
