
using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.InputMethodServices;
using Android.Widget;
using Android.Views.Animations;

namespace RepeatingWords.Droid
{

    [Activity(Label = "Cards of words", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //для установки SplashScreen обязательно использовать FormsAppCompatActivity а не FormsAppActivity
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
           
            LoadApplication(new App());
          
        }


        public async override void OnBackPressed()
        {
            base.OnBackPressed();
        }


    }
}

