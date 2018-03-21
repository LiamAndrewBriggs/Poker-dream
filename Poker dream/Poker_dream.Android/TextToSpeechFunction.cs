using Android.App;
using Android.Speech.Tts;
using static Poker_dream.Cards;
using static Poker_dream.Settings;
using static Poker_dream.Play;

[assembly: Xamarin.Forms.Dependency(typeof(Poker_dream.Droid.TextToSpeechFunction))]
namespace Poker_dream.Droid 
{
    class TextToSpeechFunction : Java.Lang.Object, ITextToSpeech, ITextSettingsToSpeech, IPlaySettingsToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(Application.Context, this);
            }
            else
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
    }
}