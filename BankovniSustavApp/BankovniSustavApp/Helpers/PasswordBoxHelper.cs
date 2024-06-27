using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace BankovniSustavApp.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(SecureString),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(null, OnBoundPasswordChanged));

        public static SecureString GetBoundPassword(DependencyObject dp)
        {
            return (SecureString)dp.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject dp, SecureString value)
        {
            dp.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (!(bool)GetIsUpdating(passwordBox))
                {
                    if (e.NewValue is SecureString newValue)
                    {
                        passwordBox.Password = newValue.ToInsecureString();
                    }
                }
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                SetIsUpdating(passwordBox, true);
                SetBoundPassword(passwordBox, passwordBox.SecurePassword.Copy());
                SetIsUpdating(passwordBox, false);
            }
        }

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached(
               "IsUpdating",
               typeof(bool),
               typeof(PasswordBoxHelper),
               new PropertyMetadata(false));

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        public static string ToInsecureString(this SecureString secureString)
        {
            if (secureString == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString Copy(this SecureString secureString)
        {
            if (secureString == null)
            {
                throw new ArgumentNullException(nameof(secureString));
            }

            SecureString copy = new SecureString();
            foreach (char c in secureString.ToInsecureString())
            {
                copy.AppendChar(c);
            }
            copy.MakeReadOnly();
            return copy;
        }

    }
}