using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RegularExpressionEvaluator.Test
{
    [TestClass]
    public class RegularExpressionOrTest
    {
        [TestMethod]
        public void PipeBetweenLetters_OneOfTheLetters()
        {
            TestThatRegularExpression.WithPattern("a|b|c")
                .Matches("a")
                .Matches("b")
                .Matches("c")
                .DoesNotMatch("|")
                .DoesNotMatch("d")
                .DoesNotMatch(String.Empty);
        }

        [TestMethod]
        public void PipeAfterEmptyString_EmptyString_Matches()
        {
            TestThatRegularExpression.WithPattern("a|")
                .Matches("a")
                .Matches(String.Empty);
        }

        [TestMethod]
        public void PipeBeforeEmptyString_EmptyString_Matches()
        {
            TestThatRegularExpression.WithPattern("|a")
                .Matches("a")
                .Matches(String.Empty);
        }

        [TestMethod]
        public void PipeBetweenStrings_OneOfTheLetters()
        {
            TestThatRegularExpression.WithPattern("hello|world|test")
                .Matches("hello")
                .Matches("world")
                .Matches("test")
                .DoesNotMatch("|")
                .DoesNotMatch(String.Empty);
        }

        [TestMethod]
        public void Pipe_Escaped_LiteralPipe()
        {
            TestThatRegularExpression.WithPattern(@"\|").Matches("|");

            TestThatRegularExpression.WithPattern(@"a\|b").Matches("a|b");
        }
    }
}
