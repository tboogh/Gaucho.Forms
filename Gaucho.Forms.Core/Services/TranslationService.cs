using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaucho.Forms.Core.Models;

namespace Gaucho.Forms.Core.Services
{
    public class TranslationService : ITranslationService
    {
        private ILanguage _currentLanguage;
        private Task _loadLanguagesTask;

        public TranslationService()
        {
            Languages = new Dictionary<string, Language>();
        }

        public bool IsIntitialized => Languages.Count > 0;
        public  Dictionary<string, Language> Languages { get; }

        public ILanguage CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                _currentLanguage = value;
                LanguageChanged(value);
            }
        }

        public virtual Task LoadLanguages()
        {
            if (_loadLanguagesTask?.IsCompleted ?? true)
            {
                _loadLanguagesTask = PerformLoadLanguaged();
            }
            return _loadLanguagesTask;
        }

        public virtual void LanguageChanged(ILanguage languageCode)
        {
            
        }

        public List<ILanguage> AvailableLanguages()
        {
            return Languages.Select(x => (ILanguage)x.Value).ToList();
        }

        public string Translate(string key)
        {
            try
            {
                return CurrentLanguage.Translations[key];
            }
            catch (Exception)
            {
                return key;
            }
        }

        protected virtual Task PerformLoadLanguaged()
        {
            return Task.FromResult(0);
        }
    }
}