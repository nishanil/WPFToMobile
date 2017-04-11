namespace Expenses.WPF.Services
{
    public interface IServiceFactory
    {
        ICurrentIdentityService CurrentIdentityService { get; }
        INavigationService NavigationService { get; }
        IRepositoryService RepositoryService { get; }
        IViewService ViewService { get; }
    }
}
