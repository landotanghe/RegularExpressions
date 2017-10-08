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
        public void Pipe_Within_Parentheses()
        {
            TestThatRegularExpression.WithPattern("hello(world|test)")
                .Matches("helloworld")
                .Matches("hellotest")
                .DoesNotMatch("hello")
                .DoesNotMatch("world")
                .DoesNotMatch("test");
        }


        [TestMethod]
        public void Pipe_Outside_Parentheses()
        {
            TestThatRegularExpression.WithPattern("(hello)world|test")
                .Matches("helloworld")
                .Matches("test")
                .DoesNotMatch("hellotest")
                .DoesNotMatch("world")
                .DoesNotMatch("hello");
        }


        [TestMethod]
        public void Pipe_Within_MultipleParentheses()
        {
            TestThatRegularExpression.WithPattern("hello(world|(test| there))")
                .Matches("helloworld")
                .Matches("hellotest")
                .Matches("hello there")
                .DoesNotMatch("hello")
                .DoesNotMatch("world")
                .DoesNotMatch("test")
                .DoesNotMatch("there");
        }

        [TestMethod]
        public void Pipe_Escaped_LiteralPipe()
        {
            TestThatRegularExpression.WithPattern(@"\|").Matches("|");

            TestThatRegularExpression.WithPattern(@"a\|b").Matches("a|b");
        }
    }
}
