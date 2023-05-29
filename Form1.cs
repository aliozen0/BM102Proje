using System.Data.SqlClient;

namespace ucuncuApp
{
    public partial class frmGiris : Form
    {
        private SqlConnection baglanti = null;
        public frmGiris()
        {
            InitializeComponent();
        }
        private void frmGiris_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=ACER-58G\SQLEXPRESS;Initial Catalog=ikinci;Integrated Security=True");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            panelGiris.Hide();
            button1.Hide();
            panelUye.Show();
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            panelUye.Hide();
            button1.Show();
            panelGiris.Show();
            Temizle();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string mail = textBoxMail.Text;
            string sifre = textBoxSifre.Text;

            string kullaniciID = KullaniciDogrula(mail, sifre);

            if (string.IsNullOrWhiteSpace(mail))
            {
                MessageBox.Show("L�tfen E posta Giriniz !!");
                return;
            }

            if (string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lutfen Sifre Giriniz !!");
                return;
            }
            if (!string.IsNullOrEmpty(kullaniciID))
            {
                MessageBox.Show("G�R�� BA�ARILI HO�GELD�N�Z");
                anaForm anaForm = new anaForm(kullaniciID);
                anaForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Ge�ersiz kullan�c� ad� veya �ifre.");
            }
        }

        private string KullaniciDogrula(string mail, string sifre)
        {
            string kullaniciID = "";
            try
            {
                using (SqlConnection baglanti = new SqlConnection(@"Data Source=ACER-58G\SQLEXPRESS;Initial Catalog=ikinci;Integrated Security=True"))
                {
                    baglanti.Open();
                    SqlCommand sqlKomut = new SqlCommand("SELECT KullaniciID FROM Kullanicilar WHERE mail = @mail AND sifre = @sifre", baglanti);
                    sqlKomut.Parameters.AddWithValue("@mail", mail);
                    sqlKomut.Parameters.AddWithValue("@sifre", sifre);

                    SqlDataReader sqlDR = sqlKomut.ExecuteReader();

                    if (sqlDR.Read())
                    {
                        kullaniciID = sqlDR["KullaniciID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullan�c� do�rulama i�leminde bir hata olu�tu: " + ex.Message);
            }

            return kullaniciID;
        }

        private void btnUyeol_Click(object sender, EventArgs e)
        {
            string ad = textBoxAd.Text;
            string soyad = textBoxSoyad.Text;
            string mail = textBoxEposta.Text;
            string telNo = textBoxTel.Text;
            string sifre = textBoxSifreyeni.Text;
            string sifreTekrar = textBoxSifreTekrar.Text;

            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad) ||
                string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(sifre) ||
                string.IsNullOrWhiteSpace(sifreTekrar) || string.IsNullOrWhiteSpace(telNo))
            {
                MessageBox.Show("L�tfen t�m alanlar� doldurun.");
                return;
            }

            if (sifre != sifreTekrar)
            {
                MessageBox.Show("�ifreler e�le�miyor. L�tfen tekrar kontrol edin.");
                return;
            }

            try
            {
                baglanti.Open();
                SqlCommand sqlKomut = new SqlCommand("INSERT INTO Kullanicilar (Ad, Soyad, Mail, Sifre,telNo) " +
                                                    "VALUES (@ad, @soyad, @mail, @sifre,@telNo)", baglanti);
                sqlKomut.Parameters.AddWithValue("@ad", ad);
                sqlKomut.Parameters.AddWithValue("@soyad", soyad);
                sqlKomut.Parameters.AddWithValue("@mail", mail);
                sqlKomut.Parameters.AddWithValue("@sifre", sifre);
                sqlKomut.Parameters.AddWithValue("@telNo", telNo);

                int etkilenenSatirSayisi = sqlKomut.ExecuteNonQuery();
                if (etkilenenSatirSayisi > 0)
                {
                    MessageBox.Show("Ba�ar�l� Bir �ekilde �ye Oldunuz ");
                    Temizle();
                    panelGiris.Show();
                    panelUye.Hide();
                    button1.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullan�c� kaydetme i�leminde bir hata olu�tu: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void Temizle()
        {
            textBoxAd.Clear();
            textBoxSoyad.Clear();
            textBoxEposta.Clear();
            textBoxTel.Clear();
            textBoxSifreyeni.Clear();
            textBoxSifreTekrar.Clear();

        }

        private void textBoxTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlar� ve kontrol tu�lar�n� kabul et
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '+')
            {
                e.Handled = true;
            }

            // Maksimum karakter say�s�n� kontrol et
            if (textBoxTel.Text.Length >= 13 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }
    }

}