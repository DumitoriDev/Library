using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private void ToggleFlyoutSetting(object sender, RoutedEventArgs e)
        {
            yourMahAppFlyout.IsOpen = !yourMahAppFlyout.IsOpen;
        }
        public MainWindow()
        {
            DataBaseContext context = new DataBaseContext();

            var tmp = context.Books.First();
            var tmpres = tmp.AuthorId.ToList();
                
            

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            yourMahAppFlyout.IsOpen = !yourMahAppFlyout.IsOpen;
        }
    }
}
