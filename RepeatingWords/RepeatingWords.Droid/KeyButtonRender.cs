using Xamarin.Forms;
using RepeatingWords;
using RepeatingWords.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(KeyButton), typeof(KeyButtonRender))]
namespace RepeatingWords.Droid
{
    public class KeyButtonRender : ButtonRenderer
    {
       //переоперделение элемента Button (все слова в своем регистре)
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var button = this.Control;
              button.SetAllCaps(false);
        }

    }
}