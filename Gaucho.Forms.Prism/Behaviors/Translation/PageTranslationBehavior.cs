using Xamarin.Forms;

namespace Gaucho.Forms.Prism.Behaviors.Translation
{
    public class PageTranslationBehavior : TranslationBehavior<Page>
    {
        public override void TranslateTarget(string key, string format)
        {
            var translatedText = TranslationService?.Translate(key)?.ToUpperInvariant();
            if (format == null)
            {
                Target.Title = translatedText;
            } else
            {
                Target.Title = string.Format(format, translatedText);
            }
        }
    }
}