using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace KutuphaneDeneme1
{

    public partial class MainWindow : Window
    {
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
            //Sil();
            //txtAuthor.Clear();
            //txtISBN.Clear();
            //txtName.Clear();
            //date.SelectedDate = null;
            //listKonu.Text = "Seciniz";
            //Listele();
        }


        //Listeleme metodu
        public void Listele()
        {
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
            @"Data Source = C:\Users\Administrator\Documents\Database1.accdb;");
            conn.Open();

            OleDbCommand show = new OleDbCommand("Select Kitaplar.ID, Kitaplar.KitapAdi, Kitaplar.Yazari, Kitaplar.BasimTarihi," +
                "Kitaplar.ISBNnumarasi, Konular.Konu From Kitaplar, Konular where Kitaplar.Konusu = Konular.ID", conn);
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
            string Konu = listKonu.Text;
            string ISBNnumara = txtISBN.Text;

            //Access connection olusturulmasi
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           @"Data Source = C:\Users\Administrator\Documents\Database1.accdb;");
            conn.Open();

            //Kitaplarin eklenmesi
            OleDbCommand add = new OleDbCommand("Insert into Kitaplar (KitapAdi, Yazari, BasimTarihi, ISBNnumarasi) " +
                "Values (@p1, @p2, @p4, @p5) ", conn);
           
            add.Parameters.AddWithValue("@p1", KitapAdi);
            add.Parameters.AddWithValue("@p2", Yazar);
            //add.Parameters.AddWithValue("@p3", Konu);
            add.Parameters.AddWithValue("@p4", basimTarihi);
            add.Parameters.AddWithValue("@p5", ISBNnumara);
            dataGrid.ItemsSource = add.ExecuteReader();
            if (listKonu.Text == "Edebiyat")
            {
                
                OleDbCommand updateEdb = new OleDbCommand("UPDATE Kitaplar SET Konusu = 1 WHERE KitapAdi = @p1", conn);
                updateEdb.Parameters.AddWithValue("@p1", KitapAdi);
                dataGrid.ItemsSource = updateEdb.ExecuteReader();
            }








            //OleDbCommand update = new OleDbCommand("UPDATE Kitaplar SET Kitaplar.Konusu = Konular.ID WHERE KitapAdi = @p1", conn);
            //update.Parameters.AddWithValue("Kitaplar.Konusu", Konu);
            //update.Parameters.AddWithValue("@p1", KitapAdi);




            //add.ExecuteNonQuery();
            //conn.Close();

            //conn.Open();

            Listele();
        }







        //Arama metodu
        public void Ara()
        {
            //if (txtAuthor.Text == "" && txtISBN.Text == "" && txtName.Text == "" && date.SelectedDate == null && listKonu.Text == "Seçiniz")
            //{
            //    MessageBox.Show("Lütfen en az bir değer giriniz.");
            //    return;
            //}
            //Get user input
            string KitapAdi = txtName.Text;
            string Yazar = txtAuthor.Text;
            string Konu = listKonu.Text;
            //DateTime basimTarihi = date.SelectedDate.Value;
            string ISBNnumara = txtISBN.Text;




            //Access connection olusturulmasi
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
           @"Data Source = C:\Users\Administrator\Documents\Database1.accdb;");
            conn.Open();

            //OleDbCommand searchbyDate = new OleDbCommand("Select * from Kitaplar where BasimTarihi > @basimTarihi",conn);

            //searchbyDate.Parameters.AddWithValue(@basimTarihi, );

            OleDbCommand search = new OleDbCommand("Select * from Kitaplar where KitapAdi = @KitapAdi or Yazari = @Yazar or ISBNnumarasi = @ISBNnumara", conn);
            search.Parameters.AddWithValue(@KitapAdi, txtName.Text);
            search.Parameters.AddWithValue(@Yazar, txtAuthor.Text);
            //search.Parameters.AddWithValue(@Konu, listKonu.Text);

            //search.Parameters.AddWithValue(basimTarihi.ToString(), date.SelectedDate.Value);
            search.Parameters.AddWithValue(@ISBNnumara, txtISBN.Text);
            dataGrid.ItemsSource = search.ExecuteReader();

            if (listKonu.Text != "Seçiniz")
            {
                OleDbCommand search_konu = new OleDbCommand("Select * from Kitaplar where Kitaplar.Konusu = Konular.ID", conn);
                search_konu.Parameters.AddWithValue("@Kitaplar.Konusu", Konu);
                dataGrid.ItemsSource = search_konu.ExecuteReader();
            }
        }
        

        ////Silme metodu
        //public void Sil()
        //{
        //    //Get user input
        //    string KitapAdi = txtName.Text;
        //    string Yazar = txtAuthor.Text;
        //    DateTime basimTarihi = date.SelectedDate.Value;
        //    string ISBNnumara = txtISBN.Text;


        //    //Access connection olusturulmasi
        //    OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0;" +
        //   @"Data Source = C:\Users\Administrator\Documents\Database1.accdb;");
        //    conn.Open();

        //    if(date.SelectedDate != null)
        //    {
        //        OleDbCommand delete4 = new OleDbCommand("Delete from Kitaplar where BasimTarihi = @basimTarihi", conn);
        //        delete4.Parameters.AddWithValue(basimTarihi.ToString(), basimTarihi);
        //        dataGrid.ItemsSource = delete4.ExecuteReader();
        //    }

        //    //Kitaplarin silinmesi
        //    OleDbCommand delete = new OleDbCommand("Delete from Kitaplar where KitapAdi = @KitapAdi", conn);
        //    OleDbCommand delete1 = new OleDbCommand("Delete from Kitaplar where Yazari = @Yazar", conn);
        //    OleDbCommand delete2 = new OleDbCommand("Delete from Kitaplar where Konusu = @Konu", conn);
        //    OleDbCommand delete3 = new OleDbCommand("Delete from Kitaplar where ISBNnumarasi = @ISBNnumara", conn);



        //    ComboBoxItem selectedItem = (ComboBoxItem)listKonu.SelectedItem;
        //    string Konu = selectedItem.Content.ToString();

        //    delete.Parameters.AddWithValue(KitapAdi, txtName.Text);
        //    delete1.Parameters.AddWithValue(Yazar, txtAuthor.Text);
        //    delete2.Parameters.AddWithValue(Konu, Konu);
        //    delete3.Parameters.AddWithValue(ISBNnumara, txtISBN.Text);


        //    dataGrid.ItemsSource = delete.ExecuteReader();
        //    dataGrid.ItemsSource = delete1.ExecuteReader();
        //    dataGrid.ItemsSource = delete2.ExecuteReader();
        //    dataGrid.ItemsSource = delete3.ExecuteReader();
        //}

        //Stop running the application when clicked close
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }


    }
}
