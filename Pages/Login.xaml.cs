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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SEC_Control.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        DBEntities db = new DBEntities();

        public Login()
        {
            InitializeComponent();
            db.Database.Connection.ConnectionString = Connect.ConnectionString;
        }
        public class Us
        {
            public string login;
            public Us(string a)
            {
                login = a;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (login.Text.Length > 0)
                {
                    if (pass.Password.Length > 0)
                    {
                        var user = db.Users
                                .AsNoTracking()
                                .FirstOrDefault(u => u.login == login.Text && u.password == pass.Password);
                        if (user == null)
                        {
                            var sec = db.SECs
                                .AsNoTracking()
                                .FirstOrDefault(s => s.login == login.Text && s.pass == pass.Password);
                            if (sec == null)
                            {
                                MessageBox.Show("Пользователь не найден");
                                return;
                            }
                            if (sec.delete == 1)
                            {
                                MessageBox.Show("Пользователь заблокирован");
                                return;
                            }
                            Us c = new Us(sec.login);
                            NavigationService.Navigate(new Users(c));
                            return;
                        }
                        Us cs = new Us(user.login);
                        switch (user.type)
                        {
                            case 0:
                                NavigationService.Navigate(new Admin(cs));
                                break;
                            default:
                                break;
                        }
                    }
                    else MessageBox.Show("Введите пароль");
                }
                else MessageBox.Show("Введите логин");
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }

        private void login_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.c = c_login.IsChecked.Value;
            if (c_login.IsChecked == true && login.Text != "")
            {
                Properties.Settings.Default.Login = login.Text;
            }
            else
            {
                Properties.Settings.Default.Login = "";
            }
            Properties.Settings.Default.Save();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            c_login.IsChecked = Properties.Settings.Default.c;
            if (c_login.IsChecked == true) login.Text = Properties.Settings.Default.Login;
        }
    }
}
