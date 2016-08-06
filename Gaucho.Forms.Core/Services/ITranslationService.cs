using System.Collections.Generic;
using System.Threading.Tasks;
using Gaucho.Forms.Core.Models;

namespace Gaucho.Forms.Core.Services
{
    public interface ITranslationService
    {
        bool IsIntitialized { get; }

        Task LoadLanguages();
        ILanguage CurrentLanguage { get; set; }
        List<ILanguage> AvailableLanguages();
        string Translate(string key);
    }
}