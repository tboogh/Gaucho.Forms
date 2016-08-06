using Xamarin.Forms;

namespace Gaucho.Forms.Core.Behaviors.Translation.Prism
{
    public class EntryTranslationBehavior : TranslationBehavior<Entry>
    {
        public override void TranslateTarget(string key, string format)
        {
            var translatedText = TranslationService?.Translate(key).ToUpperInvariant(); ;
            if (format == null)
            {
                Target.Text = translatedText;
            } else
            {
                Target.Text = string.Format(format, translatedText);
            }
        }
    }

    public class EntryPlaceholderTranslationBehavior : TranslationBehavior<Entry>
    {
        public override void TranslateTarget(string key, string format)
        {
            var translatedText = TranslationService?.Translate(key).ToUpperInvariant(); ;
            if (format == null)
            {
                Target.Placeholder = translatedText;
            } else
            {
                Target.Placeholder = string.Format(format, translatedText);
            }
        }
    }
}
