using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BankovniSustavApp.Repositories;
using BankovniSustavApp.DataAccess;
using BankovniSustavApp.Models;
using BankovniSustavApp.Services;
using BankovniSustavApp.ViewModels;
using BankovniSustavApp.Helpers;

namespace BankovniSustavApp
{
    
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<DatabaseHelper>();
            services.AddSingleton<IGenericRepository<Korisnik>, KorisnikRepository>();
            services.AddSingleton<IGenericRepository<Logovi>, LogoviRepository>();
            services.AddSingleton<IAccountRepository<Racuni>, RacuniRepository>();
            services.AddSingleton<IGenericRepository<Transakcije>, TransakcijeRepository>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddSingleton<INavigationService>(provider => new NavigationService(provider));
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddSingleton<MainWindow>();
            // Add other services and repositories as needed
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var iniPath = Path.Combine(appDataPath, "BankovniSustavApp", "settings.ini");
            var settingsManager = new SettingsManager(iniPath);

            try
            {

                var mainWindow = ServiceProvider.GetService<MainWindow>();
                if (mainWindow != null)
                {
                    mainWindow.Show();
                }
                else
                {
                    // Handle the error appropriately
                }

                this.ShutdownMode = ShutdownMode.OnLastWindowClose;
            }
            catch (Exception ex)
            {
                // Log the details of the exception.
                Log($"An exception occurred during application startup: {ex.Message}");
                Log(ex.StackTrace);

                // Optionally, display an error message to the user.
                MessageBox.Show("An error occurred while starting the application. Please see the log file for details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Terminate the application if it cannot start correctly.
                Application.Current.Shutdown();
            }
            void Log(string message)
            {
                
                Console.WriteLine(message);
            }
        }

    }

}
