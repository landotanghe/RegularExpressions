using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegularExpressionEvaluator.Test
{
    [TestClass]
    public class RegularExpressionTest
    {
        [TestMethod]
        public void EmptyString_EmptyPattern_Match()
        {
            TestThatRegularExpression.WithPattern(string.Empty)
                .Matches(string.Empty);
        }


        [TestMethod]
        public void Numbers_ExactString()
        {
            TestThatRegularExpression.WithPattern("1")
                .Matches("1")
                .DoesNotMatch("b");
        }

        [TestMethod]
        public void SingleLetter_ExactString()
        {
            TestThatRegularExpression.WithPattern("a")
                .Matches("a")
                .DoesNotMatch("b");
        }

        [TestMethod]
        public void OnlyLetters_ExactString_Match()
        {
            TestThatRegularExpression.WithPattern("abc")
                .Matches("abc")
                .DoesNotMatch("abd");
        }
        
        [TestMethod]
        public void Tab_Escaped_T_Match()
        {
            TestThatRegularExpression.WithPattern(@"\t").Matches("\t");
        }
        
        [TestMethod]
        public void BackSlash_Escaped_BackSlash_Match()
        {
            TestThatRegularExpression.WithPattern(@"\\").Matches("\\");
        }

        [TestMethod]
        public void OpenBrace_Escaped_OpenBrace_Match()
        {
            TestThatRegularExpression.WithPattern(@"\{").Matches("{");
        }

        [TestMethod]
        public void OpenParenthese_Escaped_OpenParenthese_Match()
        {
            TestThatRegularExpression.WithPattern(@"\(").Matches("(");
        }
    }
}
