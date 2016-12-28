using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Xamarin.Forms;

namespace Gaucho.Forms.Core.UnitTests.Behaviors.Translation
{
    public class TestBindableObject : BindableObject
    {
        
    }

    public class TranslationBehaviour : Core.Behaviors.Translation.TranslationBehavior<TestBindableObject>
    {
        public override void TranslateTarget(string key, string format)
        {
            OnTranslateTarget(key, format);
        }

        public virtual void CallOnAttachedTo(BindableObject bindable)
        {
            OnAttachedTo(bindable);
        }

        public virtual void CallOnDetachingFrom(BindableObject bindable)
        {
            OnDetachingFrom(bindable);
        }

        public virtual void OnTranslateTarget(string key, string format)
        {
            
        }
    }

    [TestFixture]
    public class TestTranslationBehavior
    {
        [Test]
        public void OnAttachTo_WithBindable_TargetSetToBindable()
        {
            // Arrange
            var testBindable = new TestBindableObject();
            var behavior = new TranslationBehaviour();

            // Act
            behavior.CallOnAttachedTo(testBindable);

            // Assert
            Assert.AreEqual(testBindable, behavior.Target);
        }

        [Test]
        public void OnAttachTo_WithNull_TargetSetToNull()
        {
            // Arrange
            var behavior = new TranslationBehaviour();

            // Act
            behavior.CallOnAttachedTo(null);

            // Assert
            Assert.IsNull(behavior.Target);
        }

        [Test]
        public void OnDetachingFrom_WithBindable_TargetSetToNull()
        {
            // Arrange
            var testBindable = new TestBindableObject();
            var behavior = new TranslationBehaviour();
            behavior.CallOnAttachedTo(testBindable);

            // Act
            behavior.CallOnDetachingFrom(testBindable);

            // Assert
            Assert.IsNull(behavior.Target);
        }

        [Test]
        public void TranslateTarget_OnAttachedTo_TranslateTargetCalled()
        {
            // Arrange
            var testBindable = new TestBindableObject();
            var behavior = Substitute.ForPartsOf<TranslationBehaviour>();

            // Act
            behavior.CallOnAttachedTo(testBindable);

            // Assert
            behavior.Received().OnTranslateTarget(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void TranslateTarget_OnAttachedTo_TranslateTargetKeyCorrect()
        {
            // Arrange
            var testBindable = new TestBindableObject();
            var behavior = Substitute.ForPartsOf<TranslationBehaviour>();
            behavior.Key = "TestKey";
            // Act
            behavior.CallOnAttachedTo(testBindable);

            // Assert
            behavior.Received().OnTranslateTarget(Arg.Is<string>(s => s == "TestKey"), Arg.Any<string>());
        }

        [Test]
        public void TranslateTarget_OnAttachedTo_TranslateTargetFormatCorrect()
        {
            // Arrange
            var testBindable = new TestBindableObject();
            var behavior = Substitute.ForPartsOf<TranslationBehaviour>();
            behavior.Format = "TestFormat";
            // Act
            behavior.CallOnAttachedTo(testBindable);
            
            // Assert
            behavior.Received().OnTranslateTarget(Arg.Any<string>(), Arg.Is<string>(s => s == "TestFormat"));
        }
    }
}
