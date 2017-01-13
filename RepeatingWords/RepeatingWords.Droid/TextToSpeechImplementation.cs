using System.Collections.Generic;
using Android.Runtime;
using Xamarin.Forms;
using Android.Speech.Tts;
using RepeatingWords.Droid;

[assembly: Dependency(typeof(TextToSpeechImplementation))]

namespace RepeatingWords.Droid
{
    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {

        TextToSpeech speaker;
        string toSpeak;
        string Lang;

        public TextToSpeechImplementation() { }

        //IOnInitListener implementation
        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if(status.Equals (OperationResult.Success))
            {
                var p = new Dictionary<string, string>();
                var locale = new Java.Util.Locale(Lang);
                speaker.SetLanguage(locale);
                speaker.Speak(toSpeak, QueueMode.Flush, p);
            }
        }

        public void Speak(string text, string languageIso)
        {
            var ctx = Forms.Context;
            toSpeak = text;
            Lang = languageIso;   
            if (speaker == null)
            {
                speaker = new TextToSpeech(ctx, this);            
            }
            else
            {
                var p = new Dictionary<string, string>();
                var locale = new Java.Util.Locale(languageIso);
                speaker.SetLanguage(locale);
                speaker.Speak(toSpeak, QueueMode.Flush, p);
               
            }
        }
    }
}