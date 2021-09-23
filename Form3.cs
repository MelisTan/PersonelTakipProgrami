using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp36__PERSONEL_TAKİP_PROGRAMI_
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=personel.accdb");

        private void personelleri_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleri_listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO], ad AS[ADI],soyad AS[SOYADI]," +
                    "cinsiyet AS[CİNSİYETİ],mezuniyet AS[MEZUNİYETİ], dogumtarihi AS[DOĞUM TARİHİ],gorevi AS[GÖREVİ], gorevyeri AS[GÖREV YERİ]," +
                    "maasi AS[MAAŞI] from personeller Order By ad ASC", baglantim);

                DataSet dshafiza = new DataSet();
                personelleri_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
                
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            personelleri_goster();
            this.Text = "KULLANICI İŞLEMLERİ";
            label19.Text = Form1.adi + " " + Form1.soyadi;
            pictureBox1.Width = 150; pictureBox1.Height = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.Width = 150; pictureBox2.Height = 150;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;

            try
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\" + Form1.tcno + ".jpg");
            }
            catch 
            {

                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok.jpg");
            }
            maskedTextBox1.Mask = "00000000000";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select*from personeller where tcno= '" + maskedTextBox1.Text + "'", baglantim);

                OleDbDataReader kayit_okuma = selectsorgu.ExecuteReader();
                while (kayit_okuma.Read())
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" + kayit_okuma.GetValue(0) + ".jpg");
                    }
                    catch (Exception)
                    {

                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyok.jpg");
                    }

                    label11.Text = kayit_okuma.GetValue(1).ToString();
                    label12.Text = kayit_okuma.GetValue(2).ToString();
                    label13.Text = kayit_okuma.GetValue(3).ToString();
                    label14.Text = kayit_okuma.GetValue(4).ToString();
                    label15.Text = kayit_okuma.GetValue(5).ToString();
                    label16.Text = kayit_okuma.GetValue(6).ToString();
                    label17.Text = kayit_okuma.GetValue(7).ToString();
                    label18.Text = kayit_okuma.GetValue(8).ToString();
                    break;
                }
                

                if (kayit_arama_durumu == false)
                {
                    MessageBox.Show("Kayıt bulunamadı!", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                baglantim.Close();
            }

            else
            {
                MessageBox.Show("11 haneli bir TC kimlik no giriniz", "SKY personel takip programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
