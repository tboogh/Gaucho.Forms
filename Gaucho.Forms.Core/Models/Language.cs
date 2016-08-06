using System.Collections.Generic;

namespace Gaucho.Forms.Core.Models
{
    public class Language : ILanguage
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public IDictionary<string, string> Translations { get; set; }
    }
}