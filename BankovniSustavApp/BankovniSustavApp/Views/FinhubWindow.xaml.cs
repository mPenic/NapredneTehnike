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
using BankovniSustavApp.ViewModels;

namespace BankovniSustavApp.Views
{
    public partial class FinnhubWindow : Window
    {
        public FinnhubWindow(FinnhubViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }

}
