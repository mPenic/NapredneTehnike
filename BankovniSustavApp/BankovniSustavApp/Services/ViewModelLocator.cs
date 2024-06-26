using BankovniSustavApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankovniSustavApp.Services
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
