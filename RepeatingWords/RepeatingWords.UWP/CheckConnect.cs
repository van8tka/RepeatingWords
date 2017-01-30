using Windows.Networking.Connectivity;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.UWP.CheckConnect))]
namespace RepeatingWords.UWP
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
