using NHS111.Utils.Extensions;
using NUnit.Framework;

namespace NHS111.Utils.Test.Extensions
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        public void FirstToUpper_When_Given_Null_Should_Return_Null()
        {
            string s = null;
            Assert.That(s.FirstToUpper(), Is.EqualTo(null));
        }

        [Test]
        public void FirstToUpper_When_Given_An_Empty_String_Should_Return_An_Empty_String()
        {
            string s = "";
            Assert.That(s.FirstToUpper(), Is.EqualTo(""));
        }

        [Test]
        public void FirstToUpper_When_Given_a_Should_Return_A()
        {
            string s = "a";
            Assert.That(s.FirstToUpper(), Is.EqualTo("A"));
        }

        [Test]
        public void FirstToUpper_When_Given_A_Should_Return_A()
        {
            string s = "A";
            Assert.That(s.FirstToUpper(), Is.EqualTo("A"));
        }

        [Test]
        public void FirstToUpper_When_Given_ab_Should_Return_Ab()
        {
            string s = "ab";
            Assert.That(s.FirstToUpper(), Is.EqualTo("Ab"));
        }

        [Test]
        public void FirstToUpper_When_Given_Ab_Should_Return_Ab()
        {
            string s = "Ab";
            Assert.That(s.FirstToUpper(), Is.EqualTo("Ab"));
        }

        [Test]
        public void FirstToUpper_When_Given_abc_def_Should_Return_Abc_def()
        {
            string s = "abc def";
            Assert.That(s.FirstToUpper(), Is.EqualTo("Abc def"));
        }

        [Test]
        public void FirstToUpper_When_Given_Abc_def_Should_Return_Abc_def()
        {
            string s = "Abc def";
            Assert.That(s.FirstToUpper(), Is.EqualTo("Abc def"));
        }
    }
}