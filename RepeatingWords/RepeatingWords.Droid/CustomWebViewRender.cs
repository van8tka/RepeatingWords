using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using RepeatingWords.Droid;

[assembly: ExportRenderer(typeof(RepeatingWords.CustomView.CustomWebView),typeof(CustomWebViewRender))]
namespace RepeatingWords.Droid
{

  class CustomWebViewRender:WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
           base.OnElementChanged(e);
            if (Control == null)
            {
                // Instantiate the native control and assign it to the Control property with
                // the SetNativeControl method
            }

            if (e.OldElement != null)
            {
                // Unsubscribe from event handlers and cleanup any resources
            }

            if (e.NewElement != null)
            {
                string ua = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
                Control.Settings.UserAgentString = ua;
            }
        }
    }
}