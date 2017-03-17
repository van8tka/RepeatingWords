using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.Pages
{

   
    public partial class EntryTranscription : ContentPage
    {
        int idDict;
        string RusWord;
        string EngWord;
        int idWord;
        public EntryTranscription(int idDict,int idWord ,string RusWord, string EngWord, string Trans)
        {
            InitializeComponent();
            this.idDict = idDict;
            this.RusWord = RusWord;
            this.EngWord = EngWord;
            this.idWord = idWord;
            ETransc.Text = Trans;
        }

        private void Clik_ɑ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ɑ";
        }
        private void Clik_ʌ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ʌ";
        }
        private void Clik_ə(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ə";
        }
        private void Clik_ɛ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ɛ";
        }
        private void Clik_æ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "æ";
        }
        private void Clik_ɜ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ɜ";
        }
        private void Clik_ʒ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ʒ";
        }
        private void Clik_ı(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ı";
        }
        private void Clik_ɪ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ɪ";
        }
        private void Clik_ŋ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ŋ";
        }
        private void Clik_ɔ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ɔ";
        }
        private void Clik_ɒ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ɒ";
        }
        private void Clik_ʃ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ʃ";
        }
        private void Clik_ð(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ð";
        }
        private void Clik_θ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "θ";
        }
        private void Clik_ʤ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ʤ";
        }
        private void Clik_ʊ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ʊ";
        }
        private void Clik_b(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "b";
        }
        private void Clik_d(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "d";
        }
        private void Clik_e(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "e";
        }
        private void Clik_f(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "f";
        }
        private void Clik_g(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "g";
        }
        //3 строка(2)
        private void Clik_j(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "j";
        }
        private void Clik_k(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "k";
        }
        private void Clik_l(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "l";
        }
        private void Clik_m(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "m";
        }
        private void Clik_n(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "n";
        }
        private void Clik_p(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "p";
        }
        private void Clik_r(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "r";
        }
        private void Clik_ʧ(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "ʧ";
        }
        private void Clik_h(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "h";
        }
        private void Clik_i(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "i";
        }
        private void Clik_s(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "s";
        }
        // 4 строка(3)
        private void Clik_UpperTouch(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "'";
        }
        private void Clik_t(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "t";
        }
        private void Clik_u(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "u";
        }
        private void Clik_v(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "v";
        }
        private void Clik_w(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "w";
        }
        private void Clik_z(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "z";
        }
        private void Clik_LeftSquareBracket(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "[";
        }
        private void Clik_RightSquareBracket(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + "]";
        }
        private void Clik_DoublePoint(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + ":";
        }
        private void Clik_Comma(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + ",";
        }
     
        private void Clik_Space(object sender, EventArgs e)
        {
            ETransc.Text = ETransc.Text + " ";
        }

        private void Clik_Del(object sender, EventArgs e)
        {
           try
            {
                string g = ETransc.Text;
                int Lenght = g.Length;
                if (Lenght != 0)
                {
                    ETransc.Text = g.Remove(Lenght - 1);
                }
            }
            catch { }
           
        }

        private async void Clik_Enter(object sender, EventArgs e)
        {
            try
            {
                this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
                ETransc.Text = ETransc.Text;
                CreateWord cr = new CreateWord(idDict,idWord, RusWord, EngWord, ETransc.Text);
                await Navigation.PushAsync(cr);
            }
            catch { }
        }

    }

  
}
