using System;
using Xamarin.Forms;

using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
using System.Linq;

[assembly: Dependency(typeof(RepeatingWords.WinPhone.TextToSpeechImplementation))]
namespace RepeatingWords.WinPhone
{
    public class TextToSpeechImplementation : ITextToSpeech
    {

        public TextToSpeechImplementation() { }

        public async void Speak(string text, string lang)
        {//переменная для воспроизведения , запуска плеера
            var mediaElement = new MediaElement();
            //переменна синтез речи
            var synthesizer = new SpeechSynthesizer();
            using (var speaker = new SpeechSynthesizer())
            {//проверяем наличие языка в системе если есть то устанавливаем его для воспроизведения иначе уст анг
                if(SpeechSynthesizer.AllVoices.Where(x=>x.Language==lang).FirstOrDefault()!=null)
                {
                    speaker.Voice = (SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Female && x.Language.Contains(lang)));
                }
                else
                {
                    speaker.Voice = (SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Female && x.Language.Contains("GB")));
                }
                synthesizer.Voice = speaker.Voice;
            }
            //создаем поток воспроизведения
            SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(text);

          //добавляем в проигрыватель
            mediaElement.SetSource(stream, stream.ContentType);
            //запускаем проигрываиель
            mediaElement.Play();
        }
    }
}