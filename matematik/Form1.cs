using System;
using System.Windows.Forms;

namespace matematik
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        private Label[] soruLabels;
        private int LevelToplamSoru = 20; // Toplam soru sayısını 20 olarak ayarladık
        private int mevcutLevel = 1; // Mevcut seviye değişkeni
        private int dogruCevapSayisi = 0; // Doğru cevap sayısı
        private int toplamCevapSayisi = 0; // Toplam cevaplanan soru sayısı
        private int currentQuestionIndex = 0; // Şu anki soru indeksi

        public Form1()
        {
            InitializeComponent();
            soruLabels = new Label[] { label1, label2, label3, label4, label5 };
            buttonNext.Click += new EventHandler(buttonNext_Click); // İleri butonuna tıklanma olayı ekle
            LoadQuestions(); // Soruları yükle
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            ClearPreviousAnswers(); // Oyun başladığında önceki cevapları temizle
            LoadQuestions(); // Yeni soruları yükle
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (toplamCevapSayisi < LevelToplamSoru) // Eğer toplam soru sayısına ulaşmadıysak
            {
                CheckCurrentAnswers(); // Geçerli cevapları kontrol et
                ClearPreviousAnswers(); // Önceki cevapları temizle
                currentQuestionIndex += 5; // Her seferinde sonraki 5 soruya geç
                toplamCevapSayisi += 5; // Cevaplanan soru sayısını artır

                if (toplamCevapSayisi < LevelToplamSoru) // Eğer henüz 20. soraya ulaşmadıysa
                {
                    LoadQuestions(); // Yeni soruları yükle
                }
                else // 20. soraya ulaşıldıysa
                {
                    CheckAllAnswers(); // Tüm cevapları kontrol et
                }
            }
        }

        private void ClearPreviousAnswers()
        {
            foreach (Label lbl in soruLabels)
            {
                lbl.Tag = null; // Önceki cevapları temizle
                lbl.Text = ""; // Label metnini temizle
            }

            // Tüm TextBox'ları temizle
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private string GetRandomOperationLevel1()
        {
            string[] operations = { "+", "-" }; // Sadece toplama ve çıkarma
            return operations[random.Next(operations.Length)];
        }

        private void LoadQuestions()
        {
            for (int i = 0; i < soruLabels.Length; i++)
            {
                if (currentQuestionIndex + i < LevelToplamSoru) // Sadece mevcut soru indeksine göre yükle
                {
                    int num1 = random.Next(1, 21);
                    int num2 = random.Next(1, 21);
                    string operation = GetRandomOperationLevel1(); // Sadece toplama ve çıkarma için özel fonksiyon

                    // Soru oluştur ve label'a yaz
                    string soru = $"{num1} {operation} {num2} = ?";
                    soruLabels[i].Text = soru;

                    // Doğru cevabı sakla
                    soruLabels[i].Tag = Hesapla(num1, num2, operation); // Doğru cevabı etiketin Tag özelliğine kaydet
                }
            }
        }

        private void CheckCurrentAnswers()
        {
            for (int i = 0; i < soruLabels.Length; i++)
            {
                if (soruLabels[i].Tag != null) // Eğer label'da bir cevap varsa
                {
                    int dogruCevap = (int)soruLabels[i].Tag; // Doğru cevabı al
                    int kullaniciCevabi;
                    string userInput = GetUserAnswer(i); // Kullanıcı cevabını al

                    // Kullanıcı cevaplarını kontrol et
                    if (int.TryParse(userInput, out kullaniciCevabi))
                    {
                        if (kullaniciCevabi == dogruCevap)
                        {
                            dogruCevapSayisi++; // Doğru cevap sayısını artır
                        }
                    }
                }
            }
            labelCevapSayisi.Text = $"Doğru Cevap Sayısı: {dogruCevapSayisi}"; // Doğru cevap sayısını göster
        }

        private void CheckAllAnswers()
        {
            MessageBox.Show($"Tüm sorular tamamlandı! Doğru cevap sayınız: {dogruCevapSayisi}"); // Tüm sorular tamamlandığında mesaj göster
            // Burada sonuçları gösterebilirsiniz.
        }

        private int Hesapla(int num1, int num2, string operation)
        {
            if (operation == "+")
            {
                return num1 + num2;
            }
            else if (operation == "-")
            {
                return num1 - num2;
            }
            else
            {
                return 0; // Geçersiz işlem durumunda 0 döndür
            }
        }

        private string GetUserAnswer(int index)
        {
            switch (index)
            {
                case 0: return textBox1.Text; // label1 için cevap
                case 1: return textBox2.Text; // label2 için cevap
                case 2: return textBox3.Text; // label3 için cevap
                case 3: return textBox4.Text; // label4 için cevap
                case 4: return textBox5.Text; // label5 için cevap
                default: return ""; // Geçersiz indeks
            }
        }
    }
}
