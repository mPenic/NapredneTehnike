using BankovniSustavApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankovniSustavApp
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow(RegisterViewModel registerViewModel)
        {
            InitializeComponent();
            this.DataContext = registerViewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is RegisterViewModel registerViewModel)
            {
                var passwordBox = (PasswordBox)sender;
                registerViewModel.Lozinka = passwordBox.SecurePassword.Copy();
                Console.WriteLine($"PasswordBox changed, SecurePassword updated in ViewModel. Password length: {registerViewModel.Lozinka.Length}");
            }
        }
    }
}
