using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaucho.Forms.Core.Models;
using Gaucho.Forms.Core.Services;
using NSubstitute;

namespace Gaucho.Forms.Core.UnitTests.Services
{
    [TestFixture]
    public class TestTranslationService
    {
        [Test]
        public void CurrentLanguage_Set_CurrentLanguageInvoked()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(language);

            // Act
            translationService.CurrentLanguage = language;

            // Assert
            translationService.Received(1)
                .LanguageChanged(Arg.Is<ILanguage>(l => l == language));
        }

        [Test]
        public void CurrentLanguage_Get_ReturnsSetLanguage()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(language);

            // Act
            translationService.CurrentLanguage = language;

            var setLanguage = translationService.CurrentLanguage;

            // Assert
            Assert.AreEqual(language, setLanguage);
        }

        [Test]
        public void IsInitialized_NoLanguages_ReturnsFalse()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(null);

            // Act
            var init = translationService.IsIntitialized;

            // Assert
            Assert.IsFalse(init);
        }

        [Test]
        public void IsInitialized_WithLanguages_ReturnsTrue()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(language);

            // Act
            var init = translationService.IsIntitialized;

            // Assert
            Assert.IsTrue(init);
        }

        [Test]
        public void AvailableLanguages_CorrectLangaugeList_ReturnListWithTestLanguage()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(language);

            // Act
            var langauges = translationService.AvailableLanguages();

            // Assert
            Assert.IsNotNull(langauges.FirstOrDefault(x => x == language));
        }

        [Test]
        public void Translate_ValidKey_ReturnsTranslation()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(language);
            translationService.CurrentLanguage = language;

            // Act
            var translation = translationService.Translate("TestKey");

            // Assert
            Assert.AreEqual("TestValue", translation);
        }

        [Test]
        public void Translate_InvalidKey_ReturnsKey()
        {
            // Arrange
            var language = CreateTestLanguage();
            var translationService = CreateTranslationService(language);

            // Act
            var translation = translationService.Translate("InvalidTestKey");

            // Assert
            Assert.AreEqual("InvalidTestKey", translation);
        }

        private static TranslationService CreateTranslationService(Language language)
        {
            var translationService = Substitute.ForPartsOf<TranslationService>();
            if (language != null)
            {
                translationService.Languages[language.Code] = language;
            }

            return translationService;
        }

        private static Language CreateTestLanguage()
        {
            var language = new Language {Code = "code", DisplayName = "TestLanguage", Translations = new Dictionary<string, string>() {{"TestKey", "TestValue"}}};
            return language;
        }
    }
}