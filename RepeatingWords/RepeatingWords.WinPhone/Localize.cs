using System.Globalization;
using Xamarin.Forms;

[assembly: Dependency(typeof(RepeatingWords.WinPhone.Localize))]

namespace RepeatingWords.WinPhone
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return CultureInfo.CurrentUICulture;
        }
              
    }
}