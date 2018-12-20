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
using LibraryClass;
using MahApps.Metro.Controls;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для AddNewLanguage.xaml
    /// </summary>
    public partial class AddNewLanguage : MetroWindow
    {

        private readonly LanguageRepository _languageRepository = new LanguageRepository();
        public bool Status = false;
        public AddNewLanguage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TextBox.Text))
                {
                    MessageBox.Show("Вы ввели пустые данные!", "Error");
                    return;
                }

                if (this._languageRepository.Check(lang => lang.Name != this.TextBox.Text))
                {
                    this._languageRepository.Add(new Language { Name = this.TextBox.Text });
                    this.Status = true;
                }
                else
                {
                    MessageBox.Show("Такой язык уже имеется!", "Error");
                }

              
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }
    }
}
