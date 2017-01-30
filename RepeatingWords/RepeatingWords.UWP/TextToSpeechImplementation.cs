using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(RepeatingWords.UWP.TextToSpeechImplementation))]
namespace RepeatingWords.UWP
{
    public class TextToSpeechImplementation : ITextToSpeech
    {
        public void Speak(string text, string lang)
        {
            throw new NotImplementedException();
        }
    }
}
