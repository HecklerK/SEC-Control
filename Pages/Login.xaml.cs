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
        public Login()
        {
            InitializeComponent();
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
            if (login.Text.Length > 0)
            {
                if (pass.Password.Length > 0)
                {
                    using (var db = new DBEntities())
                    {
                        var user = db.Users
                            .AsNoTracking()
                            .FirstOrDefault(u => u.login == login.Text && u.password == pass.Password);
                        if (user == null)
                        {
                            MessageBox.Show("Пользователь не найден");
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
                }
                else MessageBox.Show("Введите пароль");
            }
            else MessageBox.Show("Введите логин");
        }
    }
}
