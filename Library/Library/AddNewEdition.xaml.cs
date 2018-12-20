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
    /// Логика взаимодействия для AddNewEdition.xaml
    /// </summary>
    public partial class AddNewEdition : MetroWindow
    {
        private  readonly EditionRepository _editionRepository = new EditionRepository();
        public bool Status = false;
        public AddNewEdition()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(this.TextBox.Text))
                {
                    MessageBox.Show("Данные пусты!", "Error");
                    return;
                }

               
                if (this._editionRepository.Get(edit => edit.Name == this.TextBox.Text) is null)
                {
                    this._editionRepository.Add(new Edition { Name = this.TextBox.Text });
                    this.Status = true;
                }

                else
                {
                    MessageBox.Show("Такое издательство уже имеется!", "Error");
                }

                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Erorr");
            }
           
        }
    }
}
