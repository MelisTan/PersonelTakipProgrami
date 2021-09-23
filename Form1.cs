using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// system.data.oledb kütüphanesini ekliyoruz
using System.Data.OleDb;
    

namespace WindowsFormsApp36__PERSONEL_TAKİP_PROGRAMI_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // veri tabanı dosya yolu ve provider nesnesinin eklenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0; Data Source="+ Application.StartupPath+"\\personel.accdb");

        // formlar arası veri aktarımında kullanılacak değişkenler
        public static string tcno, adi, soyadi, yetki;
        // yerel yani yalnızca bu formda geçerli olacak değişkenler
        int hak = 3; bool durum = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (hak != 0)
            {
                baglantim.Open();
                // tablodaki tüm verileri çeken bir sorgu tanımladık
                OleDbCommand eklamesorgusu = new OleDbCommand("select*from kullanicilar", baglantim);
                OleDbDataReader kayitokuma = eklamesorgusu.ExecuteReader();
                // sorgunun sonuçlarını kayitokuma ismindeki datareader da saklıyoruz. bu bilgiler geçici olarak bellekte saklanıyor
                while (kayitokuma.Read() == true)
                {
                    // tabloda herhangi bir bilgi varsa true olarak döner 
                    if (radioButton1.Checked == true)
                    {
                        if (kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString() == textBox2.Text && kayitokuma["yetkisi"].ToString() == radioButton1.Text)
                        {
                            durum = true;
                            tcno = kayitokuma.GetValue(0).ToString();
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide(); // açık olan formu gizliyoruz

                            Form2 frm2 = new Form2();
                            frm2.Show();  // formun aktif edilmesini sağladık
                            break;
                            // artık istediğimiz veri girişi yapıldığı için while döngüsü tekrar tekar çalışmmasın diye break komutuyla döngüden çıkmasını sağlıyoruz

                        }
                    }
                    if (radioButton2.Checked == true)
                    { 
                        if (kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString() == textBox2.Text && kayitokuma["yetkisi"].ToString() == radioButton2.Text)
                        {
                            durum = true;
                            tcno = kayitokuma.GetValue(0).ToString();
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide(); // açık olan formu gizliyoruz

                            Form3 frm3 = new Form3();
                            frm3.Show();  // formun aktif edilmesini sağladık
                            break;
                            // artık istediğimiz veri girişi yapıldığı için while döngüsü tekrar tekar çalışmmasın diye break komutuyla döngüden çıkmasını sağlıyoruz

                        }
                    }
                }

                if (durum == false)
                {
                    hak--;
                    baglantim.Close();
                }

            }
            label5.Text = hak.ToString();
            if (hak == 0)
            {
                button1.Enabled = false;
                MessageBox.Show("Giriş hakkı kalmadı!", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // MessageBoxButtons.OK -- yalnızca tamam butonu görünsün   MessageBoxIcon.Error-- hata ikonu görünsün
                this.Close();
            }
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Kullanıcı Girişi";
            // enter a basıldığında button1 e basılmış gibi olacak
            this.AcceptButton = button1;
            // esc ye basıldığı zaman da button2 ye basılmış gibi
            this.CancelButton = button2;
            label5.Text = Convert.ToString(hak);
            radioButton1.Checked = true;
            // form ekranın merkezinde gelsin
            this.StartPosition = FormStartPosition.CenterScreen;
            // tam ekran yapma ve küçültmeyi devre dışı bıraktık
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
    }
}
