// Services/NavigationService.cs
using System;
using System.Linq;
using System.Windows;
using BankovniSustavApp;
using BankovniSustavApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
// Ensure you have the Views namespace with all the windows

namespace BankovniSustavApp.Services
{
    public interface INavigationService
    {
        void NavigateToLogin();
        void NavigateToRegister();
        void NavigateToDashboard();
        // Add other navigation methods as needed
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void NavigateToLogin()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Try to get an existing instance of the main window.
                var loginWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

                // If there's no existing main window, or it's not loaded (which means it's closed), create a new instance.
                if (loginWindow == null || !loginWindow.IsLoaded)
                {
                    // Resolve LoginViewModel using the service provider.
                    var loginViewModel = _serviceProvider.GetService<LoginViewModel>();

                    // If the LoginViewModel is not null, create and show the main window.
                    if (loginViewModel != null)
                    {
                        loginWindow = new MainWindow
                        {
                            DataContext = loginViewModel
                        };
                        Application.Current.MainWindow = loginWindow;
                        loginWindow.Show();
                    }
                }
                else
                {
                    // If the main window exists and is loaded, just activate it.
                    loginWindow.Activate();
                }

                // Hide any open RegisterWindow instances instead of closing them.
                var registerWindow = Application.Current.Windows.OfType<RegisterWindow>().FirstOrDefault();
                if (registerWindow != null && registerWindow.IsLoaded)
                {
                    registerWindow.Hide();
                }
            });
        }



        public void NavigateToRegister()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Resolve RegisterViewModel using the public ServiceProvider property
                var registerViewModel = (RegisterViewModel)App.ServiceProvider.GetService(typeof(RegisterViewModel));
                var registerWindow = new RegisterWindow(registerViewModel);
                registerWindow.Show();
                CloseAllWindowsExcept(registerWindow);
            });
        }
        public void NavigateToDashboard()
        {
            // Implement logic to navigate to the Dashboard view
            Application.Current.Dispatcher.Invoke(() =>
            {
                Console.WriteLine("Navigating to dashboard.");
                // Resolve DashboardViewModel using the public ServiceProvider property
                var dashboardViewModel = (DashboardViewModel)App.ServiceProvider.GetService(typeof(DashboardViewModel));
                var dashboardWindow = new DashboardWindow(dashboardViewModel);
                dashboardWindow.Show();
                CloseAllWindowsExcept(dashboardWindow);
            });
        }

        // Add other navigation methods as needed

        private void CloseAllWindowsExcept(Window windowToKeep)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != windowToKeep)
                {
                    window.Close();
                }
            }
        }
    }
}
