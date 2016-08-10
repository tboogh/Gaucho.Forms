using Gaucho.Forms.Core.Models;
using Prism.Events;

namespace Gaucho.Forms.Prism.Services
{
    public class LanguageChangedEvent : PubSubEvent<ILanguage>
    {

    }

    public class TranslationService : Core.Services.TranslationService
    {
        private readonly IEventAggregator _eventAggregator;
        public TranslationService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public override void LanguageChanged(ILanguage language)
        {
            _eventAggregator.GetEvent<LanguageChangedEvent>().Publish(language);
        }
    }
}