using Microsoft.Practices.Unity;

namespace Expenses.WPF.Services
{
    public class ServiceFactoryImpl : IServiceFactory
    {
        private IUnityContainer _container;

        public ServiceFactoryImpl(UnityContainer container)
        {
            this._container = container;
        }

        public ICurrentIdentityService CurrentIdentityService
        {
            get 
            { 
                return this._container.Resolve<ICurrentIdentityService>(); 
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this._container.Resolve<INavigationService>();
            }
        }

        public IRepositoryService RepositoryService
        {
            get
            {
                return this._container.Resolve<IRepositoryService>();
            }
        }

        public IViewService ViewService
        {
            get
            {
                return this._container.Resolve<IViewService>();
            }
        }
    }
}
