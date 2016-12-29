using System.Globalization;
using Xamarin.Forms;

[assembly: Dependency(typeof(RepeatingWords.Droid.Localize))]

namespace RepeatingWords.Droid
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var andLoc = Java.Util.Locale.Default;
            var netLanguage = andLoc.ToString().Replace("_", "-");
            return new CultureInfo(netLanguage);
        }
    }
}