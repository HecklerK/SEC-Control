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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin(Login.Ad var)
        {
            InitializeComponent();
            login.Content = var.login;
        }

        void UpdateSEC()
        {
            List1.Items.Clear();
            using (var db = new DBEntities())
            {
                var sec = db.SEC;
                foreach (var s in sec)
                {
                    string name = "";
                    if (s.type == 1) name = "ТЦ";
                    else if (s.type == 2) name = "ТРЦ";
                    name += " " + s.name;
                    List1.Items.Add(name);
                }
            }
        }

        void UpdatePavilion()
        {
            list2.Items.Clear();
            using(var db = new DBEntities())
            {
                var pav = db.Pavilion
                    .Where(a => a.SEC == List1.SelectedIndex + 1);
                foreach (var p in pav)
                {
                    if (p.name == null || p.name == "") list2.Items.Add("№ " + p.pavilion1);
                    else list2.Items.Add(p.name + "(№ " + p.pavilion1 +")");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSEC();
        }

        private void List1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePavilion();
        }
    }
}
