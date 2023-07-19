using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;

namespace KutuphaneDeneme1
{

    public partial class MainWindow : Window
    {
        private bool denOnceSelected;
        private bool denSonraSelected;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnListele_Click(object sender, RoutedEventArgs e)
        {
            Listele();
        }

        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            Ekle();
            txtAuthor.Clear();
            txtISBN.Clear();
            txtName.Clear();
            date.SelectedDate = null;
            //listKonu.Text = "Seciniz";
        }

        private void btnAra_Click(object sender, RoutedEventArgs e)
        {
            Ara();
            txtAuthor.Clear();
            txtISBN.Clear();
            txtName.Clear();
            date.SelectedDate = null;
            listKonu.Text = "Seciniz";
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            Sil();
            txtAuthor.Clear();
            txtISBN.Clear();
            txtName.Clear();
            date.SelectedDate = null;
            listKonu.Text = "Seciniz";
            Listele();
        }


        //Listeleme metodu
        public void Listele()
        {
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
            @"Data Source = C:\Users\Administrator\Desktop\Database1.accdb;");
            conn.Open();

            OleDbCommand show = new OleDbCommand("Select Kitaplar.ID, Kitaplar.KitapAdi, Kitaplar.Yazari, Kitaplar.BasimTarihi," +
                "Kitaplar.ISBNnumarasi, Konular.Konu From Kitaplar, Konular where Kitaplar.Konusu = Konular.ID" + " ORDER BY Kitaplar.ID", conn);
            // OleDbCommand show = new OleDbCommand("Select * From Kitaplar, Konular where Kitaplar.Konusu = Konular.ID", conn);

