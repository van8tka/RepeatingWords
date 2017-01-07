using Android.Gms.Ads;
using Xamarin.Forms;
using RepeatingWords;
using RepeatingWords.Droid;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace RepeatingWords.Droid
{

    public class AdMobRenderer : ViewRenderer<AdMobView, AdView>
        {
            protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
            {
                base.OnElementChanged(e);

            if (Control == null)
            {
                var ad = new AdView(Forms.Context);
                    ad.AdSize = AdSize.Banner;
                    ad.AdUnitId = "ca-app-pub-5351987413735598/7586925463";

                    var requestbuilder = new AdRequest.Builder();
                    ad.LoadAd(requestbuilder.Build());

                    SetNativeControl(ad);
            }
        }
        }
    }