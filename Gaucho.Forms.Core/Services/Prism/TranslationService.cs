using System.Threading.Tasks;
using Gaucho.Forms.Core.FileSystem;
using Gaucho.Forms.Core.Models;
using Newtonsoft.Json;
using Prism.Events;

namespace Gaucho.Forms.Core.Services.Prism
{
    public class LanguageChangedEvent : PubSubEvent<ILanguage>
    {

    }

    public class TranslationService : Services.TranslationService
    {
        private readonly IFileService _fileService;
        private readonly IEventAggregator _eventAggregator;
        public TranslationService(IFileService fileService, IEventAggregator eventAggregator)
        {
            _fileService = fileService;
            _eventAggregator = eventAggregator;
        }

        protected override Task PerformLoadLanguaged()
        {
            return LoadLocalTranslations();
        }

        public override void LanguageChanged(ILanguage languageCode)
        {
            _eventAggregator.GetEvent<LanguageChangedEvent>().Publish(languageCode);
        }

        private async Task LoadLocalLanguage(string filename)
        {
            var text = await _fileService.ReadStringAsync($"languages/{filename}.json", StorageLocation.AppResource);
            if (text == null)
                return;
            var language = JsonConvert.DeserializeObject<Language>(text);
            if (language == null)
                return;
            Languages[language.Code] = language;
        }

        private async Task LoadLocalTranslations()
        {
            await LoadLocalLanguage("en");
            await LoadLocalLanguage("sv");
        }
    }
}