            dataGrid.ItemsSource = show.ExecuteReader();


        }



        ////Seçiliyi sil
        //public void seciliSil()
        //{
        //    var selectedItem = dataGrid.SelectedItem;
        //    if (selectedItem != null)
        //    {

        //    }
        //}


        //Ekleme metodu
        public void Ekle()
        {

            //Get user input
            string KitapAdi = txtName.Text;
            string Yazar = txtAuthor.Text;
            DateTime basimTarihi = date.SelectedDate.Value;
            //string Konu = listKonu.Text;
            string ISBNnumara = txtISBN.Text;
            //string strKonu = listKonu.Text;
            int intKonu = listKonu.SelectedIndex;


            //Access connection olusturulmasi
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           @"Data Source = C:\Users\Administrator\Desktop\Database1.accdb;");
            conn.Open();

            //Kitaplarin eklenmesi
            OleDbCommand add = new OleDbCommand("Insert into Kitaplar (KitapAdi, Yazari, Konusu, BasimTarihi, ISBNnumarasi) " +
                "Values (@p1, @p2, @p3, @p4, @p5) ", conn);

            add.Parameters.AddWithValue("@p1", KitapAdi);
            add.Parameters.AddWithValue("@p2", Yazar);
            add.Parameters.AddWithValue("@p3", intKonu);
            add.Parameters.AddWithValue("@p4", basimTarihi);
            add.Parameters.AddWithValue("@p5", ISBNnumara);
            dataGrid.ItemsSource = add.ExecuteReader();
            //dataGrid.ItemsSource = updateKonu.ExecuteReader();
            //add.ExecuteNonQuery();
            //updateKonu.ExecuteNonQuery();

            conn.Close();
            conn.Open();
            Listele();

        }






        //Arama metodu
        public void Ara()
        {
            if (txtAuthor.Text == "" && txtISBN.Text == "" && txtName.Text == "" && date.SelectedDate == null && listKonu.Text == "Seçiniz")
            {
                MessageBox.Show("Lütfen en az bir değer giriniz.");
                return;
            }

            //Get user input
            string KitapAdi = txtName.Text;
            string Yazar = txtAuthor.Text;
            int intKonu = listKonu.SelectedIndex;
            //DateTime basimTarihi = date.SelectedDate.Value;
            string ISBNnumara = txtISBN.Text;




            //Access connection olusturulmasi
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           @"Data Source = C:\Users\Administrator\Desktop\Database1.accdb;");
            conn.Open();



            // OleDbCommand search = new OleDbCommand("Select Kitaplar.ID, Kitaplar.KitapAdi, Kitaplar.Yazari, Kitaplar.BasimTarihi, " +
            //     "Kitaplar.ISBNnumarasi, Konular.Konu from Kitaplar, Konular where KitapAdi = @KitapAdi or Yazari = @Yazar " +
            //     "or (Konusu = @Konu and Kitaplar.Konusu = Konular.ID) or ISBNnumarasi = @ISBNnumara", conn);
            // search.Parameters.AddWithValue(@KitapAdi, txtName.Text);
            // search.Parameters.AddWithValue(@Yazar, txtAuthor.Text);
            // search.Parameters.AddWithValue("@Konu", intKonu);

            // //search.Parameters.AddWithValue(basimTarihi.ToString(), date.SelectedDate.Value);
            // search.Parameters.AddWithValue(@ISBNnumara, txtISBN.Text);
            // dataGrid.ItemsSource = search.ExecuteReader();




            if (denOnceSelected)
            {

                DateTime basimTarihi = date.SelectedDate.Value;

                OleDbCommand tarihtenOnce = new OleDbCommand("SELECT* FROM Kitaplar WHERE BasimTarihi < @basimTarihi ORDER BY Kitaplar.ID", conn);
                tarihtenOnce.Parameters.AddWithValue("@basimTarihi", basimTarihi);

                dataGrid.ItemsSource = tarihtenOnce.ExecuteReader();

                txtAuthor.Clear();
                txtISBN.Clear();
                txtName.Clear();
                date.SelectedDate = null;
                denOnceSelected = false;
                denSonraSelected = false;

            }
            else if (denSonraSelected)
            {
                DateTime basimTarihi = date.SelectedDate.Value;

                OleDbCommand tarihtenSonra = new OleDbCommand("Select Kitaplar.ID, Kitaplar.KitapAdi, Kitaplar.Yazari, Kitaplar.BasimTarihi, " +
                    "Kitaplar.ISBNnumarasi, Konular.Konu from Kitaplar, Konular WHERE Kitaplar.BasimTarihi > @basimTarihi AND " +
                    "Kitaplar.Konusu = Konular.ID ORDER BY Kitaplar.ID", conn);
                tarihtenSonra.Parameters.AddWithValue("@basimTarihi", basimTarihi);


                dataGrid.ItemsSource = tarihtenSonra.ExecuteReader();
                txtAuthor.Clear();
                txtISBN.Clear();
                txtName.Clear();
                date.SelectedDate = null;
                denOnceSelected = false;
                denSonraSelected = false;
            }
            else
            {

                OleDbCommand search = new OleDbCommand("Select Kitaplar.ID, Kitaplar.KitapAdi, Kitaplar.Yazari, Kitaplar.BasimTarihi, " +
                    "Kitaplar.ISBNnumarasi, Konular.Konu from Kitaplar, Konular where KitapAdi = @KitapAdi or Yazari = @Yazar " +
                    "or (Konusu = @Konu and Kitaplar.Konusu = Konular.ID) or ISBNnumarasi = @ISBNnumara  ORDER BY Kitaplar.ID", conn);
                search.Parameters.AddWithValue(@KitapAdi, txtName.Text);
                search.Parameters.AddWithValue(@Yazar, txtAuthor.Text);
                search.Parameters.AddWithValue("@Konu", intKonu);

                //search.Parameters.AddWithValue(basimTarihi.ToString(), date.SelectedDate.Value);
                search.Parameters.AddWithValue(@ISBNnumara, txtISBN.Text);
                dataGrid.ItemsSource = search.ExecuteReader();
                txtAuthor.Clear();
                txtISBN.Clear();
                txtName.Clear();
                date.SelectedDate = null;
                denOnceSelected = false;
                denSonraSelected = false;
            }



        }


        //Silme metodu
        public void Sil()
        {
            //Get user input
            string KitapAdi = txtName.Text;
            string Yazar = txtAuthor.Text;
            string ISBNnumara = txtISBN.Text;
            int intKonu = listKonu.SelectedIndex;


            //Access connection olusturulmasi
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           @"Data Source = C:\Users\Administrator\Desktop\Database1.accdb;");
            conn.Open();

            if (date.SelectedDate != null)
            {
                DateTime basimTarihi = date.SelectedDate.Value;
                OleDbCommand delete4 = new OleDbCommand("Delete from Kitaplar where BasimTarihi = @basimTarihi", conn);
                delete4.Parameters.AddWithValue(basimTarihi.ToString(), basimTarihi);
                dataGrid.ItemsSource = delete4.ExecuteReader();
            }

            //Kitaplarin silinmesi
            OleDbCommand delete = new OleDbCommand("Delete from Kitaplar where KitapAdi = @KitapAdi", conn);
            OleDbCommand delete1 = new OleDbCommand("Delete from Kitaplar where Yazari = @Yazar", conn);
            OleDbCommand delete2 = new OleDbCommand("Delete from Kitaplar where Konusu = @Konu", conn);
            OleDbCommand delete3 = new OleDbCommand("Delete from Kitaplar where ISBNnumarasi = @ISBNnumara", conn);




            delete.Parameters.AddWithValue(KitapAdi, txtName.Text);
            delete1.Parameters.AddWithValue(Yazar, txtAuthor.Text);
            delete2.Parameters.AddWithValue("@Konu", intKonu);
            delete3.Parameters.AddWithValue(ISBNnumara, txtISBN.Text);


            dataGrid.ItemsSource = delete.ExecuteReader();
            dataGrid.ItemsSource = delete1.ExecuteReader();
            dataGrid.ItemsSource = delete2.ExecuteReader();
            dataGrid.ItemsSource = delete3.ExecuteReader();
        }

        //Stop running the application when clicked close
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }


        private void btnTarihOnce_Click(object sender, RoutedEventArgs e)
        {

           // //Access connection olusturulmasi
           // OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           //@"Data Source = C:\Users\Administrator\Desktop\Database1.accdb;");
           // conn.Open();

           // DateTime basimTarihi = date.SelectedDate.Value;

           // OleDbCommand tarihtenOnce = new OleDbCommand("SELECT* FROM Kitaplar WHERE BasimTarihi < @basimTarihi", conn);
           // tarihtenOnce.Parameters.AddWithValue("@basimTarihi", basimTarihi);

           // dataGrid.ItemsSource = tarihtenOnce.ExecuteReader();

            denOnceSelected = true;
            denSonraSelected = false;

        }

        private void btnTarihSonra_Click(object sender, RoutedEventArgs e)
        {
           // //Access connection olusturulmasi
           // OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           //@"Data Source = C:\Users\Administrator\Desktop\Database1.accdb;");
           // conn.Open();

           // DateTime basimTarihi = date.SelectedDate.Value;

           // OleDbCommand tarihtenOnce = new OleDbCommand("SELECT* FROM Kitaplar WHERE BasimTarihi > @basimTarihi", conn);
           // tarihtenOnce.Parameters.AddWithValue("@basimTarihi", basimTarihi);

           // dataGrid.ItemsSource = tarihtenOnce.ExecuteReader();

            denSonraSelected = true;
            denOnceSelected = false;
        }
    }
}




