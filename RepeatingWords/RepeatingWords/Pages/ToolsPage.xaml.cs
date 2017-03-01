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

    public partial class ToolsPage : ContentPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        //обработка переключателей
        private void switcher_ToggledDark(object sender, ToggledEventArgs e)
        {
            if(SwDark.IsToggled == true)
            {              
                SwLight.IsToggled = false;
            }
           else
            {
                SwLight.IsToggled = true;
            }
        }

        private void switcher_ToggledLight(object sender, ToggledEventArgs e)
        {
            if (SwLight.IsToggled == true)
            {
                SwDark.IsToggled = false;
            }
            else
            {
                SwDark.IsToggled = true;
            }
        }
    }

}
