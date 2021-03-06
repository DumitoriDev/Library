﻿using System;
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
    /// Логика взаимодействия для DeleteLanguage.xaml
    /// </summary>
    public partial class DeleteLanguage : MetroWindow
    {
        private readonly LanguageRepository _languageRepository = new LanguageRepository();
        private readonly BookRepository _bookRepository = new BookRepository();
        public bool Status = false;
        public DeleteLanguage()
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

                var tmp = this._languageRepository.Get(type => type.Name == this.TextBox.Text);

                if (tmp is null)
                {
                    MessageBox.Show("Язык не удалось найти!", "Error");
                    return;
                }

                if (this._bookRepository.Check(book => book.Language.Name == this.TextBox.Text))
                {
                    MessageBox.Show("Язык привязан к книгам, удаление невозможно", "Error");
                    return;
                }

                this._languageRepository.Delete(tmp.Id);
                this.Status = true;
                this.Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
           
        }
    }
}
