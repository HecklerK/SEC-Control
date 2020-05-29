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
        DBEntities db = new DBEntities();
        int b = 0;

        public class pavi
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class sece
        {
            public int inn { get; set; }
            public int kpp { get; set; }
            public string name { get; set; }
        }

        public Admin(Login.Us var)
        {
            InitializeComponent();
            db.Database.Connection.ConnectionString = Connect.ConnectionString;
            login.Content = var.login;
        }

        void UpdateSEC()
        {
            int l = list1.SelectedIndex;
            int l2 = list2.SelectedIndex;
            tb_inn.Text = "";
            tb_kpp.Text = "";
            tb_name.Text = "";
            tb_type.Text = "";
            tb_phone.Text = "";
            tb_login.Text = "";
            tb_pass.Text = "";
            tb_pav.Text = "";
            tb_p_n.Text = "";
            list1.Items.Clear();
            var sec = db.SECs
                .AsNoTracking();
            foreach (var s in sec)
            {
                string name = "";
                foreach (var t in db.Types)
                    if (s.type == t.id) name = t.name;
                name += " " + s.name;
                if (s.delete == 1) name += " (Заблокирован)";
                else name += "        ";
                sece Data = new sece
                {
                    inn = s.inn,
                    kpp = s.kpp,
                    name = name
                };
                list1.Items.Add(Data);
            }
            tb_type.Items.Clear();
            var type = db.Types;
            foreach (var t in type)
            {
                tb_type.Items.Add(t.name);
            }
            list1.SelectedIndex = l;
            list2.SelectedIndex = l2;
        }

        void updatePavilion()
        {
            list2.Items.Clear();
            var pav = db.Pavilions
                .Where(a => a.SEC == list1.SelectedIndex + 1);
            foreach (var p in pav)
            {
                pavi Data = new pavi
                {
                    id = p.number,
                    name = p.name
                };
                list2.Items.Add(Data);
            }
        }

        void updateInfoSEC()
        {
            tb_inn.Text = "";
            tb_kpp.Text = "";
            tb_name.Text = "";
            tb_type.Text = "";
            tb_phone.Text = "";
            tb_login.Text = "";
            tb_pass.Text = "";
            tb_pav.Text = "";
            tb_p_n.Text = "";
            var sec = db.SECs
                .Join(db.Types,
                typeID => typeID.type,
                typeN => typeN.id,
                (typeID, typeN) => new { ID = typeID, N = typeN })
                .Where(p => p.ID.id == list1.SelectedIndex + 1);
            foreach (var s in sec)
            {
                tb_inn.Text = s.ID.inn.ToString();
                tb_kpp.Text = s.ID.kpp.ToString();
                tb_name.Text = s.ID.name;
                tb_type.Text = s.N.name;
                tb_phone.Text = s.ID.phone.ToString();
                tb_login.Text = s.ID.login;
                tb_pass.Text = s.ID.pass;
            }
        }

        void updateInfoPav()
        {
            if (list2.SelectedIndex != -1)
            {
                dynamic si = list2.SelectedItem;
                int id = Convert.ToInt32(si.id);
                var pav = db.Pavilions
                        .Where(p => p.SEC == list1.SelectedIndex + 1 && p.number == id)
                        .FirstOrDefault();
                tb_pav.Text = pav.number.ToString();
                tb_p_n.Text = pav.name;
                tb_p_o.Text = pav.comments;
            }
            else
            {
                tb_pav.Text = "";
                tb_p_n.Text = "";
                tb_p_o.Text = "";
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
            l_state.Content = "Создание";
            list1.IsEnabled = false;
            list2.IsEnabled = false;
            tb_inn.Text = "";
            tb_kpp.Text = "";
            tb_name.Text = "";
            tb_type.Text = "";
            tb_phone.Text = "";
            tb_login.Text = "";
            tb_pass.Text = "";
            tb_pav.Text = "";
            tb_p_n.Text = "";
            b = 1;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (b != 1)
                {
                    int l = list1.SelectedIndex;
                    int l2 = list2.SelectedIndex;
                    if (list2.SelectedIndex != -1)
                    {
                        var pav = db.Pavilions
                            .Where(p => p.SEC == list1.SelectedIndex + 1);
                        int[] mas = new int[pav.Count()];
                        int i = 0;
                        foreach (var p in pav)
                        {
                            mas[i] = p.id;
                            i++;
                        }
                        var tmp = mas[list2.SelectedIndex];
                        var pa = db.Pavilions
                            .Where(p => p.id == tmp)
                            .FirstOrDefault();
                        pa.number = Convert.ToInt32(tb_pav.Text);
                        pa.name = tb_p_n.Text;
                        pa.comments = tb_p_o.Text;
                        db.SaveChanges();
                        updatePavilion();
                    }
                    if (list1.SelectedIndex != -1)
                    {
                        var sec = db.SECs
                            .Join(db.Types,
                            typeID => typeID.type,
                            typeN => typeN.id,
                            (typeID, typeN) => new { ID = typeID, N = typeN })
                            .Where(p => p.ID.id == list1.SelectedIndex + 1)
                            .FirstOrDefault();
                        var s = sec;
                        s.ID.inn = Convert.ToInt32(tb_inn.Text);
                        s.ID.kpp = Convert.ToInt32(tb_kpp.Text);
                        s.ID.name = tb_name.Text;
                        foreach (var t in db.Types)
                            if (t.name == tb_type.Text) s.ID.type = t.id;
                        s.ID.phone = Convert.ToInt64(tb_phone.Text);
                        s.ID.login = tb_login.Text;
                        s.ID.pass = tb_pass.Text;
                        db.SaveChanges();
                        UpdateSEC();
                    }
                    list1.SelectedIndex = l;
                    list2.SelectedIndex = l2;
                }
                else
                {
                    if (tb_inn.Text != "" && tb_name.Text != "" && tb_type.Text != "" && tb_login.Text != "" && tb_pass.Text != "" && tb_phone.Text != "")
                    {
                        var t = db.Types
                            .Where(ty => ty.name == tb_type.Text)
                            .FirstOrDefault();
                        SEC s = new SEC
                        {
                            inn = Convert.ToInt32(tb_inn.Text),
                            kpp = Convert.ToInt32(tb_kpp.Text),
                            name = tb_name.Text,
                            type = t.id,
                            login = tb_login.Text,
                            pass = tb_pass.Text,
                            phone = Convert.ToInt64(tb_phone.Text)
                        };
                        var sec = db.SECs;
                        sec.Add(s);
                        db.SaveChanges();
                        UpdateSEC();
                        l_state.Content = "Изменение";
                        list1.IsEnabled = true;
                        list2.IsEnabled = true;
                        b = 0;
                    }
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (list1.SelectedIndex != -1)
            {
                FormCol dialog = new FormCol();
                if (dialog.ShowDialog() == true)
                {
                    var p = db.Pavilions
                        .Where(pa => pa.SEC == list1.SelectedIndex + 1)
                        .Count();
                    var pav = db.Pavilions;
                    p++;
                    for (int i = p; i < p + dialog.number; i++)
                    {
                        Pavilion pa = new Pavilion
                        {
                            number = i,
                            name = "",
                            SEC = list1.SelectedIndex + 1
                        };
                        pav.Add(pa);
                    }
                    db.SaveChanges();
                    updatePavilion();
                }
                else MessageBox.Show("Создание отменено");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            l_state.Content = "Изменение";
            list1.SelectedIndex = -1;
            list2.SelectedIndex = -1;
            list1.IsEnabled = true;
            list2.IsEnabled = true;
            b = 1;
        }

        private void b_gen_Click(object sender, RoutedEventArgs e)
        {
            string rand = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabsdefghijklmnopqrstuvwxyz";
            Random rnd = new Random(DateTime.Now.Millisecond);
            if (tb_inn.Text.Length >= 9 && tb_kpp.Text.Length >= 9)
            {
                tb_login.Text = "";
                tb_pass.Text = "";
                tb_login.Text = tb_inn.Text.Substring(0, 4) + tb_kpp.Text.Substring(5, 4) + rand[Convert.ToInt32(tb_inn.Text.Substring(3, 1)) + 10];
                for (int i = 0; i < 8; i++)
                {
                    tb_pass.Text += rand[rnd.Next(0, rand.Length)];
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (list1.SelectedIndex != -1)
            {
                var sec = db.SECs
                    .FirstOrDefault(p => p.id == list1.SelectedIndex + 1);
                if (sec.delete == 1)
                    sec.delete = 0;
                else
                    sec.delete = 1;
                db.SaveChanges();
                UpdateSEC();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (list2.SelectedIndex != -1)
            {
                var pav = db.Pavilions
                    .Where(p => p.SEC == list1.SelectedIndex + 1);
                int[] mas = new int[pav.Count()];
                int i = 0;
                foreach (var p in pav)
                {
                    mas[i] = p.id;
                    i++;
                }
                var tmp = mas[list2.SelectedIndex];
                var pa = db.Pavilions
                    .Where(p => p.id == tmp)
                    .FirstOrDefault();
                db.Pavilions.Remove(pa);
                db.SaveChanges();
                UpdateSEC();
            }
        }
    }
}
