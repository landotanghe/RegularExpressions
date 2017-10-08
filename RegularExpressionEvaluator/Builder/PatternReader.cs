using RegularExpressionEvaluator.Builder;
using System;
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
        public class Repeat
        {
            public const char ZerorOrMore = '*';
            public const char Open = '{';
            public const char Close = '}';
            public const char Separator = ',';
        }
        private readonly static char[] EscapableCharactersWithSpecialMeaning = {
            EscapeChar, '{', '}', StartNewSequence, EndNewSequence, OrOperator,
            Repeat.ZerorOrMore, Repeat.Open, Repeat.Close };

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

        public Repetitions ReadRepetions()
        {
            var openTagPosition = nextSymbolPosition;
            var closingTagPosition = _input.IndexOf(Repeat.Close, openTagPosition);
            nextSymbolPosition = closingTagPosition + 1;
            if (closingTagPosition == -1)
                throw new Exception($"Expected symbol {Repeat.Close}");

            var separatorPosition = _input.IndexOf(Repeat.Separator, openTagPosition);
            if(separatorPosition == -1)
            {
                var exactAmount = ReadAmount(openTagPosition + 1, closingTagPosition);
                return new Repetitions
                {
                    Minimum = exactAmount,
                    Maximum = exactAmount
                };

            }
            else
            {
                var minAmount = ReadAmount(openTagPosition + 1, separatorPosition);
                var maxAmount = ReadAmount(separatorPosition + 1, closingTagPosition);
                return new Repetitions
                {
                    Minimum = minAmount,
                    Maximum = maxAmount
                };
            }
        }

        private int ReadAmount(int startPosition, int firstNonNumberPosition)
        {
            var numberLength = firstNonNumberPosition - startPosition;
            var exactAmountString = _input.Substring(startPosition, numberLength);
            var exactAmount = int.Parse(exactAmountString);
            return exactAmount;
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
            if(nextSymbol == Repeat.ZerorOrMore)
            {
                return new Token(Repeat.ZerorOrMore, TokenType.Repeat);
            }
            if(nextSymbol == Repeat.Open)
            {
                return new Token(Repeat.Open, TokenType.OpenRepeat);
            }
            return new Token(nextSymbol, TokenType.Character);
        }

        private bool HasUnprocessedInput()
        {
            return nextSymbolPosition < _input.Length;
        }
    }
}
