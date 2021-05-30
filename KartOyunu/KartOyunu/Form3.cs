using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KartOyunu
{
    public partial class Form3 : Form
    {
        enum Tiklamalar
        {
            ilkTiklama, ikinciTiklama

        }
        #region Genel Değişkenler
        Tiklamalar tiklama = Tiklamalar.ilkTiklama;
        PictureBox oncekiResim;
        static SoundPlayer resimAcSes = new SoundPlayer(Properties.Resources.flip);
        static SoundPlayer ayniSes = new SoundPlayer(Properties.Resources.match);
        static SoundPlayer oyunBittiSes = new SoundPlayer(Properties.Resources.gameOver);
        static SoundPlayer zaferSes = new SoundPlayer(Properties.Resources.victory);
        static SoundPlayer yanlisSes = new SoundPlayer(Properties.Resources.Yanlış_Buton_Sesi_dııııııt__online_audio_converter_com_);
        static SoundPlayer oyunYenileSes = new SoundPlayer(Properties.Resources.yt1s_com___Montaj_İçin_Ses_Efekti_Bateri_Sesi_Bu_Dım_Tısss__online_audio_converter_com_);

        int kalan;
        int ciftSay;
        int puan;

        #endregion
        void ResimleriGizle(params PictureBox[] Resimler)
        {
            if (Resimler.Length == 0) Resimler = pZemin.Controls.Cast<PictureBox>().ToArray();
            foreach (PictureBox x in Resimler) x.Image = imageList1.Images[0];
        }

        void ResimleriGoster()
        {
            foreach (PictureBox x in pZemin.Controls) x.Image = imageList1.Images[(int)x.Tag];
        }

        void ResimleriDoldur()
        {
            ArrayList Tagler = new ArrayList();
            for (int i = 0; i < (ciftSay) * 2; i++) Tagler.Add((i % ciftSay) + 1);
            Random r = new Random();


            foreach (PictureBox x in pZemin.Controls)
            {
                int sansli = r.Next(Tagler.Count);
                x.Tag = Tagler[sansli];
                x.Show();
                Tagler.RemoveAt(sansli);
            }
        }
        public Form3()
        {
            InitializeComponent();
        }
        private void YenidenBaslat()
        {
            timer1.Start();
            lblPuan.Text = "0";

            ciftSay = imageList1.Images.Count - 1;
            kalan = ciftSay;
            lblKalan.Text = kalan.ToString();
            ResimleriDoldur();
            ResimleriGizle();
            oncekiResim = null;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox simdikiResim = sender as PictureBox;
            #region Uyanıklığı Önler
            if (oncekiResim == simdikiResim)
            {
                MessageBox.Show("Uyanık Olma !");
                return;
            }
            #endregion
            #region Resim Açma
            simdikiResim.Image = imageList1.Images[(int)simdikiResim.Tag];
            pZemin.Refresh();
            resimAcSes.Play();
            #endregion
            switch (tiklama)
            {
                case Tiklamalar.ilkTiklama:
                    #region Resmi yedekler ve bir sonraki tıklama için hazırlar
                    oncekiResim = simdikiResim;
                    tiklama = Tiklamalar.ikinciTiklama;
                    #endregion
                    break;
                case Tiklamalar.ikinciTiklama:
                    Thread.Sleep(200);
                    if (oncekiResim.Tag.ToString() == simdikiResim.Tag.ToString())
                    {
                        #region Resimler Aynıysa
                        ayniSes.Play();
                        oncekiResim.Hide();
                        simdikiResim.Hide();
                        lblKalan.Text = (--kalan).ToString();

                        puan = puan + 10;
                        lblPuan.Text = puan.ToString();
                        if (kalan == 0)
                        {
                            timer1.Stop();

                            zaferSes.Play();
                            MessageBox.Show("Tebrikler! Puanınız:  " + lblPuan.Text);
                            timer1.Enabled = false;
                            dakika = 3;
                            saniye = 60;

                            YenidenBaslat();

                        }

                        #endregion
                    }
                    else
                    {
                        #region Resimler Farklıysa
                        yanlisSes.Play();
                        ResimleriGizle(oncekiResim, simdikiResim);
                        #endregion
                    }
                    tiklama = Tiklamalar.ilkTiklama;
                    oncekiResim = null;
                    break;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            oyunYenileSes.Play();

            timer1.Start();


            YenidenBaslat();
            lblPuan.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnaSayfa anasayfa = new AnaSayfa();
            this.Hide();
            anasayfa.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            timer1.Stop();
            dakika = 3;
            saniye = 60;
            Thread.Sleep(200);
            MessageBox.Show("Yeni Oyun");
            oyunYenileSes.Play();
            YenidenBaslat();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResimleriGoster();
            pZemin.Refresh();
            Thread.Sleep(500);
            ResimleriGizle();
        }
        int dakika = 3;
        int saniye = 60;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;

             saniye = saniye - 1;
             lblSaniye.Text = Convert.ToString(saniye);
             lblDakika.Text = Convert.ToString(dakika - 1);
            if (saniye == 0)
            {

                dakika = dakika - 1;
                lblDakika.Text = Convert.ToString(dakika);
                saniye = 60;
            }
            if (saniye == 9 || saniye == 8 || saniye == 7 || saniye == 6 || saniye == 5 || saniye == 4 || saniye == 3 || saniye == 2 || saniye == 1 || saniye == 0)
            {

                lblSaniye.Text = "0" + Convert.ToString(saniye);
            }
            if (lblDakika.Text == "-1")
            {
                timer1.Stop();
                lblDakika.Text = "00";
                lblSaniye.Text = "00";
                oyunBittiSes.Play();
                MessageBox.Show("Süre Bitti Başarısız Oldunuz!  Puanınız: " + lblPuan.Text);
                dakika = 3;
                saniye = 60;
                YenidenBaslat();
            }

        }
    }
}
