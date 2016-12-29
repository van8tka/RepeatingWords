using System.Globalization;

namespace RepeatingWords
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
