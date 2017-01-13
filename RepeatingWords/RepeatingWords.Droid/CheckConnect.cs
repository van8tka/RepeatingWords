
using Android.App;
using Android.Net;
using Android.Content;
//using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.Droid.CheckConnect))]

namespace RepeatingWords.Droid
{
    public class CheckConnect : ICheckConnect
    {
      
        public bool CheckTheNet()
        {
            var cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            bool isConnected = cm.ActiveNetworkInfo.IsConnected;
            return isConnected;
        }
    }
}