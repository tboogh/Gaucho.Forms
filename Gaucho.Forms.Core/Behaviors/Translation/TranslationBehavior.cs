using Xamarin.Forms;

namespace Gaucho.Forms.Core.Behaviors.Translation
{
    public abstract class TranslationBehavior<T> : Behavior<T> where T : BindableObject
    {
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(string), typeof(TranslationBehavior<T>), default(string));
        public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(TranslationBehavior<T>), default(string));

        public string Key
        {
            get { return (string) GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public T Target { get; set; }
        public abstract void TranslateTarget(string key, string format);

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            Target = bindable;

            if (Target == null)
                return;

            TranslateTarget(Key, Format);
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            Target = null;
        }
    }
}