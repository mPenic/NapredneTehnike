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

namespace BankovniSustavApp.Views
{
    public partial class BankingWindow : Window
    {
        public BankingWindow(BankingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
