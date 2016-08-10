using Gaucho.Forms.Core.Models;
using Gaucho.Forms.Core.Services;
using Gaucho.Forms.Prism.Services;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity;
using Xamarin.Forms;

namespace Gaucho.Forms.Prism.Views.Cells
{
    public class TranslatedTextCell : TextCell
    {
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(string), typeof(TranslatedTextCell), "");
        private readonly ITranslationService _translationService;

        public string Key
        {
            get { return (string) GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public TranslatedTextCell()
        {
            IUnityContainer container = ((PrismApplication) Application.Current)?.Container;
            if (container == null)
                return;

            IEventAggregator eventAggregator = container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<LanguageChangedEvent>()
                .Subscribe(LanguageChanged);

            _translationService = container.Resolve<ITranslationService>();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Key")
            {
                SetText(Key);
            }
        }

        private void LanguageChanged(ILanguage language)
        {
            if (_translationService == null)
            {
                SetText(Key);
            }
        }

        private void SetText(string key)
        {
            if (_translationService == null)
                return;

            Text = _translationService.Translate(key);
        }
    }
}