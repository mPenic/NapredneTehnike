using System;
using System.Linq;
using System.Windows;
using BankovniSustavApp;
using BankovniSustavApp.ViewModels;
using BankovniSustavApp.Views;
using Microsoft.Extensions.DependencyInjection;


namespace BankovniSustavApp.Services
{
    public interface INavigationService
    {
        void NavigateToLogin();
        void NavigateToRegister();
        void NavigateToDashboard();
        void NavigateToFinnhub();
        void NavigateToBanking();
       
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
                var loginWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

                if (loginWindow == null || !loginWindow.IsLoaded)
                {
                    var loginViewModel = _serviceProvider.GetService<LoginViewModel>();

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
                    loginWindow.Activate();
                }

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
                var registerViewModel = (RegisterViewModel)App.ServiceProvider.GetService(typeof(RegisterViewModel));
                var registerWindow = new RegisterWindow(registerViewModel);
                registerWindow.Show();
                CloseAllWindowsExcept(registerWindow);
            });
        }
        public void NavigateToDashboard()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Console.WriteLine("Navigating to dashboard.");
                var dashboardViewModel = (DashboardViewModel)App.ServiceProvider.GetService(typeof(DashboardViewModel));
                var dashboardWindow = new DashboardWindow(dashboardViewModel);
                dashboardWindow.Show();
                CloseAllWindowsExcept(dashboardWindow);
            });
        }

        public void NavigateToFinnhub()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var finnhubViewModel = _serviceProvider.GetService<FinnhubViewModel>();
                var finnhubWindow = new FinnhubWindow(finnhubViewModel);
                finnhubWindow.Show();
                CloseAllWindowsExcept(finnhubWindow);
            });
        }

        public void NavigateToBanking()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var bankingViewModel = _serviceProvider.GetService<BankingViewModel>();
                var bankingWindow = new BankingWindow(bankingViewModel);
                bankingWindow.Show();
            });
        }

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
