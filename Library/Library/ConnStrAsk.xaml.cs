using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using LibraryClass;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Library
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Логика взаимодействия для ConnStrAsk.xaml
    /// </summary>
    public partial class ConnStrAsk : MetroWindow
    {
        private bool _isConnected = false;
        private string _connExc = string.Empty;
        private string _connStr = string.Empty;
        private readonly XmlSerializer _formatter = new XmlSerializer(typeof(LoginData));

        public ConnStrAsk()
        {
            InitializeComponent();
            this.Image.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("img/Welcome.png")));
            this.LoadData();


        }

        private void LoadData()
        {
            try
            {
                var file = new FileInfo("data.xml");
                if (!file.Exists) return;
                using (var fs = new FileStream("data.xml", FileMode.Open))
                {
                    var deserialize = (LoginData)_formatter.Deserialize(fs);
                    this.DBNameTextBox.Text = deserialize.DataBaseName;
                    this.LoginTextBox.Text = deserialize.Login;
                    this.ServerNameTextBox.Text = deserialize.ServerName;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Error");
            }
            

        }

        private async void DBConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (UseDefaultConnStr.IsChecked == true)
                {
                    _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    
                }
                else
                {
                    if (this.ServerNameTextBox.Text == "" || this.LoginTextBox.Text == "" || this.DBNameTextBox.Text == "" || DBNameTextBox.Text == "")
                    {
                        MessageBox.Show("Не все поля заполнены!" + _connExc, "Ошибка подключения к БД", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }

                    _connStr = CollectConnStr(ServerNameTextBox.Text, LoginTextBox.Text, DBPasswordPwdBox.Text, DBNameTextBox.Text);


                }

                _isConnected = await CheckConn(_connStr);
                if (_isConnected == false)
                {
                    MessageBox.Show("Подключение не удалось. Ошибка: " + _connExc, "Ошибка подключения к БД", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                    DataBaseContext.connStr = this._connStr;
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }
        
        private async Task<bool> CheckConn(string connStr)
        {
            var status = true;
            this.ProgressRing.IsActive = true;
            this.Canvas.IsEnabled = false;
            await Task.Factory.StartNew(() =>
            {
                using (var sqlConnection = new SqlConnection(connStr))
                {
                    Thread.Sleep(1000);
                    try
                    {
                        sqlConnection.Open();
                        
                    }
                    catch (Exception exc)
                    {
                        _connExc = exc.Message;
                        status = false;

                    }
                }

            });
            this.Canvas.IsEnabled = true;

            this.ProgressRing.IsActive = false;
            return status;
        }

        private string CollectConnStr(string serverName, string login, string password, string dbName)
        {
            var connStr = new StringBuilder();
            connStr.Append("user id=").Append(login).Append(";");
            connStr.Append("pwd=").Append(password).Append(";");
            connStr.Append("Data Source=").Append(serverName).Append(";");
            connStr.Append("Initial Catalog=").Append(dbName).Append(";");

            connStr.Append("Integrated Security=False;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=false;MultipleActiveResultSets=True");

            return connStr.ToString();
        }

        private void ConnStrAsk_OnClosed(object sender, EventArgs e)
        {
            try
            {
                var saveDataIsChecked = this.SaveData.IsChecked;
                if (saveDataIsChecked == null || !saveDataIsChecked.Value) return;
                var data = new LoginData
                {
                    DataBaseName = this.DBNameTextBox.Text,
                    Login = this.LoginTextBox.Text,
                    ServerName = this.ServerNameTextBox.Text
                };


                using (var fs = new FileStream("data.xml", FileMode.OpenOrCreate))
                {
                    this._formatter.Serialize(fs, data);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
            
        }
    }
}
