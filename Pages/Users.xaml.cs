using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data.Entity.Validation;
namespace SEC_Control.Pages
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public class pavi
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        string l;
        SEC sec;
        string type;
        public Users(Login.Us var)
        {
            InitializeComponent();
            l = var.login;
            using (var db = new DBEntities())
            {
                sec = db.SECs
                    .FirstOrDefault(p => p.login == l);
                var t = db.Types
                    .FirstOrDefault(p => p.id == sec.type);
                type = t.name;
            }
            login.Content = type + " " + sec.name;
            updateInfoSEC();
            updatePavilion();
        }

        void updateInfoSEC()
        {
            tb_inn.Text = sec.inn.ToString();
            tb_kpp.Text = sec.kpp.ToString();
            tb_phone.Text = sec.phone.ToString();
        }

        void updateInfoPav()
        {
            if (list.SelectedIndex != -1)
                using (var db = new DBEntities())
                {
                    dynamic si = list.SelectedItem;
                    int id = Convert.ToInt32(si.id);
                    var pav = db.Pavilions
                            .Where(p => p.SEC == sec.id && p.number == id)
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

        void updatePavilion()
        {
            list.Items.Clear();
            using (var db = new DBEntities())
            {
                var pav = db.Pavilions
                    .Where(a => a.SEC == sec.id);
                foreach (var p in pav)
                {
                    pavi Data = new pavi
                    {
                        id = p.number,
                        name = p.name
                    };
                    list.Items.Add(Data);
                }
            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateInfoPav();
        }

        private void open_plan_Click(object sender, RoutedEventArgs e)
        {
            string url = "ftp://ftpplans:CH5pjX5b@hecklerk.online/" + sec.login + sec.format;
            Process.Start(url);
        }

        void fileLoad(FileInfo fi, FtpWebRequest reqFTP)
        {
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // Файловый поток
            FileStream fs = fi.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                // Читаем из потока по 2 кбайт
                contentLen = fs.Read(buff, 0, buffLength);
                // Пока файл не кончится
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // Закрываем потоки
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message, "Ошибка");

            }
        }

        private void load_plan_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".jpg";
            fd.Filter = "Изображение (*.BMP, *.JPG, *.GIF, *.PNG)|*.bmp; *.jpg; *.gif; *.png";
            var result = fd.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                    FileInfo fi = new FileInfo(fd.FileName);
                    string ext = fi.Extension;
                    FtpWebRequest reqFTP;
                    // Учетная запись
                    if (sec.format != null)
                    {
                        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://hecklerk.online/" + sec.login + sec.format));
                        reqFTP.Credentials = new NetworkCredential("ftpplans", "CH5pjX5b");
                        reqFTP.KeepAlive = false;
                        reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                        FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    }
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://hecklerk.online/" + sec.login + ext));
                    reqFTP.Credentials = new NetworkCredential("ftpplans", "CH5pjX5b");
                    reqFTP.KeepAlive = false;
                    // Задаем команду на закачку
                    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                    // Тип передачи файла
                    reqFTP.UseBinary = true;
                    // Сообщаем серверу о размере файла
                    reqFTP.ContentLength = fi.Length;
                    Task.Run(() => fileLoad(fi, reqFTP));
                    using (var db = new DBEntities())
                    {
                        sec = db.SECs
                            .FirstOrDefault(p => p.id == sec.id);
                        sec.format = ext;
                        db.SaveChanges();
                    };
                    updateInfoSEC();
                    break;
                case DialogResult.Cancel:
                default:
                    break;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DBEntities())
                {
                    sec = db.SECs
                        .FirstOrDefault(p => p.id == sec.id);
                    sec.phone = tb_phone.Text;
                    db.SaveChanges();
                    if (list.SelectedIndex != -1)
                    {
                        var pav = db.Pavilions
                            .Where(p => p.SEC == sec.id);
                        int[] mas = new int[pav.Count()];
                        int i = 0;
                        foreach (var p in pav)
                        {
                            mas[i] = p.id;
                            i++;
                        }
                        var tmp = mas[list.SelectedIndex];
                        var pa = db.Pavilions
                            .Where(p => p.id == tmp)
                            .FirstOrDefault();
                        pa.number = Convert.ToInt32(tb_pav.Text);
                        pa.name = tb_p_n.Text;
                        pa.comments = tb_p_o.Text;
                        db.SaveChanges();
                        updatePavilion();
                    }
                }
            }
            catch(Exception a)
            {
                System.Windows.MessageBox.Show(a.Message);
            }
        }
    }
}
