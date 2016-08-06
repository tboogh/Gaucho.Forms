using System.Collections.Generic;

namespace Gaucho.Forms.Core.Models
{
    public interface ILanguage
    {
        string Code { get; set; }
        string DisplayName { get; set; }
        IDictionary<string, string> Translations { get; set; }
    }
}
