using Xamarin.Forms;

namespace Gaucho.Forms.Core.Behaviors.Translation.Prism
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