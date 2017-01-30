using System.Globalization;
using Xamarin.Forms;

[assembly: Dependency(typeof(RepeatingWords.UWP.Localize))]

namespace RepeatingWords.UWP
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return CultureInfo.CurrentUICulture;
        }
              
    }
}