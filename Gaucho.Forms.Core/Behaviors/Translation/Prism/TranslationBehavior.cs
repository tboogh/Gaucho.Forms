using Gaucho.Forms.Core.Models;
using Gaucho.Forms.Core.Services;
using Gaucho.Forms.Core.Services.Prism;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity;
using Xamarin.Forms;

namespace Gaucho.Forms.Core.Behaviors.Translation.Prism
{
    public abstract class TranslationBehavior<T> : Translation.TranslationBehavior<T> where T : BindableObject
    {
        public TranslationBehavior()
        {
            PrismApplication app = (PrismApplication) Application.Current;
            var container = app?.Container;
            var eventAggregator = container?.Resolve<IEventAggregator>();
            TranslationService = container?.Resolve<ITranslationService>();

            eventAggregator?.GetEvent<LanguageChangedEvent>()
                .Subscribe(OnLanguageChanged);
        }

        public ITranslationService TranslationService { get; set; }

        public void OnLanguageChanged(ILanguage language)
        {
            TranslateTarget(Key, Format);
        }
    }
}