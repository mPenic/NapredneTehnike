using BankovniSustavApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BankovniSustavApp
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // Initialize if needed
        }

        public DashboardViewModel DashboardViewModel => App.ServiceProvider.GetRequiredService<DashboardViewModel>();
    }
}
