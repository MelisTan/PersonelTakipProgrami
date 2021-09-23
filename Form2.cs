using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// system.data.oledb kütüphanesinin tanımlanması. veri tabanı kullanacağımız için bu kütüphaneyi tanımlamalıyız
using System.Data.OleDb;
// System.Text.RegularExpressions kütüphanesinin eklenmesi. (Regex)
// güvenli parola oluşturmayı sağlayan hazır kodlar içinde bulunduruyor
using System.Text.RegularExpressions;
// giriş çıkış işlemlerine ilişkin kütüphanenin tanımlanması
// klasör işlemleri için kullanaacağız
using System.IO;
using System.Xml.Serialization;
using System.CodeDom.Compiler;

namespace WindowsFormsApp36__PERSONEL_TAKİP_PROGRAMI_
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        // veri tabanı dosya yolu ve procider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0; Data Source=personel.accdb");

        private void kullanicilari_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter kullaniciları_listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO], ad AS[ADI],soyad AS[SOYADI]," +
                    "yetkisi AS[YETKİ], kullaniciadi AS[KULLANICI ADI], parola AS[PAROLA] from kullanicilar Order by ad ASC", baglantim);
                DataSet dshafiza = new DataSet();
                kullaniciları_listele.Fill(dshafiza); // dshafiza alanını sorgunun sonuçlarıyla dooldurduk
                dataGridView1.DataSource = dshafiza.Tables[0]; // sorgunun sonucunda gelen ilk tabloyu 
                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();

            }
        }

        private void personelleri_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleri_listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO], ad AS[ADI],soyad AS[SOYADI]," +
                    "cinsiyet AS[CİNSİYETİ], mezuniyet AS[MEZUNİYETİ],dogumtarihi AS[DOĞUM TARİHİ], gorevi AS[GÖRECİ], gorevyeri AS[GÖREV YERİ]," +
                    "maasi AS[MAAŞI] from personeller", baglantim);

                DataSet dshafiza = new DataSet();
                personelleri_listele.Fill(dshafiza); // dshafiza alanını sorgunun sonuçlarıyla doldurduk
                dataGridView2.DataSource = dshafiza.Tables[0]; // sorgunun sonucunda gelen ilk tabloyu 
                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();

            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            //FORM2 AYARLARI
            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // resmi picturebox a göre ayarla


            try
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\" + Form1.tcno + ".jpg");
            }
            catch (Exception)
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok.png");

            }

            //KULLANICI İŞLEMLERİ SEKMESİ
            this.Text = "YÖNETİCİ İŞLEMLERİ";
            label12.ForeColor = Color.DarkRed;
            label12.Text = Form1.adi + Form1.soyadi;
            textBox1.MaxLength = 11;
            textBox5.MaxLength = 8;
            toolTip1.SetToolTip(this.textBox1, "TC Kimlik no 11 karakter olmalı!"); // üzerine mouse gelince uyarı veriyor
            radioButton1.Checked = true;
            // ad ve soyad kısmını büyük harf yapıyoruz
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            textBox6.MaxLength = 10;
            textBox7.MaxLength = 10;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            kullanicilari_goster();

            // PERSONEL İŞLEMLERİ SEKMESİ
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Width = 100;
            pictureBox2.Height = 100;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            maskedTextBox1.Mask = "00000000000"; // zorunlu rakam için 0 yazıyoruz
            maskedTextBox2.Mask = "LL????????????????????"; //  LL zorunlu iki harf
            maskedTextBox3.Mask = "LL????????????????????";
            maskedTextBox4.Mask = "0000";  //1000-10000 arasında maaş 
            maskedTextBox4.Text = "0";
            maskedTextBox2.Text.ToUpper();
            maskedTextBox3.Text.ToUpper();

            comboBox1.Items.Add("ilköğretim");
            comboBox1.Items.Add("ortsöğretim");
            comboBox1.Items.Add("lise");
            comboBox1.Items.Add("üniversite");

            comboBox2.Items.Add("yönetici"); comboBox2.Items.Add("memur"); comboBox2.Items.Add("şoför");
            comboBox2.Items.Add("işçi");

            comboBox3.Items.Add("arge"); comboBox3.Items.Add("bilgi-işlem");
            comboBox3.Items.Add("muhasebe"); comboBox3.Items.Add("üretim");
            comboBox3.Items.Add("paketleme"); comboBox3.Items.Add("nakliye");

            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));

            dateTimePicker1.MinDate = new DateTime(1960, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(yil - 18, ay, gun);
            dateTimePicker1.Format = DateTimePickerFormat.Short;

            radioButton3.Checked = true;
            personelleri_goster();  




        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 11)
            {
                errorProvider1.SetError(textBox1, "TC Kimlik non 11 karakter olmalı");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // klavyeden her tuşa bastığımızda keypress olayı tetikleniyor
            // ascii karakterlerini kullanıyoruz
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
            {
                //klavyeden basılan tuşun ascii karakterini buluyoruz
                // basılan tuş 48 ve 57 arasında yani rakamlar veya backspace tuşu
                e.Handled = false;
                // bu tuşlara izin veriyoruz

                // klavyeden sadece sayıya basılmasına izin veriyoruz
            }
            else
            {
                e.Handled = true;
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
            {
                //char.IsControl backspace char.isSeparator boşluk
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
            {
                //char.IsControl backspace char.isSeparator boşluk
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length != 8)
            {
                errorProvider1.SetError(textBox5, "kullanıcıadı 8 karakter olmalı!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsDigit(e.KeyChar) == true)
            {
                //char.isdigit sayı
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        int parola_skoru = 0;
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string parola_seviyesi = "";
            int kucukharf_skoru = 0;
            int buyukharf_skoru = 0;
            int rakam_skoru = 0;
            int sembol_skoru = 0;
            string sifre = textBox6.Text;

            // regex kütüphanesi ingilizce karakterleri baz aldığından , türkçe karakterlerde sorun yaşamamak için 
            // şifre string ifadesindeki türkçe karakterleri ingilizce karakterlere dönüştürmemiz gerekiyor

            string dueltilmis_sifre = "";
            dueltilmis_sifre = sifre;
            dueltilmis_sifre = dueltilmis_sifre.Replace('İ', 'I');
            dueltilmis_sifre = dueltilmis_sifre.Replace('Ç', 'C');
            dueltilmis_sifre = dueltilmis_sifre.Replace('ç', 'c');
            dueltilmis_sifre = dueltilmis_sifre.Replace('Ş', 'S');
            dueltilmis_sifre = dueltilmis_sifre.Replace('ş', 's');
            dueltilmis_sifre = dueltilmis_sifre.Replace('Ğ', 'G');
            dueltilmis_sifre = dueltilmis_sifre.Replace('ğ', 'g');
            dueltilmis_sifre = dueltilmis_sifre.Replace('Ü', 'U');
            dueltilmis_sifre = dueltilmis_sifre.Replace('ü', 'u');
            dueltilmis_sifre = dueltilmis_sifre.Replace('Ö', 'O');
            dueltilmis_sifre = dueltilmis_sifre.Replace('ö', 'o');
            dueltilmis_sifre = dueltilmis_sifre.Replace('ı', 'i');

            if (sifre != dueltilmis_sifre)
            {
                sifre = dueltilmis_sifre;
                textBox6.Text = sifre;
                MessageBox.Show("Paroladaki türkçe karakterler ingilizce karakterlere dönüştürülmüştür");

            }

            // 1 küçük harf 10 puan , 2 ve üzeri 20 puan
            int az_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;

            //  Regex.Replace(sifre, "[a-z]", "").Length; küçük harf olanlara boş değer atıyoruz böylece küçük harf olmayanların sayısını buluyoruz
            // bu sayıyı şifrenin toplam karakter sayısından çıkardığımızda da küçük harf sayısı kalıyor
            kucukharf_skoru = Math.Min(2, az_karakter_sayisi) * 10;

            // 1 büyük harf 10 puan , 2 ve üzeri 20 puan
            int AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;
            buyukharf_skoru = Math.Min(2, AZ_karakter_sayisi) * 10;

            // 1 rakam 10 puan , 2 ve üzeri 20 puan
            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length;
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10;

            // 1 sembol 10 puan ,2 ve üzeri 20 puan
            int sembol_sayısı = sifre.Length - (az_karakter_sayisi + AZ_karakter_sayisi + rakam_sayisi);
            sembol_skoru = Math.Min(2, sembol_sayısı) * 10;


            parola_skoru = kucukharf_skoru + buyukharf_skoru + rakam_skoru + sembol_skoru;
            if (sifre.Length == 9)
            {
                parola_skoru += 10;
            }
            else if (sifre.Length == 10)
            {
                parola_skoru += 20;
            }

            if (kucukharf_skoru == 0 || buyukharf_skoru == 0 || rakam_skoru == 0 || sembol_skoru == 0)
            {
                label22.Text = "Mutlaka Küçük harf , büyük harf , rakam ve sembol kullanmalısın !";
            }
            if (kucukharf_skoru != 0 && buyukharf_skoru != 0 && rakam_skoru != 0 && sembol_skoru != 0)
            {
                label22.Text = "";
            }

            if (parola_skoru < 70)
            {
                parola_seviyesi = "kabul edilemez!";
            }
            else if (parola_skoru == 70 || parola_skoru == 80)
            {
                parola_seviyesi = "Güçlü";
            }
            else if (parola_skoru == 90 || parola_skoru == 100)
            {
                parola_seviyesi = "Çok Güçlü";
            }
            label9.Text = "%" + Convert.ToString(parola_skoru);
            label10.Text = parola_seviyesi;
            progressBar1.Value = parola_skoru;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != textBox6.Text)
            {
                errorProvider1.SetError(textBox7, "Parola tekrarı eşleşmiyor!");
            }
            else
                errorProvider1.Clear();
        }

        private void topPage1_temizle()
        {
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox5.Clear();
            textBox6.Clear(); textBox7.Clear();
        }
        private void topPage2_temizle()
        {
            pictureBox2.Image = null; maskedTextBox1.Clear(); maskedTextBox2.Clear();
            maskedTextBox3.Clear(); maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1; comboBox2.SelectedIndex = -1; comboBox3.SelectedIndex = -1;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            string yetki = "";
            bool kayit_kontrol = false;


            // tc kimlik no ya girilen değeer ile acces tablosundaki değerim eşleşip eşleşmediğime baktık
            baglantim.Open();
            OleDbCommand selectsorgusu = new OleDbCommand("select*from kullanicilar where tcno= '" + textBox1.Text + "'", baglantim);
            OleDbDataReader kayitokuma = selectsorgusu.ExecuteReader();  // sorgunun sonuçlarını buraya aktarıyoruz

            while (kayitokuma.Read())
            {
                kayit_kontrol = true;
                break;
            }
            baglantim.Close();

            if (kayit_kontrol == false)
            {
                // TC kimlik no kontrolu 

                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                {
                    label1.ForeColor = Color.Red;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                }

                // adı veri kontrolu
                if (textBox2.Text.Length < 2 || textBox2.Text == "")
                {
                    label2.ForeColor = Color.Red;
                }
                else
                {
                    label2.ForeColor = Color.Black;
                }

                // soyadı veri kontrolu
                if (textBox3.Text.Length < 2 || textBox3.Text == "")
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }

                // kullanıcı adı veri kontrolu
                if (textBox5.Text.Length != 8 || textBox1.Text == "")
                {
                    label5.ForeColor = Color.Red;
                }
                else
                {
                    label5.ForeColor = Color.Black;
                }

                // parola veri kontrolu
                if (textBox6.Text == "" || parola_skoru < 70)
                {
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    label6.ForeColor = Color.Black;
                }

                // parola tekrar veri kontrolu
                if (textBox7.Text == "" || textBox7.Text != textBox6.Text)
                {
                    label7.ForeColor = Color.Red;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                }

                if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text != "" &&
                    textBox2.Text.Length > 1 && textBox3.Text != "" && textBox3.Text.Length > 1 &&
                    textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" &&
                    textBox7.Text == textBox6.Text && parola_skoru >= 70)
                {
                    if (radioButton1.Checked == true)
                    {
                        yetki = radioButton1.Text;
                    }
                    else if (radioButton2.Checked)
                    {
                        yetki = radioButton2.Text;
                    }

                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into kullanicilar values ('" + textBox1.Text + "', " +
                            "'" + textBox2.Text + "','" + textBox3.Text + "','" + yetki + "','" + textBox5.Text + "','" + textBox6.Text + "')", baglantim);

                        eklekomutu.ExecuteNonQuery(); // eklekomutu isimli sorguunun sonuçlarını acces tablosuna işle
                        baglantim.Close();
                        MessageBox.Show("yeni kullanıcı kaydı oluşturuldu", "SKY personel takip programı",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        topPage1_temizle();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message);
                        baglantim.Close();

                    }

                }
                else
                {
                    MessageBox.Show("yazı rengi kırmızı olan alanları yeniden gözden geçiriniz", "SKY personel takip programı",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Girilen TC kimlik numarası daha önceden kayıtlıdır", "SKY personel takip programı",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;

            if (textBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgusu = new OleDbCommand("select* from kullanicilar where tcno ='" + textBox1.Text + "'", baglantim);

                OleDbDataReader kayitokuma = selectsorgusu.ExecuteReader();

                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    textBox2.Text = kayitokuma.GetValue(1).ToString();
                    textBox3.Text = kayitokuma.GetValue(2).ToString();
                    if (kayitokuma.GetValue(3).ToString() == "Yönetici")
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                    }
                    textBox5.Text = kayitokuma.GetValue(4).ToString();
                    textBox6.Text = kayitokuma.GetValue(5).ToString();
                    textBox7.Text = kayitokuma.GetValue(5).ToString();

                    break;
                }

                if (kayit_arama_durumu == false)
                {
                    MessageBox.Show("Aranan kayıt bulunamadı!", "SKY personel takip programı"
                        , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();
                }
            }

            else
            {
                MessageBox.Show("Lütfen 11 haneli bir TC kimlik no giriniz!", "SKY personel takip programı",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage1_temizle();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string yetki = "";
           
                // TC kimlik no kontrolu 

                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                {
                    label1.ForeColor = Color.Red;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                }

                // adı veri kontrolu
                if (textBox2.Text.Length < 2 || textBox2.Text == "")
                {
                    label2.ForeColor = Color.Red;
                }
                else
                {
                    label2.ForeColor = Color.Black;
                }

                // soyadı veri kontrolu
                if (textBox3.Text.Length < 2 || textBox3.Text == "")
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }

                // kullanıcı adı veri kontrolu
                if (textBox5.Text.Length != 8 || textBox1.Text == "")
                {
                    label5.ForeColor = Color.Red;
                }
                else
                {
                    label5.ForeColor = Color.Black;
                }

                // parola veri kontrolu
                if (textBox6.Text == "" || parola_skoru < 70)
                {
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    label6.ForeColor = Color.Black;
                }

                // parola tekrar veri kontrolu
                if (textBox7.Text == "" || textBox7.Text != textBox6.Text)
                {
                    label7.ForeColor = Color.Red;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                }

                if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text != "" &&
                    textBox2.Text.Length > 1 && textBox3.Text != "" && textBox3.Text.Length > 1 &&
                    textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" &&
                    textBox7.Text == textBox6.Text && parola_skoru >= 70)
                {
                    if (radioButton1.Checked == true)
                    {
                        yetki = radioButton1.Text;
                    }
                    else if (radioButton2.Checked)
                    {
                        yetki = radioButton2.Text;
                    }

                    try
                    {
                        baglantim.Open();
                    OleDbCommand guncellekomutu = new OleDbCommand("update kullanicilar set ad ='" + textBox2.Text + "', soyad = '" + textBox3.Text + "'," +
                                          "yetkisi = '" + yetki + "',kullaniciadi= '" + textBox5.Text + "',parola = '" + textBox6.Text + "'where tcno = '"+textBox1.Text+"'", baglantim);
                         guncellekomutu.ExecuteNonQuery(); // güncelle komutu isimli sorguunun sonuçlarını acces tablosuna işle
                        baglantim.Close();
                        MessageBox.Show("kullanıcı bilgileri güncellendi", "SKY personel takip programı",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    kullanicilari_goster();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message,"SKY personel takip programı",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        baglantim.Close();

                    }

                }
                else
                {
                    MessageBox.Show("yazı rengi kırmızı olan alanları yeniden gözden geçiriniz", "SKY personel takip programı",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 11)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand secmesorgusu = new OleDbCommand("select*from kullanicilar where tcno = '" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayit_okuma = secmesorgusu.ExecuteReader();

                while (kayit_okuma.Read())
                {
                    kayit_arama_durumu = true;
                    OleDbCommand deletesorgusu = new OleDbCommand("delete from kullanicilar where tcno = '" + textBox1.Text + "'", baglantim);
                    deletesorgusu.ExecuteNonQuery();
                    MessageBox.Show("kullanıcı kaydı silindi", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    kullanicilari_goster();
                    topPage1_temizle();
                    break;
                }

                if (kayit_arama_durumu== false)
                {
                    MessageBox.Show("silinecek kayıt bulunamadı", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                baglantim.Close();
                topPage1_temizle();
            }
            else
            {
                MessageBox.Show("lütfen 11 karakterden oluşan bir TC kimlik no giriniz", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            topPage1_temizle();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog resimsec = new OpenFileDialog();
            resimsec.Title = "personel resmi seçiniz";  // seçim pencersinin başlkığı
            resimsec.Filter = "JPG dosyalar (*.jpg) | *.jpg";

            if (resimsec.ShowDialog() == DialogResult.OK)
            {
                // openfşkedialog kullanıcıya gösterildiyse
                // bu işlem sağlandıysa
                this.pictureBox2.Image = new Bitmap(resimsec.OpenFile()); // seçilen resmin picrurebox2 ye yüklenmesini sağlıyoruz
                // new bitmap ile  yeni bir resim nesnesi tanımlıyoruz
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            bool kayitkontrol = false;
            baglantim.Open();
            OleDbCommand secimsorgusu = new OleDbCommand("select * from personeller where tcno= '" + maskedTextBox1.Text + "'", baglantim);

            OleDbDataReader kayitokuma = secimsorgusu.ExecuteReader();

            while (kayitokuma.Read()== true)
            {
                kayitkontrol = true;
                break;
            }
            baglantim.Close();

            if (kayitkontrol == false)
            {
                if (pictureBox2.Image == null)
                {
                    button6.ForeColor = Color.Red;
                }
                else
                {
                    button6.ForeColor = Color.Black;
                }

                if (maskedTextBox1.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label13.ForeColor = Color.Red;
                }
                else
                    label13.ForeColor = Color.Black;

                if (maskedTextBox2.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label14.ForeColor = Color.Red;
                }
                else
                    label14.ForeColor = Color.Black;

                if (maskedTextBox3.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label15.ForeColor = Color.Red;
                }
                else
                    label15.ForeColor = Color.Black;

                if (comboBox1.Text == "")
                {
                    label17.ForeColor = Color.Red;
                }
                else
                {
                    label17.ForeColor = Color.Black;
                }

                if (comboBox2.Text == "")
                {
                    label19.ForeColor = Color.Red;
                }
                else
                {
                    label19.ForeColor = Color.Black;
                }

                if (comboBox3.Text == "")
                {
                    label20.ForeColor = Color.Red;
                }
                else
                {
                    label20.ForeColor = Color.Black;
                }

                if (maskedTextBox4.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label21.ForeColor = Color.Red;
                }
                else
                    label21.ForeColor = Color.Black;


                if (int.Parse(maskedTextBox4.Text) <1000)
                {
                    label21.ForeColor = Color.Red;
                }
                else
                {
                    label21.ForeColor = Color.Black;
                }


                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted !=false && maskedTextBox2.MaskCompleted == true 
                    &&maskedTextBox3.MaskCompleted != false && comboBox1.Text != ""&& comboBox2.Text !="" && comboBox3.Text != "" 
                    && maskedTextBox4.MaskCompleted == true)
                {
                    if (radioButton3.Checked == true)
                    {
                        cinsiyet = radioButton3.Text;
                    }
                    else
                    {
                        cinsiyet = radioButton4.Text;
                    }

                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into personeller values('" + maskedTextBox1.Text + "','" + maskedTextBox2.Text+"'," +
                            "'"+maskedTextBox3.Text+"','" + cinsiyet + "','" + comboBox1.Text + "','" + dateTimePicker1.Text + "','" + comboBox2.Text + "','" + comboBox3.Text + "','"            
                            + maskedTextBox4.Text + "')", baglantim);

                        eklekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        if (!Directory.Exists(Application.StartupPath+"\\personelresimler"))
                        {
                            // bu klasör yoksa oluşturulmasını sağlıyoruz
                            Directory.CreateDirectory(Application.StartupPath + "\\personelresimler");
                        }                       
                        pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\"+ maskedTextBox1.Text+".jpg");
                                                   
                        MessageBox.Show("yeni personel kaydı oluşturuldu", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        personelleri_goster();
                        topPage2_temizle();
                        maskedTextBox4.Text = "0";
                    }
                    catch (Exception hatamsj)
                    {

                        MessageBox.Show(hatamsj.Message, "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }

                }

                else
                {
                    MessageBox.Show("yazı rengi kırmızı olan alanları yeniden gözden geçiriniz", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            else
            {
                MessageBox.Show("girilen TC kimlik numarası daha önceden kayıtlıdır", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool kayitarama = false;
            if (maskedTextBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select*from personeller where tcno='" + maskedTextBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read()== true)
                {
                    kayitarama = true;

                    try
                    {
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" + kayitokuma.GetValue(0).ToString() + ".jpg") ;

                    }
                    catch (Exception)
                    {

                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyok.jpg");
                    }
                    maskedTextBox2.Text = kayitokuma.GetValue(1).ToString();
                    maskedTextBox3.Text = kayitokuma.GetValue(2).ToString();
                    if (kayitokuma.GetValue(3).ToString() == "Kadın")
                    {
                        radioButton3.Checked = true;
                    }
                    else
                    {
                        radioButton4.Checked = true;
                    }
                    comboBox1.Text = kayitokuma.GetValue(4).ToString();
                    dateTimePicker1.Text = kayitokuma.GetValue(5).ToString();
                    comboBox2.Text = kayitokuma.GetValue(6).ToString();
                    comboBox3.Text = kayitokuma.GetValue(7).ToString();
                    maskedTextBox4.Text = kayitokuma.GetValue(8).ToString();
                    break;
                }

                if (kayitarama == false)
                {
                    MessageBox.Show("Aranan kayıt nulunamadı", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                baglantim.Close();
            }
            else
            {
                MessageBox.Show("11 haneli TC no giriniz", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);             
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
           
           
                if (pictureBox2.Image == null)
                {
                    button6.ForeColor = Color.Red;
                }
                else
                {
                    button6.ForeColor = Color.Black;
                }

                if (maskedTextBox1.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label13.ForeColor = Color.Red;
                }
                else
                    label13.ForeColor = Color.Black;

                if (maskedTextBox2.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label14.ForeColor = Color.Red;
                }
                else
                    label14.ForeColor = Color.Black;

                if (maskedTextBox3.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label15.ForeColor = Color.Red;
                }
                else
                    label15.ForeColor = Color.Black;

                if (comboBox1.Text == "")
                {
                    label17.ForeColor = Color.Red;
                }
                else
                {
                    label17.ForeColor = Color.Black;
                }

                if (comboBox2.Text == "")
                {
                    label19.ForeColor = Color.Red;
                }
                else
                {
                    label19.ForeColor = Color.Black;
                }

                if (comboBox3.Text == "")
                {
                    label20.ForeColor = Color.Red;
                }
                else
                {
                    label20.ForeColor = Color.Black;
                }

                if (maskedTextBox4.MaskCompleted == false)
                {
                    // oaradki kurala uyulmamışsa
                    label21.ForeColor = Color.Red;
                }
                else
                    label21.ForeColor = Color.Black;


                if (int.Parse(maskedTextBox4.Text) < 1000)
                {
                    label21.ForeColor = Color.Red;
                }
                else
                {
                    label21.ForeColor = Color.Black;
                }


                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted == true
                    && maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != ""
                    && maskedTextBox4.MaskCompleted == true)
                {
                    if (radioButton3.Checked == true)
                    {
                        cinsiyet = radioButton3.Text;
                    }
                    else
                    {
                        cinsiyet = radioButton4.Text;
                    }

                    try
                    {
                        baglantim.Open();
                    OleDbCommand guncellekomutu = new OleDbCommand("update personeller set ad = '" + maskedTextBox2.Text + "',soyad= '" + maskedTextBox3.Text + "'," +
                        "cinsiyet ='" + cinsiyet + "',mezuniyet = '" + comboBox1.Text + "',dogumtarihi='" + dateTimePicker1.Text + "',gorevi = '" + comboBox2.Text + "'," +
                        "gorevyeri = '" + comboBox3.Text + "',maasi = '" + maskedTextBox4.Text + "'where tcno = '" + maskedTextBox1.Text + "'", baglantim);

                        guncellekomutu.ExecuteNonQuery();
                        baglantim.Close();
                        

                        MessageBox.Show("personel bilgileri güncellendi", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        personelleri_goster();
                        topPage2_temizle();
                        maskedTextBox4.Text = "0";
                    }
                    catch (Exception hatamsj)
                    {

                        MessageBox.Show(hatamsj.Message, "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }

                }

                else
                {
                    MessageBox.Show("yazı rengi kırmızı olan alanları yeniden gözden geçiriniz", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.MaskCompleted==true)
            {
                bool kayit_arama = false;
                baglantim.Open();
                OleDbCommand arama_sorgusu = new OleDbCommand("select* from personeller where tcno = '" + maskedTextBox1.Text + "'", baglantim);
                OleDbDataReader kayit_okuma = arama_sorgusu.ExecuteReader();

                while (kayit_okuma.Read())
                {
                    kayit_arama = true;
                    OleDbCommand deletesorgusu = new OleDbCommand("delete from personeller where tcno = '" + maskedTextBox1.Text + "'", baglantim);

                    deletesorgusu.ExecuteNonQuery();
                    break;
                }
                if (kayit_arama == false)
                {
                    MessageBox.Show("silinecek kayıt bulunamadı", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                baglantim.Close();
                personelleri_goster();
                topPage2_temizle();
                maskedTextBox4.Text = "0";
            }

            else
            {
                MessageBox.Show("lütfen 11 karakterden oluşan bit TC kimlik no giriniz", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage2_temizle();
                maskedTextBox4.Text = "0";
            }


        }

        private void button11_Click(object sender, EventArgs e)
        {
            topPage2_temizle();
        }
    }

    
}


