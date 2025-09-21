using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace EnkripsiApp
{
    public partial class Form1 : Form
    {
        private readonly string key = "1234567890123456"; // Kunci untuk AES (harusnya 16 byte)
        private readonly string iv = "abcdefghijklmnop"; // IV untuk AES (harusnya 16 byte)

        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }

        // Metode untuk mengenkripsi teks
        private string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Metode untuk mendekripsi teks
        private string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        // Event handler untuk tombol Encrypt
        private void button1_Click(object sender, EventArgs e)
        {
            string plainText = textBox1.Text; // Ambil teks asli dari textBox1
            if (!string.IsNullOrEmpty(plainText))
            {
                string encryptedText = Encrypt(plainText); // Encrypt
                textBox2.Text = encryptedText; // Tampilkan hasil enkripsi di textBox2
            }
            else
            {
                MessageBox.Show("Teks asli tidak boleh kosong.");
            }
        }

        // Event handler untuk tombol Decrypt
        private void button2_Click(object sender, EventArgs e)
        {
            string cipherText = textBox3.Text; // Ambil teks terenkripsi dari textBox3
            if (!string.IsNullOrEmpty(cipherText))
            {
                try
                {
                    string decryptedText = Decrypt(cipherText); // Decrypt
                    textBox4.Text = decryptedText; // Tampilkan hasil dekripsi di textBox4
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan saat dekripsi: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Teks terenkripsi tidak boleh kosong.");
            }
        }

        // Event handler saat form dimuat
        private void Form1_Load(object sender, EventArgs e)
        {
            // Inisialisasi saat form dimuat
        }
    }
}
