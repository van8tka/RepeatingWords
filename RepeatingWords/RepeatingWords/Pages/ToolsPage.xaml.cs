
using System;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{

    public partial class ToolsPage : ContentPage
    {


        //переменная отображения стиля темы
        const string Them = "theme";
        const string _whiteThem = "white";
        const string _blackThem = "black";
        //переменные для показа клавиатуры транскрипции
        const string TrKeyboard = "TrKeyboard";
        const string showKeyboard = "true";
        const string UnShowKeyboard = "false";

        public ToolsPage()
        {
            InitializeComponent();
         
            object propThem = "";
            object propTrKeyb = "";
            if (App.Current.Properties.TryGetValue(Them, out propThem))
            {
                if (propThem.Equals(_blackThem))
                {                  
                    SwDark.IsToggled = true;
                }
                else
                {
                    SwLight.IsToggled = true;
                    this.BackgroundColor = Color.Silver;
                }
            }
            if(App.Current.Properties.TryGetValue(TrKeyboard, out propTrKeyb))
            {
                if (propTrKeyb.Equals(showKeyboard))
                {
                   SwShowKeyboard.IsToggled = true;
                }
                else
                {
                    SwShowKeyboard.IsToggled = false;
                }
            }
        }



        //вызов главной страницы и чистка стека страниц
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private void switcher_ToggledShowKeyboard(object sender, ToggledEventArgs e)
        {
          if(SwShowKeyboard.IsToggled==true)
            {
                App.Current.Properties.Remove(TrKeyboard);
                App.Current.Properties.Add(TrKeyboard, showKeyboard);
            }
          else
            {
                App.Current.Properties.Remove(TrKeyboard);
                App.Current.Properties.Add(TrKeyboard, UnShowKeyboard);
            }
          
        }


        //обработка переключателей
        private void switcher_ToggledDark(object sender, ToggledEventArgs e)
        {
            //delete properties and then create new
           
             if (SwDark.IsToggled == true)
            {               
                SwLight.IsToggled = false;
                App.Current.Properties.Remove(Them);
                App.Current.Properties.Add(Them, _blackThem);
                Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppBlack"];
                Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppWhite"];
                Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelYellow"];
                Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelWhite"];
                Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorWhite"];
                Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorGreen"];
                this.BackgroundColor = Color.Black;
            }
           else
            {//при изменении IsToggled происходит вызов события switcher_ToggledLight              
                SwLight.IsToggled = true;
                App.Current.Properties.Remove(Them);
                App.Current.Properties.Add(Them, _whiteThem);
                Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppWhite"];
                Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppBlack"];
                Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelNavy"];
                Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelBlack"];
                Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorBlack"];
                Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorBlue"];
                this.BackgroundColor = Color.Silver;
            }

        }

        private void switcher_ToggledLight(object sender, ToggledEventArgs e)
        {
            //delete properties and then create new
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
