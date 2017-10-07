using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegularExpressionEvaluator.Test
{
    [TestClass]
    public class RegularExpressionTest
    {

        [TestMethod]
        public void EmptyString_EmptyPattern_Match()
        {
            var pattern = string.Empty;
            var text = string.Empty;

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }
        
        [TestMethod]
        public void SingleLetter_ExactString_Match()
        {
            var pattern = "a";
            var text = "a";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }

        [TestMethod]
        public void OnlyLetters_ExactString_Match()
        {
            var pattern = "abc";
            var text = "abc";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }

        [TestMethod]
        public void OnlyLetters_DifferentEndLetter_NoMatch()
        {
            var pattern = "abc";
            var text = "abd";

            var regex = new RegularExpression(pattern);

            Assert.IsFalse(regex.IsMatch(text));
        }

        [TestMethod]
        public void Tab_Escaped_T_Match()
        {
            var pattern = @"\t";
            var text = "\t";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }
        
        [TestMethod]
        public void BashSlash_Escaped_BackSlash_Match()
        {
            var pattern = @"\\";
            var text = "\\";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }

        [TestMethod]
        public void OpenBrace_Escaped_OpenBrace_Match()
        {
            var pattern = @"\{";
            var text = "{";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }

        [TestMethod]
        public void OpenParenthese_Escaped_OpenParenthese_Match()
        {
            var pattern = @"\(";
            var text = "(";

            var regex = new RegularExpression(pattern);

            Assert.IsTrue(regex.IsMatch(text));
        }
    }
}
