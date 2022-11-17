using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace _2115101061_YasarCan
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("server=.;Initial Catalog=StokTakipDb; Integrated Security=SSPI;");
        SqlCommand cmd;
        SqlDataAdapter adapter;

        public Form1()
        {
            InitializeComponent();
        }

        void UrunleriGetir()
        {
            conn.Open();
            adapter = new SqlDataAdapter("SELECT * FROM Urunler", conn);
            DataTable tablo = new DataTable();
            adapter.Fill(tablo);
            dataGridView1.DataSource = tablo;
            conn.Close();
            dataGridView1.Columns["ResimUrl"].Visible = false;
            ToplamUrunSayisi();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UrunleriGetir();
            MagazaAdi();
            ToplamUrunSayisi();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtBarkod.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtDepo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtRaf.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtCins.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtStok.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            cbBirim.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            txtBirimFiyat.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            double kdvEkleme = Convert.ToDouble(txtBirimFiyat.Text) + Convert.ToDouble(txtBirimFiyat.Text) * 0.18;
            txtKdvDahil.Text = kdvEkleme.ToString();
            lblEskiBarkod.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            try
            {
                pictureBox2.Image = Image.FromFile(dataGridView1.CurrentRow.Cells[8].Value.ToString());
            }
            catch
            {
                pictureBox2.ImageLocation = @"..\..\Resources\istockphoto-1216251206-170667a.jpg";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Urunler(BarkodNo,UrunAdi,Depo,Raf,Cinsi,Stok,Birim,BirimFiyat, ResimUrl) VALUES (@BarkodNo,@UrunAdi,@Depo,@Raf,@Cinsi,@Stok,@Birim,@BirimFiyat,@ResimUrl)";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BarkodNo", Convert.ToInt32(txtBarkod.Text));
            cmd.Parameters.AddWithValue("@UrunAdi", txtUrunAdi.Text);
            cmd.Parameters.AddWithValue("@Depo", txtDepo.Text);
            cmd.Parameters.AddWithValue("@Raf", txtRaf.Text);
            cmd.Parameters.AddWithValue("@Cinsi", txtCins.Text);
            cmd.Parameters.AddWithValue("@Stok", Convert.ToInt32(txtStok.Text));
            cmd.Parameters.AddWithValue("@Birim", cbBirim.Text);
            cmd.Parameters.AddWithValue("@BirimFiyat", Convert.ToDouble(txtBirimFiyat.Text));

            if (resimDegisti)
            {
                cmd.Parameters.AddWithValue("@ResimUrl", openFileDialog1.FileName.ToString());
            }

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            UrunleriGetir();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        void temizle()
        {
            for (int i = 0; i < groupBox1.Controls.Count - 1; i++)
            {
                if (groupBox1.Controls[i] is TextBox || groupBox1.Controls[i] is PictureBox)
                {
                    groupBox1.Controls[i].Text = "";
                    lblEskiBarkod.Text = "";
                    pictureBox2.ImageLocation = @"..\..\Resources\istockphoto-1216251206-170667a.jpg";
                }
            }

            for (int i = 0; i < groupBox2.Controls.Count - 1; i++)
            {
                if (groupBox2.Controls[i] is TextBox || groupBox2.Controls[i] is PictureBox)
                {
                    groupBox2.Controls[i].Text = "";
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Urunler SET BarkodNo=@BarkodNo, UrunAdi=@UrunAdi, Depo=@Depo, Raf=@Raf, Cinsi=@Cinsi, Stok=@Stok, Birim=@Birim, BirimFiyat=@BirimFiyat, ResimUrl=@ResimUrl WHERE BarkodNo=@EskiBarkodNo";
            cmd = new SqlCommand(query, conn);
            int eskiBarkod = Convert.ToInt32(lblEskiBarkod.Text);
            cmd.Parameters.AddWithValue("@BarkodNo", Convert.ToInt32(txtBarkod.Text));
            cmd.Parameters.AddWithValue("@EskiBarkodNo", eskiBarkod);
            cmd.Parameters.AddWithValue("@UrunAdi", txtUrunAdi.Text);
            cmd.Parameters.AddWithValue("@Depo", txtDepo.Text);
            cmd.Parameters.AddWithValue("@Raf", txtRaf.Text);
            cmd.Parameters.AddWithValue("@Cinsi", txtCins.Text);
            cmd.Parameters.AddWithValue("@Stok", Convert.ToInt32(txtStok.Text));
            cmd.Parameters.AddWithValue("@Birim", cbBirim.Text);
            cmd.Parameters.AddWithValue("@BirimFiyat", Convert.ToDouble(txtBirimFiyat.Text));

            if (resimDegisti)
            {
                cmd.Parameters.AddWithValue("@ResimUrl", openFileDialog1.FileName.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@ResimUrl", dataGridView1.CurrentRow.Cells[8].Value.ToString());
            }

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            UrunleriGetir();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Urunler WHERE BarkodNo=@BarkodNo";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BarkodNo", Convert.ToInt32(lblEskiBarkod.Text));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            UrunleriGetir();
        }

        void MagazaAdi()
        {
            conn.Open();
            string query = "SELECT MagazaAdi FROM MagazaAyarlari";
            cmd = new SqlCommand(query, conn);
            var deger = cmd.ExecuteReader();
            while (deger.Read())
            {
                lblMagazaAdi.Text = deger["MagazaAdi"].ToString();

            }
            conn.Close();
        }

        void ToplamUrunSayisi()
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Urunler";
            cmd = new SqlCommand(query, conn);
            int deger = (Int32)cmd.ExecuteScalar();
            conn.Close();
            lblUrunSayisi.Text = deger.ToString();
        }

        void AramaYap()
        {
            if (txtArama.Text == "")
            {
                UrunleriGetir();
            }
            else
            {
                conn.Open();
                adapter = new SqlDataAdapter("SELECT * FROM Urunler WHERE BarkodNo LIKE '%" + txtArama.Text + "%' OR UrunAdi LIKE '%" + txtArama.Text + "%'", conn);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);
                dataGridView1.DataSource = tablo;
                conn.Close();
                dataGridView1.Columns["ResimUrl"].Visible = false;
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            AramaYap();
        }

        bool resimDegisti = false;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult sonuc;
            sonuc = openFileDialog1.ShowDialog();

            if (sonuc == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
                resimDegisti = true;
            }
            else
            {
                resimDegisti = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
