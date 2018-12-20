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
    /// Логика взаимодействия для AddNewType.xaml
    /// </summary>
    public partial class AddNewType : MetroWindow
    {
        private readonly TypeRepository _typeRepository = new TypeRepository();

            public bool Status = true;

        public AddNewType()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox.Text))
            {
                MessageBox.Show("Данные пусты!","Error");
                return;
            }

            var tmp = new LibraryClass.Type{Name = this.TextBox.Text };

            if (this._typeRepository.Get(type => type.Name == this.TextBox.Text) is null)
            {
                this._typeRepository.Add(tmp);
                this.Status = true;
            }
            else
            {
                MessageBox.Show("Такой тип уже имеется!", "Error");
            }
            
            this.Close();

        }
    }
}
