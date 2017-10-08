using System.Linq;

namespace RegularExpressionEvaluator
{
    internal class PatternReader
    {
        public const char EscapeChar = '\\';
        public const char OrOperator = '|';
        public const char StartNewSequence = '(';
        public const char EndNewSequence = ')';
        public const char Tab = '\t';
        private readonly static char[] EscapableCharactersWithSpecialMeaning = {
            EscapeChar, '{', '}', StartNewSequence, EndNewSequence, OrOperator };

        private int nextSymbolPosition = 0;
        private string _input;
        public PatternReader(string input)
        {
            _input = input;
        }

        public Token PeekNextToken()
        {
            if (!HasUnprocessedInput())
                return new Token(' ', TokenType.EndOfInput);

            var nextSymbol = _input[nextSymbolPosition];
            if (nextSymbol == EscapeChar)
            {
                return PeekAtEscapedToken();
            }
            else
            {
                return CreateUnescapedToken(nextSymbol);
            }
        }

        public Token ReadNextToken()
        {
            if (!HasUnprocessedInput())
                return new Token(' ', TokenType.EndOfInput);

            var nextSymbol = _input[nextSymbolPosition];
            if (nextSymbol == EscapeChar)
            {
                var token = PeekAtEscapedToken();
                nextSymbolPosition+=2;
                return token;
            }
            else
            {
                nextSymbolPosition++;
                return CreateUnescapedToken(nextSymbol);
            }
        }

        private Token PeekAtEscapedToken()
        {
            if (!HasUnprocessedInput())
                throw new System.Exception("expected input after \\");

            var nextSymbol = _input[nextSymbolPosition+1];
            if (nextSymbol == 't')
            {
                return new Token(Tab, TokenType.Character);
            }
            else if (EscapableCharactersWithSpecialMeaning.Contains(nextSymbol))
            {
                return new Token(nextSymbol, TokenType.Character);
            }
            else
            {
                throw new System.Exception($"unexpected input symbol {nextSymbol} at position {nextSymbolPosition + 1}");
            }
        }

        private static Token CreateUnescapedToken(char nextSymbol)
        {
            if (nextSymbol == OrOperator)
            {
                return new Token(OrOperator, TokenType.OrOperator);
            }
            if (nextSymbol == StartNewSequence)
            {
                return new Token(StartNewSequence, TokenType.StartNewSequence);
            }
            if (nextSymbol == EndNewSequence)
            {
                return new Token(EndNewSequence, TokenType.EndNewSequence);
            }
            return new Token(nextSymbol, TokenType.Character);
        }

        private bool HasUnprocessedInput()
        {
            return nextSymbolPosition < _input.Length;
        }
    }
}
