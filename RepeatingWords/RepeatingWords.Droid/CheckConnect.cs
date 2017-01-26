
using Android.App;
using Android.Net;
using Android.Content;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.Droid.CheckConnect))]

namespace RepeatingWords.Droid
{
    public class CheckConnect : ICheckConnect
    {
      
        public bool CheckTheNet()
        {
            var cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo != null)
            {
                bool isConnected = cm.ActiveNetworkInfo.IsConnected;
                return isConnected;
            }
            else
                return false;
        }
    }
}