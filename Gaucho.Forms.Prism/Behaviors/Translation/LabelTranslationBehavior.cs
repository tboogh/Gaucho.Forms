using Xamarin.Forms;

namespace Gaucho.Forms.Prism.Behaviors.Translation
{
    public class LabelTranslationBehavior : TranslationBehavior<Label>
    {
        public override void TranslateTarget(string key, string format)
        {
            var translatedText = TranslationService?.Translate(key).ToUpperInvariant(); ;
            if (format == null)
            {
                Target.Text = translatedText;
            }
            else
            {
                Target.Text = string.Format(format, translatedText);
            }
        }
    }
}