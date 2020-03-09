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
        public Admin(Login.Us var)
        {
            InitializeComponent();
            login.Content = var.login;
        }

        void UpdateSEC()
        {
            list1.Items.Clear();
            using (var db = new DBEntities())
            {
                var sec = db.SEC;
                foreach (var s in sec)
                {
                    string name = "";
                    if (s.type == 1) name = "ТЦ";
                    else if (s.type == 2) name = "ТРЦ";
                    name += " " + s.name;
                    list1.Items.Add(name);
                }
                var type = db.Type;
                foreach (var t in type)
                {
                    tb_type.Items.Add(t.name);
                }
            }
        }

        void updatePavilion()
        {
            list2.Items.Clear();
            using(var db = new DBEntities())
            {
                var pav = db.Pavilion
                    .Where(a => a.SEC == list1.SelectedIndex + 1);
                foreach (var p in pav)
                {
                    list2.Items.Add(p.number);
                }
            }
        }

        void updateInfoSEC()
        {
            using (var db = new DBEntities())
            {
                var sec = db.SEC
                    .Join(db.Type,
                    typeID => typeID.type,
                    typeN => typeN.id,
                    (typeID, typeN) => new { ID = typeID, N = typeN })
                    .Where(p => p.ID.id == list1.SelectedIndex + 1);
                foreach (var s in sec)
                {
                    tb_inn.Text = s.ID.inn.ToString();
                    tb_name.Text = s.ID.name;
                    tb_type.Text = s.N.name;
                    tb_phone.Text = s.ID.phone.ToString();
                    tb_login.Text = s.ID.login;
                    tb_pass.Text = s.ID.pass;
                }
            }
        }

        void updateInfoPav()
        {
            if (list2.SelectedIndex != -1)
                using (var db = new DBEntities())
                {
                    var pav = db.Pavilion
                        .Where(p => p.SEC == list1.SelectedIndex + 1 && p.number.ToString() == list2.SelectedItem.ToString());
                    foreach (var p in pav)
                    {
                        tb_pav.Text = p.number.ToString();
                        tb_p_n.Text = p.name; 
                    }
                }
            else
            {
                tb_pav.Text = "";
                tb_p_n.Text = "";
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
            updatePavilion();
            updateInfoSEC();
        }

        private void list2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateInfoPav();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tb_inn.Text = "";
            tb_name.Text = "";
            tb_type.Text = "";
            tb_phone.Text = "";
            tb_login.Text = "";
            tb_pass.Text = "";
            tb_pav.Text = "";
            tb_p_n.Text = "";
            but.Content = "Отмена";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (var db = new DBEntities())
            {
                if (but.Content != "Отмена")
                {
                    if (list1.SelectedIndex != -1)
                    {
                        var sec = db.SEC
                            .Join(db.Type,
                            typeID => typeID.type,
                            typeN => typeN.id,
                            (typeID, typeN) => new { ID = typeID, N = typeN })
                            .Where(p => p.ID.id == list1.SelectedIndex + 1)
                            .FirstOrDefault();
                        var s = sec;
                        s.ID.inn = Convert.ToInt32(tb_inn.Text);
                        s.ID.name = tb_name.Text;
                        s.N.name = tb_type.Text;
                        s.ID.phone = Convert.ToInt32(tb_phone.Text);
                        s.ID.login = tb_login.Text;
                        s.ID.pass = tb_pass.Text;
                        db.SaveChanges();
                    }
                    if (list2.SelectedIndex != -1)
                    {
                        var pav = db.Pavilion
                            .Where(p => p.SEC == list1.SelectedIndex + 1 && p.number.ToString() == list2.SelectedItem.ToString())
                            .FirstOrDefault();
                        var pa = pav;
                        pa.number = Convert.ToInt32(tb_pav.Text);
                        pa.name = tb_p_n.Text;
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (tb_inn.Text != "" && tb_name.Text != "" && tb_type.Text != "" && tb_login.Text != "" && tb_pass.Text != "")
                    {
                        var t = db.Type
                            .Where(ty => ty.name == tb_type.Text)
                            .FirstOrDefault();
                        SEC s = new SEC
                        {
                            inn = Convert.ToInt32(tb_inn.Text),
                            name = tb_name.Text,
                            type = t.id,
                            login = tb_login.Text,
                            pass = tb_pass.Text,
                            phone = Convert.ToInt32(tb_phone.Text)
                        };
                        var sec = db.SEC;
                        sec.Add(s);
                        db.SaveChanges();
                        UpdateSEC();
                        but.Content = "Добавить админа";
                    }
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
