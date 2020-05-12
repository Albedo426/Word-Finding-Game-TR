using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wordsFind
{
    public partial class Form1 : Form
    {
        String[,] dizi=new String[,] { 
                                    { "a", "0" },{ "b", "0" },{ "c", "0" },{ "ç", "0" },{ "d", "0"},{ "e","0" },
                                    { "f", "0" },{ "g", "0" },{ "ğ", "0" },{ "h", "0" },{ "ı", "0" },{ "i", "0" },
                                    { "j", "0" },{ "k", "0" },{ "l", "0" },{ "m", "0" },{ "n", "0" },{ "o", "0" },
                                    { "ö", "0" },{ "p", "0" },{ "r", "0" },{ "s", "0" },{ "ş", "0" },{ "t","0" },
                                    { "u", "0" },{ "ü", "0" },{ "v", "0" },{ "y", "0" },{ "z", "0" }
        };
        private int puan = 0;
        String[,] character = new String[8, 2];
        public void setup() {
            int item = 0;
            lettertxt.Text = "";
            selectedword.Text = "";
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                item = rand.Next(0, 29);
                lettertxt.Text += dizi[item, 0]+ " ";
            }
            
        }
        public Form1()
        {
            InitializeComponent();
            setup();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = Application.StartupPath + "\\TDK_Sözlük_Kelime_Listesi.txt";
            button2.Enabled = false;
            int k = 0;
            string[] words = lettertxt.Text.Split(' ');
            foreach (var item in words)
            {
                for (int i = 0; i < 29; i++)
                {
                    if (item == dizi[i, 0])
                    {
                        dizi[i, 1] = (Convert.ToInt16(dizi[i, 1]) + 1).ToString(); ;
                    }
                }
            }

            for (int i = 0; i < (dizi.Length / 2) - 1; i++)
            {

                if (Convert.ToInt16(dizi[i, 1]) != 0)
                {
                    character[k, 0] = dizi[i, 0];
                    character[k, 1] = dizi[i, 1];
                    k++;
                }

            }
            String word = "";//kelimemiz
            int mincarakter = 4;//minimum karakter sayısı kelimede min kaç karakter olsun
            int control = 0;//control bulunan kelimeyle listedeki kelimenin  harrf sayısı aynı mı koonrol 
            int coni = 1;//joker hakkı
            int temp = 0;//soraki kelimeye geçtiğimde jokeri kullnamam için
            var kelimeler = File.ReadLines(filePath);
            String[,] tempcharacter = new String[8, 2];
            Array.Copy(character, tempcharacter, character.Length);
            List<String> selecword = new List<String>();//bulunan kelimeleri tuutğum adres
            foreach (var kelimes in kelimeler)
            {


                foreach (var achar in kelimes)
                {
                    temp = 0;
                    for (int i = 0; i < (character.Length / 2) - 1; i++)
                    {
                        if (character[i, 0]==achar.ToString() && Convert.ToInt16(character[i, 1]) > 0)
                        {
                            control++;
                            character[i, 1] = (Convert.ToInt16(character[i, 1]) - 1).ToString();
                            word += character[i, 0];
                            temp++;
                        }
                    }

                    if (0 == temp && coni == 1)
                    {
                        coni--;
                        control++;
                        word += "("+ achar+")";
                    }
                    if (coni == 1)
                    {
                        temp = 0;
                    }
                }
                if (control== kelimes.Length && control>= mincarakter) {
                    selecword.Add(word);
                }
                Array.Copy(tempcharacter, character, tempcharacter.Length);
                temp = 0;
                word = "";
                control = 0;
                coni = 1;
            }
            listBox1.Items.AddRange(selecword.ToArray());
            selectedword.Text = selectWord(selecword);
            lettertxt.Enabled = false;

        }
        public int score(String maxAge) {
            maxAge = maxAge.Replace(")", "");
            maxAge = maxAge.Replace("(", "");
            int count = maxAge.Length;
            if (count == 3)
            {
                puan += 3;
            }
            else if (count == 4)
            {
                puan += 4;
            }
            else if (count == 5)
            {
                puan += 5;
            }
            else if (count == 6)
            {
                puan += 7;
            }
            else if (count == 7)
            {
                puan += 9;
            }
            else if (count == 8)
            {
                puan += 11;
            }
            else {
                puan += 15;
            }
            return puan;
        }
        public String selectWord(List<String> words) {
            String maxAge = words.ElementAt(0);
            foreach (String word in words)
            {
                if (maxAge.Length < word.Length)
                {
                    maxAge = word;
                }
            }
            Puanlb.Text=score(maxAge).ToString();
            return maxAge;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            lettertxt.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            lettertxt.Enabled = false;
            button2.Enabled = true;
            listBox1.Items.Clear();
            for (int i = 0; i < (dizi.Length / 2) - 1; i++)
            {
                    dizi[i, 1] = "0";
            }
            setup();
            puan = 0;
            Puanlb.Text = "0";
        }
        private void nextbuttton_Click(object sender, EventArgs e)
        {
            lettertxt.Enabled = false;
            button2.Enabled = true;
            for (int i = 0; i < (dizi.Length / 2) - 1; i++)
            {
                dizi[i, 1] = "0";
            }
            setup();
            Puanlb.Text = puan.ToString();
        }
    }
}
