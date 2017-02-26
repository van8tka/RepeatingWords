using Windows.Networking.Connectivity;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.WinPhone.CheckConnect))]
namespace RepeatingWords.WinPhone
{
    public class CheckConnect : ICheckConnect
    {
        public bool CheckTheNet()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            bool isConnected = profile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None;
            return isConnected;
        }
    }
}
